using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectManager : MonoBehaviour
{
    public TextAsset gameText;
    public GameObject baseTextObject;
    public float verticalSpaceBetweenObjects;

    Dictionary<string, FloatingText> textObjects;

    FloatingText lastText;

    GameTextParser gameTextParser;
    int index = 1;

    // Start is called before the first frame update
    void Start()
    {
        textObjects = new Dictionary<string, FloatingText>();
        gameTextParser = new GameTextParser(gameText);
        lastText = CreateTextObject(null, false, "", gameTextParser.paragraphs[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastText = CreateTextObject(lastText, true, "", gameTextParser.paragraphs[index]);
            ++index;
        }
    }

    FloatingText CreateTextObject(FloatingText parent, bool fixedToParent, string id, string text)
    {
        GameObject newTextObject = Instantiate(baseTextObject);
        newTextObject.name = text.Substring(0, Mathf.Min(20, text.Length));

        FloatingText ft = newTextObject.AddComponent<FloatingText>();
        ft.parent = parent;
        ft.fixedToParent = fixedToParent;
        ft.id = id;
        ft.text = text;

        RectTransform ftRectTransform = ft.GetComponent<RectTransform>();

        if (parent != null)
        {
            if (fixedToParent)
            {
                FloatingText firstUnfixedParent = parent;
                while (firstUnfixedParent.fixedToParent && firstUnfixedParent.parent != null)
                {
                    firstUnfixedParent = firstUnfixedParent.parent;
                }
                ft.transform.SetParent(firstUnfixedParent.transform, false);
            }

            RectTransform parentRectTransform = parent.GetComponent<RectTransform>();
            float parentHeight = parentRectTransform.rect.height;
            ftRectTransform.localPosition = parentRectTransform.localPosition + Vector3.down * (parentHeight + verticalSpaceBetweenObjects);
        }
        else
        {
            ft.transform.SetParent(this.transform, false);
            ftRectTransform.localPosition = Vector3.zero;
        }

        if (id != "")
        {
            textObjects.Add(id, ft);
        }

        return ft;
    }
}
