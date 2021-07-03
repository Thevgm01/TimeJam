using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goto
{
    public TextNode parent;
    public string id;

    public Goto(TextNode parent, string id)
    {
        this.parent = parent;
        this.id = id;
    }
}
