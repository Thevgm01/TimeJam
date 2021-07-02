using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameTextParser
{
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
        else if (line.StartsWith("MINOR CHOICE")) return new TextCommand(TextCommand.CommandType.MinorChoice, new ArraySegment<string>(sections, 1, sections.Length - 2).Array);
        else if (line.StartsWith("MAJOR CHOICE")) return new TextCommand(TextCommand.CommandType.MajorChoice, new ArraySegment<string>(sections, 1, sections.Length - 2).Array);
        else return new TextCommand(TextCommand.CommandType.Id, line);
    }

    public bool IsCommand(string line)
    {
        return commands[line] != null;
    }
}