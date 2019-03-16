﻿using UnityEngine;
using System.Collections.Generic;

public class RuneNode : MonoBehaviour
{
    private void Awake()
    {
        Power = 0;
    }
    private void Update()
    {
        // Fail if max power exceeded
        if (Power > _maxPower)
        {
            RuneManager.StartButton.SetStateOff();
            return;
        }

        // Generate energy
        if (_emitEnergyAsSource)
        {
            Power += _powerGeneration * Time.deltaTime;
        }

        // Send Energy
        if (Power > _minWorkingPower)
        {
            foreach (RuneLink connection in Connections)
            {
                if (connection == null) continue;

                var powerDivision = _powerGain * Time.deltaTime;
                if (ActiveConnections != 0) powerDivision = powerDivision / ActiveConnections;
                Power -= connection.SendEnergy(powerDivision, sender: this);
            }
        }

        // Update colour
        var col = _inactiveColour;
        if (Power >= _highPower)
        {
            col = _highChargeColour;
        }
        else if (Power >= _minWorkingPower - 0.1f)
        {
            col = _activeColour;
        }
        if (_elementAObj != null) _elementAObj.GetComponent<SpriteRenderer>().color = col;

    }

    public int ActiveConnections
    {
        get
        {
            var activeConnections = 0;

            if (Connections == null && Connections.Count <= 0) return activeConnections;
            foreach (RuneLink connection in Connections)
            {
                if (connection == null) continue;

                if (connection.Active) activeConnections++;
            }
            return activeConnections;
        }
    }
    public Vector2 Position { get { return transform.position; } set { transform.position = value; } }

    public List<RuneLink> Connections;

    public float Power { get; private set; }
    public bool Active
    {
        get
        {
            return _elementAObj != null || _elementBObj != null;
        }
    }

    [SerializeField]
    private GameObject _elementAObj;
    [SerializeField]
    private GameObject _elementBObj;
    [SerializeField]
    private RuneSet _runeSet;

    private const float _powerGeneration = 0.3f;
    private const float _powerGain = 0.3f;
    private const float _maxPower = 1f;
    private const float _highPower = 0.75f;
    private const float _minWorkingPower = 0.25f;
    private const float _powerAmp = 3f;
    private bool _emitEnergyAsSource = false;

    private Color _inactiveColour = Color.black;
    private Color _activeColour = Color.yellow;
    private Color _highChargeColour = Color.red;

    public void SpawnElement(ElementType type, int index)
    {
        if (index < 0 || index > 1) throw new System.IndexOutOfRangeException();

        // Find correct prefab by element type
        var elementObject = _runeSet.Element.Basic;
        switch (type)
        {
            case ElementType.Injection:
                elementObject = _runeSet.Element.Injection;
                break;

            case ElementType.Amplifier:
                elementObject = _runeSet.Element.Amplifier;
                break;

            case ElementType.EnergyInput:
                elementObject = _runeSet.Element.EnergyInput;
                break;
        }
        
        // Remove any existing gameobject and replace with new one based on type.
        if (index == 0)
        {
            if (_elementAObj != null) Destroy(_elementAObj);
            _elementAObj = Instantiate(elementObject, transform.position, Quaternion.identity);
        }
        else
        {
            if (_elementBObj != null) Destroy(_elementBObj);
            _elementBObj = Instantiate(elementObject, transform.position, Quaternion.identity);
        }

        // Connect to any linked active nodes
        foreach (RuneLink connection in Connections)
        {
            if (connection == null) continue;

            var otherNode = connection.GetOther(this);
            if (otherNode == null) continue;

            if (otherNode.Active) connection.SetActive(true);
        }
    }
    public void ShowConnections(bool visibility)
    {
        if (Connections == null && Connections.Count <= 0) return;
        foreach (RuneLink connection in Connections)
        {
            if (connection == null) continue;
            connection.SetVisible(visibility);
        }
    }

    public void StartEnergyFlow()
    {
        if (_elementAObj == null) return;

        if (_elementAObj.GetComponent<RuneElement>() is EnergyInputElement)
        {
            // Start Energy Flow
            _emitEnergyAsSource = true;
        }
    }
    public void ResetEnergyFlow()
    {
        _emitEnergyAsSource = false;
        Power = 0;

        // Reset Colour
        if (_elementAObj != null) _elementAObj.GetComponent<SpriteRenderer>().color = _inactiveColour;

        // Reset links
        if (Connections == null && Connections.Count <= 0) return;
        foreach (RuneLink connection in Connections)
        {
            if (connection == null) continue;
            connection.ResetEnergyFlow();
        }
    }
    public void IncreaseEnergy(float amount)
    {
        Power += amount;
    }
}