using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName ="Rune Set", menuName ="Runes/RuneSet")]
public class RuneSet : ScriptableObject
{
    [Serializable]
    public class Elements
    {
        public GameObject Basic;
        public GameObject Injection;
        public GameObject Amplifier;
        public GameObject EnergyInput;
    }
    public Elements Element;
}
public enum ElementType
{
    Basic = 1,
    Injection,
    Amplifier,
    EnergyInput
}