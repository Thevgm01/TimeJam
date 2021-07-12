using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : INode
{
    public INode parent;
    public List<INode> children;
    public Dictionary<INode, FloatingText> floatingText;

    public ChoiceNode()
    {
        children = new List<INode>();
        floatingText = new Dictionary<INode, FloatingText>();
    }

    public void SetChild(INode node)
    {
        if (!children.Contains(node))
            children.Add(node);
    }
}
