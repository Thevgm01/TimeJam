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
    public GameObject clickablePrefab;
    public GameObject burnTextPrefab;
    public Transform burnHolderTransform;
    public float verticalSpaceBetweenObjects;

    Dictionary<string, FloatingText> textObjects;

    Node activeNode;
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
            InstantiateTextObject((TextNode)activeNode);
            Node newNode = ((TextNode)activeNode).child;

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
            InstantiateClickableObject(choice);
            yield return new WaitForSeconds(0.2f);
        }
    }

    FloatingText InstantiateTextObject(TextNode node)
    {
        GameObject newTextObject = Instantiate(textPrefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        node.floatingText = ft;
        return ft;
    }

    FloatingText InstantiateClickableObject(TextNode node)
    {
        GameObject newTextObject = Instantiate(clickablePrefab);
        string prefix = "(CHOICE) ";
        newTextObject.name = prefix + node.Text.Substring(0, Mathf.Min(20 - prefix.Length, node.Text.Length));

        FloatingTextClickable ft = newTextObject.GetComponent<FloatingTextClickable>();
        ft.node = node;
        node.floatingText = ft;

        ft.nodeClicked += NodeClicked;

        return ft;
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
