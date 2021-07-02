using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBurner : MonoBehaviour
{
    TMPro.TextMeshPro tmp;
    TextRevealer revealer;
    string burnText;
    int burnTextIndex;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        revealer = GetComponent<TextRevealer>();
        revealer.characterRevealed += CharacterRevealed;
    }

    public void SetBurnText(string text, int index)
    {
        burnText = text;
        burnTextIndex = index;
    }

    // Update is called once per frame
    void CharacterRevealed(int i)
    {
        if (i < burnTextIndex)
        {
            Vector3 topLeft = tmp.textInfo.characterInfo[burnTextIndex].topLeft;

            GameObject burnObject = Instantiate(StaticTextObjectManager.BurnPrefab);
            burnObject.GetComponent<TMPro.TextMeshPro>().text = burnText;
            float width = burnObject.GetComponent<RectTransform>().rect.width;
            burnObject.transform.position = topLeft + Vector3.right * width / 2f;
            burnObject.transform.parent = StaticTextObjectManager.BurnHolderTransform;

            revealer.characterRevealed -= CharacterRevealed;
            Destroy(this);
        }
    }
}
