using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public int Line { get; protected set; }
    public Node parent;

    public abstract void TrySetChild(Node node);

    public virtual void TrySetParent(Node node)
    {
        if (parent == null && node != null)
            parent = node;
    }

    public virtual void SetLine(int line)
    {
        this.Line = line;
    }
}
