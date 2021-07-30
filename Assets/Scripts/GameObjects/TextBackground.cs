using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBackground : MonoBehaviour
{
    FloatingText ft;

    public bool expandWithRevealer = false;
    SpriteRenderer background = null;
    public Vector2 backgroundPadding = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        ft = GetComponent<FloatingText>();

        background = GetComponentInChildren<SpriteRenderer>();
        if (expandWithRevealer)
        {
            background.transform.localPosition = new Vector3(-1000, 0, -1000);
            TextRevealer revealer = GetComponent<TextRevealer>();
            revealer.characterRevealed += (int c) => { SetSize(ft.Dimensions); };
            revealer.finishedRevealing += () => { SetSize(ft.PreferredDimensions); Destroy(this); };
        }
        else
        {
            SetSize(ft.PreferredDimensions);
        }
    }

    void SetSize(Vector2 size)
    {
        background.size = (size + backgroundPadding) / background.transform.localScale;
        // For left-justified text
        Vector2 center = 
        background.transform.localPosition = 
            new Vector3((background.size.x / 2 - ft.PreferredDimensions.x - backgroundPadding.x) * background.transform.localScale.x,
                       -(background.size.y / 2 - backgroundPadding.y) * background.transform.localScale.y, 
                        1);
    }
}
