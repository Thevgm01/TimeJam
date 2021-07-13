using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextRevealer : MonoBehaviour
{
    TMPro.TextMeshPro tmp;
    public GameObject revealerCharacter;

    public float lettersPerSecond = 50f;

    float timeTracker = 0;
    int curLetterIndex = 0;
    public Action<int> characterRevealed = delegate { };
    public Action finishedRevealing;

    TMPro.TextMeshPro revealerTmp;
    TextObjectManager tom;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshPro>();
        tmp.maxVisibleCharacters = 0;

        revealerCharacter = Instantiate(revealerCharacter);
        revealerTmp = revealerCharacter.GetComponent<TMPro.TextMeshPro>();
    }

    void Update()
    {
        if (curLetterIndex < tmp.text.Length)
        {
            timeTracker += Time.deltaTime;
            curLetterIndex = (int)Mathf.Clamp(timeTracker * lettersPerSecond, 0, tmp.text.Length);
            tmp.maxVisibleCharacters = curLetterIndex;

            if (curLetterIndex < tmp.text.Length)
                StaticTextObjectManager.Manager.MatchCharacterPositions(tmp, curLetterIndex, revealerTmp, 0);
            else
                Destroy(revealerCharacter);

            characterRevealed?.Invoke(curLetterIndex);
        }
        else
        {
            finishedRevealing?.Invoke();
            Destroy(this);
        }
    }
}
