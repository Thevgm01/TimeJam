using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatingTextClickable : FloatingText
{
    public Action<TextNode> nodeClicked = null;
    public Color clickedColor;
    Vector3 startClickCamPosition;
    Vector3 startClickMousePosition;

    protected override void Start()
    {
        base.Start();
        TextRevealer revealer = GetComponent<TextRevealer>();
        revealer.finishedRevealing += AddCollider;
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
        startClickCamPosition = Camera.main.transform.position;
        startClickMousePosition = Input.mousePosition;
    }

    void OnMouseUp()
    {
        if (Vector3.Distance(Camera.main.transform.position, startClickCamPosition) < 0.1f &&
            Vector3.Distance(Input.mousePosition, startClickMousePosition) == 0.0f)
        {
            nodeClicked?.Invoke(node);
        }
    }
}
