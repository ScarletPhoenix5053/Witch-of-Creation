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

        // On right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                // If already configuring node
                if (_configuringNodeRotation)
                {
                    // Rotate the node to face new clicked target
                    var clickedElement = hit.collider.GetComponent<RuneElement>();
                    _lookAtNode = clickedElement.transform.parent.GetComponent<RuneNode>();
                    _configuringNode.RotateTowards(_lookAtNode);
                    _configuringNodeRotation = false;
                    _lookAtNode = null;
                    _configuringNode = null;
                }
                else
                {
                    // Check if on an injection node
                    var injectionElement = hit.collider.GetComponent<InjectionElement>();
                    if (injectionElement != null )
                    {
                        _configuringNodeRotation = true;
                        _configuringNode = injectionElement.transform.parent.GetComponent<RuneNode>();
                        Debug.Log("Configuring injection node");
                    }
                }
            }            
        }

        // Call events
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
    private RuneNode _configuringNode;
    private RuneNode _lookAtNode;
    private bool _configuringNodeRotation = false;

    private void CheckNodeRotationOnHover()
    {

    }
    private void CheckNodeRotationOnInput()
    {

    }
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

            case ActiveTool.NodeRotation:
                OnMouseNormal -= CheckLineOnHover;
                OnMouseNormal -= CheckElementOnHover;
                OnMouseClick -= CheckLineOnInput;
                OnMouseClick -= CheckElementOnInput;

                
                break;
        }

        _activeTool = newTool;
    }
}
public enum ActiveTool
{
    Element,
    Line,
    NodeRotation
}