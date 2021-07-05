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

            List<string> commands;
            try
            {
                line = ExtractCommands(line, out commands);
            }
            catch
            {
                Debug.LogError("Improper command brackets on line " + i + " of game text.");
                return;
            }

            ITextDisplayable text = null;
            string id = "";

            for (int commandStart = 0; commandStart < line.Length; ++commandStart)
            {
                if (line[commandStart] == '[')
                {
                    int commandLength = 1;
                    while (line[commandStart + commandLength] != ']') ++commandLength;

                    string command = line.Substring(commandStart + 1, commandLength - 1);

                    string[] parts = command.Split(':');
                    parts[0] = parts[0].Trim();
                    if (parts.Length > 1)
                        parts[1] = parts[1].Trim();

                    if (parts[0] == "MINOR_CHOICE" || parts[0] == "MAJOR_CHOICE")
                    {
                        string[] options = parts[1].Split(',');
                        for (int j = 0; j < options.Length; ++j)
                            options[j] = options[j].Trim();
                    }
                    else if (parts[0] == "GOTO")
                    {
                        allGotos.Add(new Goto(curNode, parts[1]));
                    }
                    else if (parts[0] == "GET" || parts[0] == "LOSE")
                    {
                        TextInventoryModifier.Operation op = 
                            parts[0] == "GET" ? 
                            TextInventoryModifier.Operation.Add : 
                            TextInventoryModifier.Operation.Remove;
                        text = new TextInventoryModifier(null, parts[1], op);
                    }
                    else
                    {
                        id = parts[0];
                        line = line.Substring(0, commandStart) + line.Substring(commandStart + command.Length + 2);
                        line = line.Trim();
                        line = line.Replace("  ", " ");
                    }
                }
            }

            if (text != null)
                curNode = new TextNode(text, id, curNode);
            else
                curNode = new TextNode(line, id, curNode);
            if (id != "")
                namedNodes[id] = curNode;
        }
        /*
        foreach (Goto g in allGotos)
        {
            if (!namedNodes.ContainsKey(g.id))
                Debug.LogError("Tag " + g.id + " is not defined in the game text.");
            g.parent.child = namedNodes[g.id];
            namedNodes[g.id].parent = g.parent;
        }*/
    }

    private string ExtractCommands(string line, out List<string> commands)
    {
        commands = new List<string>();
        Stack<int> leftBrackets = new Stack<int>();
        for (int i = 0; i < line.Length; ++i)
        {
            if (line[i] == '[')
            {
                leftBrackets.Push(i);
            }
            else if (line[i] == ']')
            {
                int leftIndex = leftBrackets.Pop();
                int rightIndex = i;
                int length = rightIndex - leftIndex;
                string command = line.Substring(leftIndex + 1, length - 1);
                commands.Add(command);
            }
        }
        commands.Reverse();
        return line;
    }
}