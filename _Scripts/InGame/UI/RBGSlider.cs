using System;
using UnityEngine;
using UnityEngine.UI;

public class RBGSlider : MonoBehaviour
{
    [SerializeField]
    private Slider _rgbSlider;
    
    private void Awake()
    {
        EventManager.onUpdateRGBSlider += OnUpdateRGBSlider;
        _rgbSlider.onValueChanged.AddListener(PostEventChangeRGBSlider);
    }

    private void OnDestroy()
    {
        EventManager.onUpdateRGBSlider -= OnUpdateRGBSlider;
        _rgbSlider.onValueChanged.RemoveListener(PostEventChangeRGBSlider);
    }

    private void OnUpdateRGBSlider(Color obj)
    {
        Color.RGBToHSV(obj,out var h,out var s,out var v);
        _rgbSlider.value = h;
    }

    private void PostEventChangeRGBSlider(float value)
    {
        var color = Color.HSVToRGB(value, 1, 1);
        EventManager.onChangeRGBSlider?.Invoke(color);
    }
}