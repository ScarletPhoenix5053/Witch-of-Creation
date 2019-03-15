using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        //GenerateOuterCircleNodes();
        _previewElement = Instantiate(_runeSet.Element.Basic, transform);
        _previewElement.name = "Preview Element";
        _previewElement.GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    public static RuneManager Instance { get; private set; }

    [SerializeField]
    private RuneSet _runeSet;
    [SerializeField]
    private GameObject _nodePrefab;
    [SerializeField]
    private Transform _nodes;
    [SerializeField]
    private float _maxPointerdistFromNode = 4f;
    private GameObject _previewElement;
    private ElementType _prevElement = ElementType.Basic;

    /// <summary>
    /// Check or place an element, snapped to a new position, based on a vector2 location.
    /// Checks if input is false, places if input is true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="input"></param>
    /// <returns>False if position is too far from nearest snap target.</returns>
    public static bool PlaceElement(Vector2 position, ElementType elementType, bool input, out Vector2 snappedPos)
    {
        var posIsValid = false;
        snappedPos = Vector2.zero;

        // Find  nearest valid node
        var closestNodeDist = Instance._maxPointerdistFromNode;
        for (int n = 0; n < Instance._nodes.childCount; n++)
        {
            var node = Instance._nodes.GetChild(n);
            var nodeDist = Vector3.Distance(position, node.position);
            if (nodeDist < closestNodeDist)
            {
                snappedPos = node.position;
                closestNodeDist = nodeDist;
            }
        }
        if (closestNodeDist >= Instance._maxPointerdistFromNode)
        {
            if (Instance._previewElement.activeInHierarchy != false) Instance._previewElement.SetActive(false);
            posIsValid = false;
            return posIsValid;
        }

        // Check element
        var elementObject = Instance._runeSet.Element.Basic;
        switch (elementType)
        {
            case ElementType.Injection:
                elementObject = Instance._runeSet.Element.Injection;
                break;

            case ElementType.Amplifier:
                elementObject = Instance._runeSet.Element.Amplifier;
                break;

            case ElementType.EnergyInput:
                elementObject = Instance._runeSet.Element.EnergyInput;
                break;
        }
        // Update preview if changed
        if (Instance._prevElement != elementType)
        {
            Instance._prevElement = elementType;
            Instance._previewElement.GetComponent<SpriteRenderer>().sprite = elementObject.GetComponent<SpriteRenderer>().sprite;
        }

        if (input)
        {
            Instantiate(elementObject, snappedPos, Quaternion.identity);
            if (Instance._previewElement.activeInHierarchy != false) Instance._previewElement.SetActive(false);
        }
        else
        {
            if (Instance._previewElement.activeInHierarchy != true) Instance._previewElement.SetActive(true);
            Instance._previewElement.transform.position = snappedPos;
        }

        return posIsValid;
    }    
    /// <summary>
    /// Check or place an element, snapped to a new position, based on a vector2 location.
    /// Checks if input is false, places if     input is true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="input"></param>
    /// <returns>False if position is too far from nearest snap target.</returns>
    public static bool PlaceElement(Vector2 position, ElementType elementType, bool input)
    {
        return PlaceElement(position, elementType, input, out Vector2 snappedPos);
    }
    
    private static void GenerateOuterCircleNodes()
    {
        var fullCircle = 360f;
        var nodeCount = 12;
        var degreesOfSeperation = fullCircle / nodeCount;
        var radius = 8f;
        var spawnPos = Instance._nodes.position + new Vector3(0, radius, 0);

        for (float i = 0; i < fullCircle; i += degreesOfSeperation)
        {            
            var newnode = Instantiate(Instance._nodePrefab, spawnPos, Quaternion.identity, Instance._nodes);
            newnode.transform.RotateAround(Instance._nodes.position, Vector3.back, i);
        }
    }
    private static void AddNodesInside(Transform parent)
    {

    }
}
