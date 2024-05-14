using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

public class MachineSettingsManager : MonoBehaviour
{
    public TextMeshPro kvpValueOnScreen;
    public TextMeshProUGUI kvpIncreaseAmount;

    public TextMeshPro masValueOnScreen;
    public TextMeshProUGUI masIncreaseAmount;

    // kvp settings
    public ButtonManager KVPAddBtn;
    public ButtonManager KVPSubtractBtn;
    public ButtonManager KVPIncreaseModBtn;
    public ButtonManager KVPDecreaseModBtn;

    // mas settings
    public ButtonManager MASAddBtn;
    public ButtonManager MASSubtractBtn;
    public ButtonManager MASIncreaseModBtn;
    public ButtonManager MASDecreaseModBtn;


    public void UpdateKVPValue(string updateType)
    {
        var currentValue = int.Parse(kvpValueOnScreen.text);
        var currentMultiplier = int.Parse(kvpIncreaseAmount.text);

        if (updateType == "add")
        {
            var newValue = currentValue += currentMultiplier;
            if (newValue > 130)
                newValue = 130;
            kvpValueOnScreen.text = $"{newValue}";
        }
        else if (updateType == "subtract")
        {
            var newValue = currentValue -= currentMultiplier;
            if (newValue < 40)
                newValue = 40;
            kvpValueOnScreen.text = $"{newValue}";
        }
    }

    public void UpdateKVPIncrement(string settingType)
    {
        var currentValue = int.Parse(kvpIncreaseAmount.text);
        if (settingType == "add")
        {
            if (currentValue < 25)
                currentValue += 1;
        }
        else if (settingType == "subtract")
        {
            if (currentValue > 1)
                currentValue -= 1;
        }

        kvpIncreaseAmount.text = currentValue.ToString();
    }

    public void UpdateMASValue(string updateType)
    {
        var currentValue = float.Parse(masValueOnScreen.text);
        var currentMultiplier = float.Parse(masIncreaseAmount.text);

        if (updateType == "add")
        {
            var newValue = currentValue += currentMultiplier;
            if (newValue > 4.0f)
                newValue = 4.0f;
            masValueOnScreen.text = $"{newValue}";
        }
        else if (updateType == "subtract")
        {
            var newValue = currentValue -= currentMultiplier;
            if (newValue < 0.1f)
                newValue = 0.1f;
            masValueOnScreen.text = $"{newValue}";
        }
    }

    public void UpdateMASIncrement(string settingType)
    {
        var currentValue = float.Parse(masIncreaseAmount.text);
        if (settingType == "add")
        {
            if (currentValue < 5.0f)
                currentValue += 0.1f;
        }
        else if (settingType == "subtract")
        {
            if (currentValue > 0.1f)
                currentValue -= 0.1f;
        }

        masIncreaseAmount.text = currentValue.ToString();
    }
}
