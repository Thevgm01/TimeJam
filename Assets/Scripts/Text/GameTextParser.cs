using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameTextParser
{
    /* Valid commands
     * 
     * [MINOR_CHOICE: , , ,]
     * Choices that the player can make, comma separated
     * Each choice can end with a GOTO to designate where in the script the player should go
     *                 start with [SECRET: <item>] for it to only appear when a certain item is in the inventory
     * 
     * [FATAL_CHOICE: , , ,]
     * As MINOR_CHOICE, but items can be sent through time from one place to another
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

    public List<string> paragraphs;
    public Dictionary<string, TextCommand> commands;

    // Start is called before the first frame update
    public GameTextParser(TextAsset gameText)
    {
        paragraphs = new List<string>();
        commands = new Dictionary<string, TextCommand>();

        string[] lines = gameText.text.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
            //Debug.Log(line);
            if (line == "")
            {
                continue;
            }
            if (line[0] == '[' && line[line.Length - 1] == ']')
            {
                commands[line] = GetCommand(line.Substring(1, line.Length - 3));
            }
            paragraphs.Add(line);
        }
    }

    private TextCommand GetCommand(string line)
    {
        string[] sections = line.Split(',');

        if (line.StartsWith("GOTO")) return new TextCommand(TextCommand.CommandType.Goto, line.Substring(5));
        else if (line.StartsWith("MINOR_CHOICE")) return new TextCommand(TextCommand.CommandType.MinorChoice, new ArraySegment<string>(sections, 1, sections.Length - 2).Array);
        else if (line.StartsWith("MAJOR_CHOICE")) return new TextCommand(TextCommand.CommandType.MajorChoice, new ArraySegment<string>(sections, 1, sections.Length - 2).Array);
        else return new TextCommand(TextCommand.CommandType.Id, line);
    }

    public bool IsCommand(string line)
    {
        return commands[line] != null;
    }
}