using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightController : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector3> setColorValue;
    [SerializeField] private UnityEvent<Vector3> setSatuationValue;
    [SerializeField] private UnityEvent<Vector3> setAngleValue;

    private Vector3 _color;
    private Vector3 _satuation;
    private Vector3 _angle;

    [SerializeField] private float angleFactor;
    [SerializeField] private float intensityFactor;
    
    private Light _light;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public Light Light
    {
        get => _light;
        set
        {
            _light = value;
            if(_light == null) return;
            _angle = new Vector3(0, 0, _light.spotAngle / angleFactor);
            setAngleValue.Invoke(_angle);
            
            float H, S, V;
            Color.RGBToHSV(_light.color, out H, out S, out V);
            _color = new Vector3(H, 0, S);
            setColorValue.Invoke(_color);
            
            _satuation = new Vector3(0, 0, _light.intensity / intensityFactor);
            setSatuationValue.Invoke(_satuation);
        }
    }
    
    

    public void ChangeColor(Vector3 values)
    {
        if (Light == null) return;
        _color = values;
        Light.color = Color.HSVToRGB(_color.x, _color.z, 1);
        var mColor = Light.transform.parent.GetChild(1).GetComponent<Renderer>()?.material;
        if (mColor == null) return;
        var color = Light.color;
        mColor.color = color;
        mColor.SetColor(EmissionColor, color);
    }
    
    public void ChangeSatuation(Vector3 values)
    {
        if (Light == null) return;
        _satuation = values;
        Light.intensity = _satuation.z * intensityFactor;
    }
    
    public void ChangeAngle(Vector3 values)
    {
        if (Light == null) return;
        _angle = values;
        Light.spotAngle = _angle.z * angleFactor;
    }
}
