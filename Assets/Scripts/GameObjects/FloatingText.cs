using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Rect dimensions;

    void Start()
    {
        StartCoroutine("Initialize");
    }

    IEnumerator Initialize()
    {
        TMPro.TextMeshPro tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.text = node.Text;
        yield return null;
        dimensions = GetComponent<RectTransform>().rect; // Allow time for the content size fitter to change values

        if (node.parent != null)
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

            float parentHeight = ParentFT.dimensions.height;
            transform.localPosition = ParentFT.transform.localPosition + Vector3.down * (parentHeight + StaticTextObjectManager.Manager.verticalSpaceBetweenObjects);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }

        Vector3 cameraMovePoint = transform.position + Vector3.down * dimensions.height / 2f;
        FindObjectOfType<CameraPanner>().MoveToPoint(cameraMovePoint);
        FindObjectOfType<CameraScroller>().MoveToPoint(cameraMovePoint);
    }
}
