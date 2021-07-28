using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatingTextClickable : FloatingText
{
    public Action<TextNode> nodeClicked = null;
    public Color clickedColor;
    Vector3 startClickPosition;

    SpriteRenderer background = null;
    public Vector2 backgroundPadding = new Vector2();

    protected override void Start()
    {
        base.Start();
        TextRevealer revealer = GetComponent<TextRevealer>();
        revealer.finishedRevealing += AddCollider;

        background = GetComponentInChildren<SpriteRenderer>();
        background.size = (new Vector2(tmp.preferredWidth, tmp.preferredHeight) + backgroundPadding) / background.transform.localScale;
    }

    void AddCollider()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        var collider = gameObject.GetComponent<BoxCollider>();
        collider.center = tmp.bounds.center;
        collider.size = tmp.bounds.size;
    }

    void OnMouseDown()
    {
        startClickPosition = Input.mousePosition;
    }

    void OnMouseUp()
    {
        if (Input.mousePosition == startClickPosition)
        {
            nodeClicked?.Invoke(node);
            tmp.color = clickedColor;
        }
    }
}
