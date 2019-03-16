using UnityEngine;
using System.Collections.Generic;

public class RuneLink : MonoBehaviour
{
    private void Awake()
    {
        // Get children
        var childList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            childList.Add(child.gameObject);
        }
        _children = childList.ToArray();

        // Initialize inactive
        Active = false;
    }
    private void Update()
    {
        // Update colour
        if (Active)
        {
            var col = _inactiveColour;
            if (Flow != 0)
            {
                col = _activeColour;
            }
            SetColour(col);
        }
    }

    public RuneNode NodeA;
    public RuneNode NodeB;
    public bool Active { get; private set; }
    public int Flow { get; private set; }

    private GameObject[] _children;

    private Color _inactiveColour = Color.black;
    private Color _activeColour = Color.yellow;
    private Color _highChargeColour = Color.red;

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
    /// <summary>
    /// Passes energy through the link to a connected node. Returns the amount of energy sent if successful,
    /// otherwise returns 0.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public float SendEnergy(float amount, RuneNode sender)
    {

        var reciever = GetOther(sender);
        var sentEnergy = 0f;

        // Exit early if reciever inactive
        if (!reciever.Active)
        {
            return sentEnergy;
        }

        Active = true;

        // Try to transmit energy
        if (sender == NodeA)
        {
            if (Flow >= 0)
            {
                reciever.IncreaseEnergy(amount);
                Flow = 1;
                sentEnergy = amount;
            }
        }
        else
        {
            if (Flow <= 0)
            {
                reciever.IncreaseEnergy(amount);
                Flow = -1;
                sentEnergy = amount;
            }
        }

        return sentEnergy;
    }
    public void ResetEnergyFlow()
    {
        Flow = 0;
        if (Active)
        {
            SetColour(_inactiveColour);
        }
        Active = false;
    }
}
