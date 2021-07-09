using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNode : BaseNode
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
            parent.child = this;
        }
    }

    public TextNode(string text, string id, INode parent) : this(new TextStory(text), id, parent) { }
}
