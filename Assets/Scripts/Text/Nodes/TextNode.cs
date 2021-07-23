using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNode : Node
{
    ITextDisplayable textItem;
    public string Text => textItem.GetText();
    public Node child;
    public FloatingText floatingText;
    public int timeline;

    public TextNode(ITextDisplayable textItem)
    {
        this.textItem = textItem;
    }

    public TextNode(string text) : this(new TextStory(text)) { }

    public override void TrySetChild(Node node)
    {
        if (child == null && node != null)
            child = node;
    }
}
