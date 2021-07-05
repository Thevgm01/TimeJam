using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : TextNode
{
    public new List<TextNode> child;
    public new Dictionary<TextNode, FloatingText> floatingText;

    public ChoiceNode()
    {
        child = new List<TextNode>();
        floatingText = new Dictionary<TextNode, FloatingText>();
    }
}
