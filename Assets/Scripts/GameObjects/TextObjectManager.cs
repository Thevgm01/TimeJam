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

    TextNode curNode;

    GameTextParser gameTextParser;
    int index = 1;

    void Awake()
    {
        textObjects = new Dictionary<string, FloatingText>();
        gameTextParser = new GameTextParser(gameScript);

        curNode = gameTextParser.FirstNode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curNode = CreateText(curNode);
            ++index;
        }
    }

    TextNode CreateText(TextNode node)
    {
        if (node is ChoiceNode) return InstantiateChoiceObject((ChoiceNode)node);
        else return InstantiateTextObject(node);
    }

    TextNode InstantiateChoiceObject(ChoiceNode node)
    {
        for (int i = 0; i < node.children.Count; ++i)
        {
            TextNode choice = node.children[i];
            InstantiateTextObject(choice);
        }
        return node.children[0];
    }

    TextNode InstantiateTextObject(TextNode node)
    {
        GameObject newTextObject = Instantiate(textPrefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        ft.fixedToParent = false;
        node.floatingText = ft;
        return node.child;
    }
}
