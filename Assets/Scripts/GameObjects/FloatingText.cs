using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public FloatingText parent;
    public bool fixedToParent = true;

    public string id;
    public string text;

    public FloatingText(FloatingText parent, bool fixedToParent, string id, string text)
    {
        this.parent = parent;
        this.fixedToParent = fixedToParent;
        this.id = id;
        this.text = text;
    }

    void Start()
    {
        TMPro.TextMeshPro tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.text = text;
    }
}
