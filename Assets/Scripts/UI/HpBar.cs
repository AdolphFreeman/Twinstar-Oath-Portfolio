using System;
using CraneFSM.Core;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Slider slider;
    public FloatParameter hp;
    
    private void Awake()
    {
        if(!slider)
            slider = GetComponent<Slider>();
        
        slider.value = hp.value;
        slider.minValue = 0;
        slider.maxValue = hp.value;
    }

    private void Update()
    {
        slider.value = hp.value;
    }
}
