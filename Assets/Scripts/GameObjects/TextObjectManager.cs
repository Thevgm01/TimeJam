using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectManager : MonoBehaviour
{
    public TextAsset gameScript;
    public GameObject textPrefab;
    public GameObject choicePrefab;
    public GameObject burnTextPrefab;
    public Transform burnHolderTransform;
    public float verticalSpaceBetweenObjects;

    Dictionary<string, FloatingText> textObjects;

    INode curNode;
    IEnumerator choiceObjectsInstantiator;

    GameTextParser gameTextParser;

    void Awake()
    {
        textObjects = new Dictionary<string, FloatingText>();
        gameTextParser = new GameTextParser(gameScript);

        curNode = gameTextParser.FirstNode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curNode is TextNode)
        {
            INode newNode = InstantiateTextObject((TextNode)curNode);

            if (newNode is ChoiceNode)
            {
                FloatingText parentFT = ((TextNode)curNode).floatingText;
                TextRevealer parentRevealer = parentFT.GetComponent<TextRevealer>();

                choiceObjectsInstantiator = InstantiateChoiceObject((ChoiceNode)newNode);
                parentRevealer.finishedRevealing += StartInstantiatingChoiceObjects;
            }

            curNode = newNode;
        }
    }

    void StartInstantiatingChoiceObjects()
    {
        StartCoroutine(choiceObjectsInstantiator);
    }

    IEnumerator InstantiateChoiceObject(ChoiceNode node)
    {
        for (int i = 0; i < node.children.Count; ++i)
        {
            TextNode choice = (TextNode)node.children[i];
            InstantiateTextObject(choice);
            yield return new WaitForSeconds(0.2f);
        }
    }

    INode InstantiateTextObject(TextNode node)
    {
        GameObject newTextObject = Instantiate(textPrefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        ft.fixedToParent = false;
        node.floatingText = ft;
        return node.child;
    }

    public void MatchCharacterPositions(TMPro.TextMeshPro source, int sourceIndex, TMPro.TextMeshPro item, int itemIndex)
    {
        var sourceCharInfo = source.textInfo.characterInfo[sourceIndex];
        var itemCharInfo = item.textInfo.characterInfo[itemIndex];
        Vector3 sourceCharPos = new Vector3(sourceCharInfo.origin, sourceCharInfo.baseLine);
        Vector3 itemCharPos = new Vector3(itemCharInfo.origin, itemCharInfo.baseLine);
        item.transform.position = source.transform.position + sourceCharPos - itemCharPos;
    }
}
