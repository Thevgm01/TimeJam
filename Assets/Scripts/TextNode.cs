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

    public TextNode(ITextDisplayable text) :                   this(text, "", null) { }
    public TextNode(ITextDisplayable text, string id) :        this(text, id, null) { }
    public TextNode(ITextDisplayable text, TextNode parent) :  this(text, "", parent) { }

    public TextNode(string text) :                             this(new TextStory(text), "", null) { }
    public TextNode(string text, string id) :                  this(new TextStory(text), id, null) { }
    public TextNode(string text, TextNode parent) :            this(new TextStory(text), "", parent) { }
    public TextNode(string text, string id, TextNode parent) : this(new TextStory(text), id, parent) { }
}
