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

    public override void SetChild(Node node)
    {
        if (!children.Contains(node))
        {
            children.Add(node);
            node.TrySetParent(this);
        }
    }
}
