using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatingTextClickable : FloatingText
{
    public Action<TextNode> nodeClicked = null;

    protected override void Start()
    {
        base.Start();
        TextRevealer revealer = GetComponent<TextRevealer>();
        revealer.finishedRevealing += AddCollider;
    }

    void AddCollider()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        var collider = gameObject.AddComponent<BoxCollider>();
        collider.center = tmp.bounds.center;
        collider.size = tmp.bounds.size;
    }

    void OnMouseDown()
    {
        nodeClicked?.Invoke(node);
    }
}