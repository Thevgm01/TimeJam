﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatingText : MonoBehaviour
{
    public TextNode node;
    public FloatingText ParentFT {
        get
        {
            if (node != null && node.parent != null && node.parent is TextNode)
                return ((TextNode)node.parent).floatingText;
            return null;
        }    
    }
    public bool fixedToParent = true;

    private TMPro.TextMeshPro tmp;
    public Vector2 Dimensions => new Vector2(tmp.preferredWidth, tmp.preferredHeight);

    public Action<TextNode> nodeClicked = null;

    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.text = node.Text;
        tmp.ForceMeshUpdate();

        if (ParentFT != null)
        {
            if (fixedToParent)
            {
                FloatingText firstUnfixedParent = ParentFT;
                while (firstUnfixedParent.fixedToParent && firstUnfixedParent.ParentFT != null)
                {
                    firstUnfixedParent = firstUnfixedParent.ParentFT;
                }
                transform.SetParent(firstUnfixedParent.transform, false);
            }

            float parentHeight = ParentFT.Dimensions.y;
            transform.localPosition = ParentFT.transform.localPosition + Vector3.down * (parentHeight + TextObjectManager.Instance.verticalSpaceBetweenObjects);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }

        Vector3 cameraMovePoint = transform.position + Vector3.down * Dimensions.y / 2f;
        CameraFocuser.Instance.Focus(cameraMovePoint);

        if (nodeClicked != null)
        {
            TextRevealer revealer = GetComponent<TextRevealer>();
            revealer.finishedRevealing += AddCollider;
        }
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
