using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Range(0, 5)]
    public int volume = 5;


    public Slider slider;

    
    public int GetVolume()
    {
        return volume;
    }

    public void SetValue()
    {
        volume = (int)slider.value;
    }
}
