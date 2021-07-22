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

    public TextNode(ITextDisplayable textItem, Node parent)
    {
        this.textItem = textItem;
        parent?.SetChild(this);
        TrySetParent(parent);
    }

    public TextNode(string text, Node parent) : this(new TextStory(text), parent) { }

    public override void SetChild(Node node)
    {
        child = node;
    }
}
