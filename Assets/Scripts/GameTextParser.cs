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

    public Dictionary<string, INode> namedNodes;
    public INode FirstNode => namedNodes["ORIGIN"];
    private string[] lines;

    public GameTextParser(TextAsset gameText)
    {
        namedNodes = new Dictionary<string, INode>();

        INode curNode = null;
        Dictionary<TextNode, string> gotos = new Dictionary<TextNode, string>();

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

            ChoiceNode choiceNode = null;
            ITextDisplayable text = null;
            string id = "";

            for (int j = 0; j < commandStrings.Count; ++j)
            {
                string commandString = commandStrings[j];
                Command command = GetCommand(commandString);

                if (command.op == "MINOR_CHOICE" || command.op == "MAJOR_CHOICE")
                {
                    choiceNode = CreateChoiceNode(command.data, gotos);
                }
                else if (command.op == "GOTO")
                {
                    gotos[(TextNode)curNode] = command.data;
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

            if (choiceNode != null)
                curNode = (INode)choiceNode;
            else if (text != null)
                curNode = (INode)new TextNode(text, id, curNode);
            else
                curNode = (INode)new TextNode(line, id, curNode);

            if (id != "")
                namedNodes[id] = curNode;
        }

        foreach (KeyValuePair<TextNode, string> kvp in gotos)
        {
            //Debug.Log("Linking \"" + g.parent.Text + "\" to " + g.destinationId);
            if (!namedNodes.ContainsKey(kvp.Value))
                Debug.LogError("Id " + kvp.Value + " is not defined in the game text.");
            kvp.Key.parent.SetChild(namedNodes[kvp.Value]);
            //namedNodes[kvp.Value].parent = kvp.Key;
        }
    }

    private ChoiceNode CreateChoiceNode(string choiceCommand,  Dictionary<TextNode, string> gotos)
    {
        string[] choicesStringArray = choiceCommand.Split(',');
        ChoiceNode choiceNode = new ChoiceNode();
        TextNode[] choices = new TextNode[choicesStringArray.Length];
        for (int i = 0; i < choicesStringArray.Length; ++i)
        {
            string choiceString = ExtractCommands(choicesStringArray[i], out var commands);
            TextNode textNode = new TextNode(choiceString, "", choiceNode);
            choiceNode.children.Add(textNode);

            for (int j = 0; j < commands.Count; ++j)
            {
                string commandString = commands[j];
                Command command = GetCommand(commandString);

                if (command.op == "GOTO")
                {
                    gotos[textNode] = command.data;
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