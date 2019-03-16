using UnityEngine;
using System.Collections.Generic;

public class RuneNode : MonoBehaviour
{
    private void Awake()
    {
        Power = 0;
    }

    public Vector2 Position { get { return transform.position; } set { transform.position = value; } }

    public List<RuneLink> Connections { get; set; }

    public float Power { get; private set; }
    public bool Active { get; private set; }

    [SerializeField]
    private GameObject _elementAObj;
    [SerializeField]
    private GameObject _elementBObj;
    [SerializeField]
    private RuneSet _runeSet;

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
    }
}