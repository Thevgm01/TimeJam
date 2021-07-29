using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatingText : MonoBehaviour
{
    public Action<FloatingText> initialized = delegate { };

    public TextNode node;
    public FloatingText ParentFT {
        get
        {
            if (node != null && node.parent != null && node.parent is TextNode)
                return ((TextNode)node.parent).floatingText;
            return null;
        }    
    }
    public bool fixedToParent = true;

    protected TMPro.TextMeshPro tmp;
    public Vector2 Dimensions => new Vector2(tmp.renderedWidth, tmp.renderedHeight);
    public Vector2 PreferredDimensions => new Vector2(tmp.preferredWidth, tmp.preferredHeight);

    protected virtual void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.text = node.Text;
        tmp.ForceMeshUpdate();

        if (ParentFT != null)
        {
            if (fixedToParent)
            {
                FloatingText firstUnfixedParent = ParentFT;
                while (firstUnfixedParent.fixedToParent && firstUnfixedParent.ParentFT != null)
                {
                    firstUnfixedParent = firstUnfixedParent.ParentFT;
                }
                transform.SetParent(firstUnfixedParent.transform, false);
            }
        }

        initialized?.Invoke(this);
    }
}
