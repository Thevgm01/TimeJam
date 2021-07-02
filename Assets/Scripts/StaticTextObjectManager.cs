using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTextObjectManager
{
    private static TextObjectManager _textObjectManager;
    private static TextObjectManager _textObjectManager2
    {
        get
        {
            if (_textObjectManager == null)
                _textObjectManager = Object.FindObjectOfType<TextObjectManager>();
            return _textObjectManager;
        }
    }

    public static GameObject TextPrefab => _textObjectManager2.textPrefab;
    public static GameObject BurnPrefab => _textObjectManager2.burnTextPrefab;
    public static Transform BurnHolderTransform => _textObjectManager2.burnHolderTransform;

    public static float VerticalSpacing => _textObjectManager2.verticalSpaceBetweenObjects;
}
