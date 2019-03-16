using UnityEngine;
using System.Collections.Generic;

public class RuneLink : MonoBehaviour
{
    private void Awake()
    {
        var childList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            childList.Add(child.gameObject);
        }
        _children = childList.ToArray();
    }

    public RuneNode NodeA;
    public RuneNode NodeB;
    public bool Active { get; private set; }

    private GameObject[] _children;

    public RuneNode GetOther(RuneNode thisNode)
    {
        if (thisNode == NodeA)
        {
            return NodeB;
        }
        else if (thisNode == NodeB)
        {
            return NodeA;
        }
        else
        {
            Debug.LogError("Node " + thisNode + " is not bound by link " + name);
            return null;
        }
    }
    public void SetVisible(bool visibility)
    {
        // Ignore visibility commands if already active
        if (Active) return;

        foreach (GameObject child in _children)
        {
            child.SetActive(visibility);
        }
    }
    public void SetActive(bool isActive)
    {
        SetColour(Color.black);
        Active = true;
    }
    public void SetColour(Color colour)
    {
        foreach (GameObject child in _children)
        {
            child.GetComponent<SpriteRenderer>().color = colour;
        }
    }
}
