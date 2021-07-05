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

        public Command(string op, string data)
        {
            this.op = op;
            this.data = data;
        }
    }

    private struct Goto
    {
        public TextNode parent;
        public string destinationId;

        public Goto(TextNode parent, string destinationId)
        {
            this.parent = parent;
            this.destinationId = destinationId;
        }
    }

    public Dictionary<string, TextNode> namedNodes;
    public TextNode FirstNode => namedNodes["ORIGIN"];
    private string[] lines;

    public GameTextParser(TextAsset gameText)
    {
        namedNodes = new Dictionary<string, TextNode>();

        TextNode curNode = null;
        List<Goto> allGotos = new List<Goto>();

        lines = gameText.text.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();

            if (line == "")
            {
                continue;
            }

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

            TextNode newNode = null;
            ITextDisplayable text = null;
            string id = "";

            for (int j = 0; j < commandStrings.Count; ++j)
            {
                string commandString = commandStrings[j];
                Command command = GetCommand(commandString);

                if (command.op == "MINOR_CHOICE" || command.op == "MAJOR_CHOICE")
                {
                    newNode = CreateChoiceNode(command.data, allGotos);
                }
                else if (command.op == "GOTO")
                {
                    allGotos.Add(new Goto(curNode, command.data));
                }
                else if (command.op == "BURN")
                {

                }
                else if (command.op == "GET" || command.op == "LOSE")
                {
                    TextInventoryModifier.Operation opType =
                        command.op == "GET" ? TextInventoryModifier.Operation.Add :
                        TextInventoryModifier.Operation.Remove;
                    text = new TextInventoryModifier(null, command.data, opType);
                }
                else if (command.op == "DEAD")
                {
                    text = new TextDead();
                }
                else if (command.op == "ID")
                {
                    id = commandString;
                }
                else
                {
                    Debug.LogError("Unknown command type: " + command.op + " on line " + i + " of game text.");
                }
            }

            if (newNode != null)
                curNode = newNode;
            else if (text != null)
                curNode = new TextNode(text, id, curNode);
            else
                curNode = new TextNode(line, id, curNode);
            if (id != "")
                namedNodes[id] = curNode;
        }

        foreach (Goto g in allGotos)
        {
            if (!namedNodes.ContainsKey(g.destinationId))
                Debug.LogError("Id " + g.destinationId + " is not defined in the game text.");
            g.parent.child = namedNodes[g.destinationId];
            namedNodes[g.destinationId].parent = g.parent;
        }
    }

    private ChoiceNode CreateChoiceNode(string choiceCommand,  List<Goto> allGotos)
    {
        string[] choicesStringArray = choiceCommand.Split(' ');
        ChoiceNode choiceNode = new ChoiceNode();
        TextNode[] choices = new TextNode[choicesStringArray.Length];
        for (int i = 0; i < choicesStringArray.Length; ++i)
        {
            string choiceString = ExtractCommands(choiceCommand, out var commands);
            TextNode textNode = new TextNode(choiceString, "", choiceNode);
            choiceNode.child.Add(textNode);

            for (int j = 0; j < commands.Count; ++j)
            {
                string commandString = commands[j];
                Command command = GetCommand(commandString);

                if (command.op == "GOTO")
                {
                    allGotos.Add(new Goto(textNode, command.data));
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
            string[] commandParts = command.Split(':');
            c.op = commandParts[0].Trim();
            c.data = commandParts[1].Trim();
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
        int startIndex = 0;
        int bracketCounter = 0;
        commands = new List<string>();
        for (int i = 0; i < line.Length; ++i)
        {
            if (line[i] == '[')
            {
                ++bracketCounter;
                startIndex = i;
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
                }
            }
        }
        line = line.Trim();
        while (line.Contains("  ")) 
            line = line.Replace("  ", " ");
        return line;
    }
}