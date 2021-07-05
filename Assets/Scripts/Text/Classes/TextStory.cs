using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStory : ITextDisplayable
{
    string text;

    public string GetText()
    {
        return text;
    }

    public TextStory(string text)
    {
        this.text = text;
    }
}
