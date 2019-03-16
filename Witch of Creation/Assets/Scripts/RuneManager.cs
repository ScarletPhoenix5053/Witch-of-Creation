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

        // Create preview
        _previewElement = Instantiate(_runeSet.Element.Basic, transform);
        _previewElement.name = "Preview Element";
        _previewElement.GetComponent<SpriteRenderer>().color = Color.cyan;

        // Couple methods to start button
        if (_startButton != null)
        {
            _startButton.OnRitualStart += BeginRitual;
            _startButton.OnRitualEnd += EndRitual;
        }
        else
        {
            throw new MissingReferenceException("Start Button reference of the Rune Manager must be assigned in inspector.");
        }
    }

    public static RuneManager Instance { get; private set; }
    public static StartButton StartButton { get { return Instance._startButton; } }

    [SerializeField]
    private RuneSet _runeSet;
    [SerializeField]
    private GameObject _nodePrefab;
    [SerializeField]
    private Transform _nodes;
    [SerializeField]
    private float _maxPointerdistFromNode = 4f;
    [SerializeField]
    private StartButton _startButton;

    private GameObject _previewElement;
    private ElementType _prevElement = ElementType.Basic;
    private RuneNode _lineStartNode;
    private RuneNode _previousSnappedNode;

    public void BeginRitual()
    {
        foreach (Transform child in _nodes)
        {
            var node = child.GetComponent<RuneNode>();


            // Find and activate all energy input runes
            node.StartEnergyFlow();
        }
    }
    public void EndRitual()
    {
        foreach (Transform child in _nodes)
        {
            var node = child.GetComponent<RuneNode>();

            // Find and reset all runes
            node.ResetEnergyFlow();
        }
    }

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
        RuneNode snappedNode;

        // Find  nearest valid node
        snappedNode = FindNearestValidNode(position, out posIsValid);
        if (!posIsValid)
        {
            // Disable preview if there is no valid node.
            if (Instance._previewElement.activeInHierarchy != false) Instance._previewElement.SetActive(false);
            return posIsValid;
        }
        snappedPos = snappedNode.Position;


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
        // Update preview img if changed
        if (Instance._prevElement != elementType)
        {
            Instance._prevElement = elementType;
            Instance._previewElement.GetComponent<SpriteRenderer>().sprite = elementObject.GetComponent<SpriteRenderer>().sprite;
        }
        // Update preview node visibility if new node is snapped to
        if (Instance._previousSnappedNode != null)
        {
            if (Instance._previousSnappedNode != snappedNode)
            {
                Instance._previousSnappedNode.ShowConnections(false);
                snappedNode.ShowConnections(true);
                Instance._previousSnappedNode = snappedNode;
            }
        }
        else
        {
            // Give reference if null
            Instance._previousSnappedNode = snappedNode;
        }


        // Spawn on input, else update preview pos
        if (input)
        {
            snappedNode.SpawnElement(elementType, 0);
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

    public static bool StartLine(Vector2 position, bool input)
    {
        // Find nearest valid node
        var posIsValid = false;
        var nearestNode = FindNearestValidNode(position, out posIsValid);
        if (!posIsValid) return false;

        // On input, return true and store the node
        if (input)
        {
            Instance._lineStartNode = nearestNode;
            return true;
        }

        return false;
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
    /// <summary>
    /// Sets snappedPos to the position of the nearest node. Returns false if there are no nodes close enough.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="snappedNode"></param>
    /// <returns></returns>
    private static RuneNode FindNearestValidNode(Vector2 position, out bool nodeNearby)
    {
        RuneNode snappedNode = null;

        var closestNodeDist = Instance._maxPointerdistFromNode;
        for (int n = 0; n < Instance._nodes.childCount; n++)
        {
            var node = Instance._nodes.GetChild(n);
            var nodeDist = Vector3.Distance(position, node.position);
            if (nodeDist < closestNodeDist)
            {
                // Update return node
                snappedNode = node.GetComponent<RuneNode>();
                closestNodeDist = nodeDist;
            }
        }
        // If there wasn't a valid node
        if (closestNodeDist >= Instance._maxPointerdistFromNode)
        {
            // Notify root method that there was no valid node
            nodeNearby = false;
        }
        else nodeNearby = true;

        return snappedNode;
    }
}
