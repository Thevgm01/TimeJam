using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWin : ITextDisplayable, IGameStateLink
{
    public string GetText()
    {
        return "You win! Yay!";
    }
}
