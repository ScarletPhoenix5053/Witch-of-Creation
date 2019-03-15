using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private void Awake()
    {
        OnMouseNormal += CheckElementOnHover;
        OnMouseClick += CheckElementOnInput;
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

    private void CheckElementOnHover()
    {
        RuneManager.PlaceElement(_mousePos, ElementType.Basic, false);
    }
    private void CheckElementOnInput()
    {
        RuneManager.PlaceElement(_mousePos, ElementType.Basic, true);
    }
    private void GetMousePos()
    {
        Camera cam = Camera.main;
        var mousePos = Input.mousePosition;

        _mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    }
}
