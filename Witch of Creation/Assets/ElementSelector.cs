using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelector : MonoBehaviour
{
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public ElementType CurrentElement;

    [SerializeField]
    private RuneSet _runeSet;
    private Image _image;

    public void CycleElementType()
    {
        // Cycle element
        var elementCount = Enum.GetNames(typeof(ElementType)).Length;
        var currentElementIndex = (int)CurrentElement;
        if (currentElementIndex >= elementCount)
        {
            CurrentElement = (ElementType)1;
        }
        else
        {
            CurrentElement++;
        }

        // Update sprite
        if (_runeSet != null)
        {
            switch (CurrentElement)
            {
                case ElementType.Basic:
                    _image.sprite = _runeSet.Element.Basic.GetComponent<SpriteRenderer>().sprite;
                    break;
                case ElementType.Injection:
                    _image.sprite = _runeSet.Element.Injection.GetComponent<SpriteRenderer>().sprite;
                    break;
                case ElementType.Amplifier:
                    _image.sprite = _runeSet.Element.Amplifier.GetComponent<SpriteRenderer>().sprite;
                    Debug.Log("Amp");
                    break;
                case ElementType.EnergyInput:
                    _image.sprite = _runeSet.Element.EnergyInput.GetComponent<SpriteRenderer>().sprite;
                    break;
                default:
                    Debug.Log("Def");
                    _image.sprite = _runeSet.Element.Basic.GetComponent<SpriteRenderer>().sprite;
                    break;
            }
        }
        else
        {
            Debug.LogError("No rune set found, cannot update image");
        }
    }
}
