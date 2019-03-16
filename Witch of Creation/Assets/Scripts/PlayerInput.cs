using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private void Awake()
    {
        // Initialize as element selector by default
        _activeTool = ActiveTool.Line;
        SetActiveTool("Element");
    }
    private void Update()
    {
        GetMousePos();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnMouseClick?.Invoke();
        }
        else
        {
            OnMouseNormal?.Invoke();
        }
    }

    public delegate void OnMouseNormalHandler();
    public event OnMouseNormalHandler OnMouseNormal;

    public delegate void OnMouseClickHandler();
    public event OnMouseClickHandler OnMouseClick;

    private Vector3 _mousePos;
    [SerializeField]
    private ElementSelector _elementSelector;
    [SerializeField]
    private ActiveTool _activeTool;

    private void CheckElementOnHover()
    {
        RuneManager.PlaceElement(_mousePos, _elementSelector.CurrentElement, false);
    }
    private void CheckElementOnInput()
    {
        RuneManager.PlaceElement(_mousePos, _elementSelector.CurrentElement, true);
    }
    private void CheckLineOnHover()
    {
        Debug.Log("Mehhh");
    }
    private void CheckLineOnInput()
    {
        Debug.Log("YEET");
    }
    private void GetMousePos()
    {
        Camera cam = Camera.main;
        var mousePos = Input.mousePosition;

        _mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    }
    public void SetActiveTool(string aString)
    {
        var newTool = (ActiveTool)Enum.Parse(typeof(ActiveTool), aString, true);
        if (newTool == _activeTool) return;

        switch (newTool)
        {
            case ActiveTool.Element:                
                OnMouseNormal += CheckElementOnHover;
                OnMouseClick += CheckElementOnInput;
                OnMouseNormal -= CheckLineOnHover;
                OnMouseClick -= CheckLineOnInput;
                break;

            case ActiveTool.Line:
                OnMouseNormal += CheckLineOnHover;
                OnMouseClick += CheckLineOnInput;
                OnMouseNormal -= CheckElementOnHover;
                OnMouseClick -= CheckElementOnInput;
                break;
        }

        _activeTool = newTool;
    }
}
public enum ActiveTool
{
    Element,
    Line
}