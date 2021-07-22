using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public Node parent;

    public abstract void SetChild(Node node);

    public virtual void TrySetParent(Node node)
    {
        if (parent == null && node != null)
            parent = node;
    }
}
