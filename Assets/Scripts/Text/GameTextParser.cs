using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameTextParser
{
    [SerializeField] private static TextAsset gameText;
    public static List<string> paragraphs;
    public static Dictionary<string, TextCommand> commands;

    // Start is called before the first frame update
    static GameTextParser()
    {
        paragraphs = new List<string>();
        commands = new Dictionary<string, TextCommand>();

        string[] lines = gameText.text.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
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

    private static TextCommand GetCommand(string line)
    {
        string[] sections = line.Split(',');

        if (line.StartsWith("GOTO")) return new TextCommand(TextCommand.CommandType.Goto, line.Substring(5));
        else if (line.StartsWith("MINOR CHOICE")) return new TextCommand(TextCommand.CommandType.MinorChoice, new ArraySegment<string>(sections, 1, sections.Length - 2).Array);
        else if (line.StartsWith("MAJOR CHOICE")) return new TextCommand(TextCommand.CommandType.MajorChoice, new ArraySegment<string>(sections, 1, sections.Length - 2).Array);
        else return new TextCommand(TextCommand.CommandType.Id, line.Substring(5));
    }

    public static bool IsCommand(string line)
    {
        return commands[line] != null;
    }
}