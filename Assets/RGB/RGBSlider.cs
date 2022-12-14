using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class RGBSlider : MonoBehaviour
{
    [SerializeField] private Slider rgb;
    [SerializeField] Light lampe ; 
    [SerializeField] private Material mColor;
    void Start()
    {
        rgb = gameObject.GetComponent<Slider>();
    }
    
    public void Slider()
    {
        var hue = rgb.value;
        lampe.color =  Color.HSVToRGB(hue, 1f, 1f);
        mColor.color = lampe.color;
        mColor.SetColor("_EmissionColor", lampe.color);


    }
}
