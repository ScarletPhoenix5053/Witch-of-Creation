using UnityEngine;
using System.Collections.Generic;

public class RuneNode : MonoBehaviour
{
    private void Awake()
    {
        Power = 0;
    }

    public Vector2 Position { get { return transform.position; } set { transform.position = value; } }

    public List<RuneNode> Connected { get; set; }

    public float Power { get; private set; }
}