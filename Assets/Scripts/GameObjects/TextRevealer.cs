using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextRevealer : MonoBehaviour
{
    TMPro.TextMeshPro tmp;

    public float lettersPerSecond = 50f;

    float timeTracker = 0;
    int curLetterIndex = 0;
    public Action<int> characterRevealed = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.maxVisibleCharacters = 0;
    }

    void Update()
    {
        if (curLetterIndex < tmp.text.Length)
        {
            timeTracker += Time.deltaTime;
            curLetterIndex = (int)Mathf.Clamp(timeTracker * lettersPerSecond, 0, tmp.text.Length);
            tmp.maxVisibleCharacters = curLetterIndex;
            characterRevealed?.Invoke(curLetterIndex);
        }
        else
        {
            Destroy(this);
        }
    }
}
