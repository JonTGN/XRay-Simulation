using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimalSettings : MonoBehaviour
{
    private int optimalKVP;
    private List<int> kvpRanges = new List<int>();
    private float optimalMAS;
    private List<float> masRanges = new List<float>();

    public Settings GetSetting(SceneTypes sceneType)
    {
        if (sceneType == SceneTypes.PAHand)
        {
            return new Settings
            {
                optimalKVP = 60,
                kvpRanges = new List<int> { 20, 10, 5, 0 },
                optimalMAS = 1.5f,
                masRanges = new List<float> { 4.5f, 1.5f, 0.5f, 0 },
            };
        }

        else if (sceneType == SceneTypes.PAFoot)
        {
            return new Settings();
        }

        else
        {
            return new Settings();
        }
    }
}

public class Settings
{
    public int optimalKVP { get; set; }
    public List<int> kvpRanges { get; set; }
    public float optimalMAS { get; set; }
    public List<float> masRanges { get; set; }
}
