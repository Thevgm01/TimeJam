﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRevealer : MonoBehaviour
{
    TMPro.TextMeshPro tmp;

    public float lettersPerSecond = 50f;

    float timeTracker = 0;
    int curLetterIndex = 0;

    private IEnumerator coroutine;

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
        }
        else
        {
            Destroy(this);
        }
    }
}
