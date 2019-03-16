using UnityEngine;
using System.Collections;

public class RuneLink : MonoBehaviour
{
    public RuneNode NodeA;
    public RuneNode NodeB;

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
}
