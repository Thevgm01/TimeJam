using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectManager : MonoBehaviour
{
    private static TextObjectManager _instance;
    public static TextObjectManager Instance { get => _instance; }

    public TextAsset gameScript;
    public GameObject textPrefab;
    public GameObject clickablePrefab;
    public GameObject burnTextPrefab;
    public Transform burnHolderTransform;
    public float verticalSpaceBetweenObjects;
    public float verticalSpaceBetweenChoices;
    public float delayBetweenChoiceInstantiation;

    Dictionary<string, FloatingText> textObjects;

    Node activeNode;
    FloatingText activeFT;

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
        if (Input.GetKeyDown(KeyCode.Space) && activeNode != null)
        {
            activeNode = CreateNode(activeNode);
        }
    }

    Node CreateNode(Node node)
    {
        if (node is TextNode)
        {
            activeFT = InstantiateTextObject((TextNode)node, textPrefab, verticalSpaceBetweenObjects);
            activeFT.initialized += CenterCamera;
            return ((TextNode)node).child;
        }
        else if (node is ChoiceNode)
        {
            IEnumerator instantiator = InstantiateChoiceObject((ChoiceNode)node);
            StartCoroutine(instantiator);
            return null;
        }
        return null;
    }

    IEnumerator InstantiateChoiceObject(ChoiceNode node)
    {
        float heightOffset = verticalSpaceBetweenObjects;

        for (int i = 0; i < node.children.Count; ++i)
        {
            TextNode choice = (TextNode)node.children[i];
            activeFT = InstantiateTextObject(choice, clickablePrefab, heightOffset);
            activeFT.name = "(CHOICE) " + activeFT.name;
            ((FloatingTextClickable)activeFT).nodeClicked += NodeClicked;

            if (i == 0)
            {
                activeFT.initialized += CenterCamera;
                heightOffset = verticalSpaceBetweenChoices;
            }
            yield return new WaitForSeconds(delayBetweenChoiceInstantiation);
        }
    }

    FloatingText InstantiateTextObject(TextNode node, GameObject prefab, float heightOffset)
    {
        GameObject newTextObject = Instantiate(prefab);
        newTextObject.name = node.Text.Substring(0, Mathf.Min(20, node.Text.Length));

        if (activeFT == null)
        {
            newTextObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            float height = activeFT.Dimensions.y;
            newTextObject.transform.localPosition = activeFT.transform.localPosition + Vector3.down * (height + heightOffset);
        }

        FloatingText ft = newTextObject.GetComponent<FloatingText>();
        ft.node = node;
        node.floatingText = ft;
        return ft;
    }

    public static void MatchCharacterPositions(TMPro.TextMeshPro source, int sourceIndex, TMPro.TextMeshPro item, int itemIndex)
    {
        var sourceCharInfo = source.textInfo.characterInfo[sourceIndex];
        var itemCharInfo = item.textInfo.characterInfo[itemIndex];
        Vector3 sourceCharPos = new Vector3(sourceCharInfo.origin, sourceCharInfo.baseLine);
        Vector3 itemCharPos = new Vector3(itemCharInfo.origin, itemCharInfo.baseLine);
        item.transform.position = source.transform.position + sourceCharPos - itemCharPos;
    }

    void NodeClicked(TextNode node)
    {
        Node child = node.child;

        if (child is TextNode && ((TextNode)child).floatingText != null)
        {
            FloatingText ft = ((TextNode)child).floatingText;
            if (ft != null)
            {
                CenterCamera(ft);
            }
        }
        else
        {
            activeNode = CreateNode(child);
        }
    }

    static void CenterCamera(FloatingText ft)
    {
        Vector3 cameraMovePoint = ft.transform.position + Vector3.down * ft.Dimensions.y / 2f;
        CameraManager.Instance.Focus(cameraMovePoint);
    }
}
