using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDead : ITextDisplayable, IGameStateLink
{
    public string GetText()
    {
        return "You have perished. Perhaps you could have done something differently?";
    }
}
