using UnityEngine;
using System.Collections;

public class RuneNode : MonoBehaviour
{
    public Vector2 Position { get { return transform.position; } set { transform.position = value; } }
}