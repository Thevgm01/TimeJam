using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCommand
{
    public enum CommandType
    {
        Id,
        Goto,
        MinorChoice,
        MajorChoice
    }

    public CommandType type;
    public string[] parameters;
    public string parameter
    {
        get => parameters[0];
    }

    public TextCommand(CommandType type, string[] parameters)
    {
        this.type = type;
        this.parameters = parameters;
    }

    public TextCommand(CommandType type, string parameter)
    {
        this.type = type;
        this.parameters = new string[] { parameter };
    }
}
