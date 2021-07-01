using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextRevealer : MonoBehaviour
{
    TMPro.TextMeshPro tmp;

    public float wordsPerSecond = 10f;
    private string[] words;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        words = tmp.text.Trim().Split(' ');
        coroutine = RevealText();
        StartCoroutine(coroutine);
    }

    IEnumerator RevealText()
    {
        float timeTracker = 0;
        int curWordIndex = 0;

        while (curWordIndex < words.Length)
        {
            timeTracker += Time.deltaTime;
            curWordIndex = (int)Mathf.Clamp(timeTracker * wordsPerSecond, 0, words.Length);
            tmp.text = string.Join(" ", new ArraySegment<string>(words, 0, curWordIndex));
            yield return null;
        }
    }
}
