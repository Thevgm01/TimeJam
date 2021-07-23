using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : Node
{
    public List<Node> children;
    public Dictionary<Node, FloatingText> floatingText;

    public ChoiceNode()
    {
        children = new List<Node>();
        floatingText = new Dictionary<Node, FloatingText>();
    }

    public override void TrySetChild(Node node)
    {
        if (!children.Contains(node) && node != null)
            children.Add(node);
    }

    public override void SetLine(int line)
    {
        this.Line = line;
        foreach (Node child in children)
            child.SetLine(line);
    }
}
