using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextNode node;
    public FloatingText ParentFT => node.parent != null ? node.parent.floatingText : null;
    public bool fixedToParent = true;
    public Rect dimensions;

    void Awake()
    {
        dimensions = GetComponent<RectTransform>().rect;
    }

    void Start()
    {
        TMPro.TextMeshPro tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.text = node.Text;

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
            transform.localPosition = ParentFT.transform.localPosition + Vector3.down * (parentHeight + StaticTextObjectManager.VerticalSpacing);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
