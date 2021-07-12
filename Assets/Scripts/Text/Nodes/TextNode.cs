using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNode : INode
{
    ITextDisplayable textItem;
    public string Text => textItem.GetText();
    public INode parent;
    public INode child;
    public string id;
    public FloatingText floatingText;

    public TextNode(ITextDisplayable textItem, string id, INode parent)
    {
        this.textItem = textItem;
        this.id = id;
        if (parent != null)
        {
            this.parent = parent;
            parent.SetChild(this);
        }
    }

    public TextNode(string text, string id, INode parent) : this(new TextStory(text), id, parent) { }

    public void SetChild(INode node)
    {
        child = node;
    }
}
