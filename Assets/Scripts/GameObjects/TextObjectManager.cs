using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectManager : MonoBehaviour
{
    private static TextObjectManager _instance;
    public static TextObjectManager Instance { get => _instance; }


    public TextAsset gameScript;
    public GameObject textPrefab;
    public GameObject choicePrefab;
    public GameObject burnTextPrefab;
    public Transform burnHolderTransform;
    public float verticalSpaceBetweenObjects;

    Dictionary<string, FloatingText> textObjects;

    INode activeNode;
    IEnumerator choiceObjectsInstantiator;

    GameTextParser gameTextParser;

    void Awake()
    {
        _instance = this;

        textObjects = new Dictionary<string, FloatingText>();
        gameTextParser = new GameTextParser(gameScript);

        activeNode = gameTextParser.FirstNode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeNode is TextNode)
        {
            INode newNode = InstantiateTextObject((TextNode)activeNode);

            if (newNode is ChoiceNode)
            {
                FloatingText parentFT = ((TextNode)activeNode).floatingText;
                TextRevealer parentRevealer = parentFT.GetComponent<TextRevealer>();

                choiceObjectsInstantiator = InstantiateChoiceObject((ChoiceNode)newNode);
                parentRevealer.finishedRevealing += StartInstantiatingChoiceObjects;
            }

            activeNode = newNode;
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
            InstantiateTextObject(choice, listenForClick: true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    INode InstantiateTextObject(TextNode node, bool listenForClick = false)
    {
        GameObject newTextObject = Instantiate(textPrefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        ft.fixedToParent = false;
        if (listenForClick)
            ft.nodeClicked += NodeClicked;
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

    void NodeClicked(TextNode node)
    {
        Debug.Log(node.Text);
    }
}
