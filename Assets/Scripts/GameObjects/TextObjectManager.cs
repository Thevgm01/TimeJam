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
    Node newNode;
    FloatingText activeFT;

    GameTextParser gameTextParser;

    void Awake()
    {
        _instance = this;

        textObjects = new Dictionary<string, FloatingText>();
        gameTextParser = new GameTextParser(gameScript);

        newNode = gameTextParser.FirstNode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && newNode != null)
        {
            activeNode = newNode;

            if (activeNode is TextNode)
            {
                activeFT = InstantiateTextObject((TextNode)activeNode);
                activeFT.initialized += CenterCamera;
                newNode = ((TextNode)activeNode).child;
            }
            else if (activeNode is ChoiceNode)
            {
                IEnumerator instantiator = InstantiateChoiceObject((ChoiceNode)newNode);
                StartCoroutine(instantiator);
                newNode = null;
            }
        }
    }

    IEnumerator InstantiateChoiceObject(ChoiceNode node)
    {
        TextNode choice = (TextNode)node.children[0];
        activeFT = InstantiateClickableObject(choice, verticalSpaceBetweenObjects);
        activeFT.initialized += CenterCamera;

        for (int i = 1; i < node.children.Count; ++i)
        {
            yield return new WaitForSeconds(0.2f);
            choice = (TextNode)node.children[i];
            activeFT = InstantiateClickableObject(choice, 0);
        }
    }

    FloatingText InstantiateTextObject(TextNode node)
    {
        GameObject newTextObject = Instantiate(textPrefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        if (activeFT == null)
        {
            newTextObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            float height = activeFT.Dimensions.y;
            newTextObject.transform.localPosition = activeFT.transform.localPosition + Vector3.down * (height + verticalSpaceBetweenObjects);
        }

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        node.floatingText = ft;
        return ft;
    }

    FloatingText InstantiateClickableObject(TextNode node, float spaceOffset)
    {
        GameObject newTextObject = Instantiate(clickablePrefab);
        string prefix = "(CHOICE) ";
        newTextObject.name = prefix + node.Text.Substring(0, Mathf.Min(20 - prefix.Length, node.Text.Length));

        if (activeFT == null)
        {
            newTextObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            float height = activeFT.Dimensions.y;
            newTextObject.transform.localPosition = activeFT.transform.localPosition + Vector3.down * (height + spaceOffset);
        }

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

    void CenterCamera(FloatingText ft)
    {
        Vector3 cameraMovePoint = ft.transform.position + Vector3.down * ft.Dimensions.y / 2f;
        CameraManager.Instance.Focus(cameraMovePoint);
    }
}
