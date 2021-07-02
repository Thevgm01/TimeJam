﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public Dictionary<string, TextNode> nodes;
    public TextNode FirstNode => nodes["ORIGIN"];

    // Start is called before the first frame update
    public GameTextParser(TextAsset gameText)
    {
        nodes = new Dictionary<string, TextNode>();

        string[] lines = gameText.text.Split('\n');
        TextNode curNode = new TextNode(lines[0], "ORIGIN");
        nodes[curNode.id] = curNode;

        for (int i = 1; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();

            if (line == "")
            {
                continue;
            }
            curNode = new TextNode(line, curNode);
        }
    }
    /*
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
    }*/
}