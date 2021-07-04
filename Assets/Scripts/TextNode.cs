using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNode
{
    ITextDisplayable textItem;
    public string Text => textItem.GetText();
    public TextNode parent;
    public TextNode child;
    public string id;
    public FloatingText floatingText;

    public TextNode(ITextDisplayable textItem, string id, TextNode parent)
    {
        this.textItem = textItem;
        this.id = id;
        if (parent != null)
        {
            this.parent = parent;
            parent.child = this;
        }
    }

    public TextNode(string text, string id, TextNode parent) : this(new TextStory(text), id, parent) { }
}
