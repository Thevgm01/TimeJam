using System.Collections.Generic;
using UnityEngine;

public class GameTextParser
{
    /* Valid commands
     * 
     * [GOTO: <id>]
     * Reveals the part of the story tagged with the ID
     * 
     * [MINOR_CHOICE: , , ,]
     * Choices that the player can make, comma separated
     * Each choice can end with a GOTO to designate where in the script the player should go
     *                 start with [SECRET: <item>] for it to only appear when a certain item is in the inventory
     * 
     * [MAJOR_CHOICE: , , ,]
     * As MINOR_CHOICE, but items can also be sent through time from one place to another
     * Start with [TIME: <item>] to only appear when that item is added or removed through time
     * 
     * [GET: <item>]
     * Add the item to the current inventory
     * 
     * [LOSE: <item>]
     * Lose the item, if it was in your inventory
     * 
     * [BURN: <text>]
     * The text is burned into the screen
     * 
     * [DEAD]
     * Death flag is set, must take a different action to proceed
     * 
     * [<anything>]
     * The text ID of this paragraph, a point for a GOTO to connect to
     */

    private struct Command
    {
        public string op;
        public string data;
    }

    private struct Goto
    {
        public TextNode source;
        public string destinationId;
    }

    private class Linker
    {
        public bool ShouldSkip { get; private set; } = false;
        private bool shouldSkipNext = false;
        public bool ShouldLink { get; private set; } = true;
        private bool shouldLinkNext = true;

        public void StopLinking()
        {
            shouldSkipNext = true;
            shouldLinkNext = false;
        }

        public void StartLinking()
        {
            ShouldSkip = false;
            shouldSkipNext = false;
            shouldLinkNext = true;
        }

        public void Update()
        {
            ShouldLink = shouldLinkNext;
            ShouldSkip = shouldSkipNext;
        }

        public void Link(Node parent, Node child)
        {
            if (parent == null || child == null)
                return;
            parent.TrySetChild(child);
            child.TrySetParent(parent);
            Debug.Log("Linking " + parent.Line + " with " + child.Line);
        }
    }

    public Dictionary<string, Node> namedNodes;
    public Node FirstNode => namedNodes["ORIGIN"];
    private string[] lines;

    Linker linker;

    public GameTextParser(TextAsset gameText)
    {
        namedNodes = new Dictionary<string, Node>();

        Node previousNode = null;

        // Prevent linking nodes when the current node should terminate
        linker = new Linker();

        List<Goto> gotos = new List<Goto>();

        lines = gameText.text.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();

            // Ignore empty lines
            if (line == "" || line[0] == '#')
            {
                continue;
            }

            // Find all top-level commands
            List<string> commandStrings;
            try
            {
                line = ExtractCommands(line, out commandStrings);
            }
            catch
            {
                Debug.LogError("Improper command brackets on line " + i + " of game text.");
                return;
            }

            // Set up base variables
            Node newNode = null;
            ITextDisplayable specialText = null;
            string id = "";
            string gotoDestination = "";

            // Update the state of the linker
            linker.Update();

            // Parse commands
            for (int j = 0; j < commandStrings.Count; ++j)
            {
                string commandString = commandStrings[j];
                Command command = GetCommand(commandString);

                if (command.op == "MINOR_CHOICE" || command.op == "MAJOR_CHOICE")
                {
                    newNode = CreateChoiceNode(command.data, gotos);
                    linker.StopLinking();
                }
                else if (command.op == "GOTO")
                {
                    gotoDestination = command.data;
                    linker.StopLinking();
                }
                else if (command.op == "BURN")
                {

                }
                else if (command.op == "GET" || command.op == "LOSE")
                {
                    TextInventoryModifier.Operation opType =
                        command.op == "GET" ? TextInventoryModifier.Operation.Add :
                        TextInventoryModifier.Operation.Remove;
                    specialText = new TextInventoryModifier(null, command.data, opType);
                }
                else if (command.op == "DEAD")
                {
                    specialText = new TextDead();
                    linker.StopLinking();
                }
                else if (command.op == "ID")
                {
                    id = commandString;
                    linker.StartLinking();
                }
                else
                {
                    Debug.LogError("Unknown command type: " + command.op + " on line " + i + " of game text.");
                }
            }

            // If no ID was set and a previous node was an end point, there's no way to reach the current node
            if (linker.ShouldSkip)
            {
                Debug.Log("Discarding " + (i + 1));
                continue;
            }

            // If the new node wasn't created by a command, create it now as a basic text node
            if (newNode == null) {
                // If some special text was assigned, use it
                if (specialText != null)
                    newNode = new TextNode(specialText);
                // Otherwise use the line itself
                else
                    newNode = new TextNode(line);
            }

            Debug.Log("Creating " + (i + 1));
            newNode.SetLine(i + 1);

            // Now that the node exists, if an ID was set, assign it to the list
            if (id != "")
                namedNodes[id] = newNode;

            // Set link the previous node and the current node
            if (previousNode != null && linker.ShouldLink)
                linker.Link(previousNode, newNode);

            // Keep track of the Goto and deal with it later
            if (gotoDestination != "")
                gotos.Add(new Goto { source = (TextNode)newNode, destinationId = gotoDestination });

            // Advance the current node tracker
            previousNode = newNode;
        }

        // Now that all IDs in the text have been registered, go through the gotos and reassign parents/children as necessary
        Debug.Log("----- GOTO PARSING -----");
        foreach (Goto g in gotos)
        {
            if (!namedNodes.ContainsKey(g.destinationId))
                Debug.LogError("Id " + g.destinationId + " is not defined in the game text.");
            linker.Link(g.source, namedNodes[g.destinationId]);
        }
    }

    private ChoiceNode CreateChoiceNode(string choiceCommand, List<Goto> gotos)
    {
        string[] choicesStringArray = choiceCommand.Split(',');
        ChoiceNode choiceNode = new ChoiceNode();
        for (int i = 0; i < choicesStringArray.Length; ++i)
        {
            string choiceString = ExtractCommands(choicesStringArray[i], out var commands);
            TextNode textNode = new TextNode(choiceString);
            linker.Link(choiceNode, textNode);

            for (int j = 0; j < commands.Count; ++j)
            {
                string commandString = commands[j];
                Command command = GetCommand(commandString);

                if (command.op == "GOTO")
                {
                    gotos.Add(new Goto { source = textNode, destinationId = command.data });
                }
            }
        }
        return choiceNode;
    }

    private Command GetCommand(string command)
    {
        Command c;
        if (command.Contains(":")) // Command has data that needs to be handled
        {
            int index = command.IndexOf(":");
            c.op = command.Substring(0, index).Trim();
            c.data = command.Substring(index + 1).Trim();
        }
        else
        {
            c.op = "ID";
            c.data = command;
        }
        return c;
    }

    private string ExtractCommands(string line, out List<string> commands)
    {
        int startIndex = -1;
        int bracketCounter = 0;
        commands = new List<string>();
        for (int i = 0; i < line.Length; ++i)
        {
            if (line[i] == '[')
            {
                ++bracketCounter;
                if (startIndex < 0)
                {
                    startIndex = i;
                }
            }
            else if (line[i] == ']')
            {
                --bracketCounter;
                if (bracketCounter == 0)
                {
                    int leftIndex = startIndex;
                    int rightIndex = i;
                    int length = rightIndex - leftIndex;
                    string command = line.Substring(leftIndex + 1, length - 1);

                    if (command.StartsWith("BURN"))
                    {
                        line = line.Substring(0, leftIndex) + command.Split(':')[1] + line.Substring(rightIndex + 1);
                    }
                    else
                    {
                        line = line.Substring(0, leftIndex) + line.Substring(rightIndex + 1);
                    }

                    commands.Add(command);
                    startIndex = -1;
                }
            }
        }
        line = line.Trim();
        while (line.Contains("  ")) 
            line = line.Replace("  ", " ");
        return line;
    }
}