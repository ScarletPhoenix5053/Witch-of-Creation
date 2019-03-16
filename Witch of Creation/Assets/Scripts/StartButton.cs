using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private Color _inactiveColour = Color.black;
    [SerializeField]
    private Color _activeColour = Color.cyan;
    [SerializeField]
    private Image _image;

    public bool Active { get; private set; }

    public delegate void RitualStartHandler();
    public event RitualStartHandler OnRitualStart;
    public delegate void RitualEndHandler();
    public event RitualEndHandler OnRitualEnd;

    public void ToggleState()
    {
        if (Active)
        {
            SetStateOff();
        }
        else
        {
            Active = true;
            if (_image != null) _image.color = _activeColour;
            OnRitualStart?.Invoke();
        }
    }
    public void SetStateOff()
    {
        Active = false;
        if (_image != null) _image.color = _inactiveColour;
        OnRitualEnd?.Invoke();
    }
}
