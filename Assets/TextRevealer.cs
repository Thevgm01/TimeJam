using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRevealer : MonoBehaviour
{
    TMPro.TextMeshPro tmp;

    public float lettersPerSecond = 50f;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        coroutine = RevealText();
        StartCoroutine(coroutine);
    }

    IEnumerator RevealText()
    {
        float timeTracker = 0;
        int curLetterIndex = 0;

        while (curLetterIndex < tmp.text.Length)
        {
            timeTracker += Time.deltaTime;
            curLetterIndex = (int)Mathf.Clamp(timeTracker * lettersPerSecond, 0, tmp.text.Length);
            tmp.maxVisibleCharacters = curLetterIndex;
            yield return null;
        }
    }
}
