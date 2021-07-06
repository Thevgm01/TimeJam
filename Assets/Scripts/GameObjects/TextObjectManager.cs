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
            CreateTextObject(curNode, true);
            curNode = curNode.child;
            ++index;
        }
    }

    void CreateTextObject(TextNode node, bool fixedToParent)
    {
        GameObject newTextObject = Instantiate(textPrefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        ft.fixedToParent = fixedToParent;

        node.floatingText = ft;
    }
}
