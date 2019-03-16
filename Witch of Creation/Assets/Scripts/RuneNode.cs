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
    private RuneElement _elementA;
    [SerializeField]
    private RuneElement _elementB;
}