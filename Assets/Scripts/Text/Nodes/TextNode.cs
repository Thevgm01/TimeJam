using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNode : INode
{
    ITextDisplayable textItem;
    public string Text => textItem.GetText();
    public INode parent;
    public INode child;
    public FloatingText floatingText;
    public int timeline;

    public TextNode(ITextDisplayable textItem, INode parent)
    {
        this.textItem = textItem;
        if (parent != null)
        {
            parent.SetChild(this);
            TrySetParent(parent);
        }
    }

    public TextNode(string text, INode parent) : this(new TextStory(text), parent) { }

    public void SetChild(INode node)
    {
        child = node;
    }

    public void TrySetParent(INode node)
    {
        if (parent == null && node != null)
            parent = node;
    }
}
