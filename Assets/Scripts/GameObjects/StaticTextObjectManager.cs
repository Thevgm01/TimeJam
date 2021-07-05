using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTextObjectManager
{
    private static TextObjectManager _textObjectManager;
    public static TextObjectManager Manager
    {
        get
        {
            if (_textObjectManager == null)
                _textObjectManager = Object.FindObjectOfType<TextObjectManager>();
            return _textObjectManager;
        }
    }
}
