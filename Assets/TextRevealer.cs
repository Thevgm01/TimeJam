using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRevealer : MonoBehaviour
{
    TMPro.TextMeshPro tmp;

    public float lettersPerSecond = 50f;
    private string actualText;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        actualText = tmp.text;
        coroutine = RevealText();
        StartCoroutine(coroutine);
    }

    IEnumerator RevealText()
    {
        float timeTracker = 0;
        int curLetterIndex = 0;

        while (curLetterIndex < actualText.Length)
        {
            timeTracker += Time.deltaTime;
            curLetterIndex = (int)Mathf.Clamp(timeTracker * lettersPerSecond, 0, actualText.Length);
            tmp.text = actualText.Substring(0, curLetterIndex);
            Debug.Log(curLetterIndex);
            yield return null;
        }
    }
}
