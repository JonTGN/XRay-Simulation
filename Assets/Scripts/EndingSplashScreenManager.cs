using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

public class EndingSplashScreenManager : MonoBehaviour
{
    // this class will be used to inject scores into the splashscreen and display the xray image
    [SerializeField] private SliderManager kVpSlider;
    [SerializeField] private SliderManager mASSlider;

    public TextMeshPro kvpValue;
    public TextMeshPro masValue;

    [Header("This is getting set from JS Loader, DO NOT EDIT")]
    public SceneTypes selectedSceneType; // this is getting set from JS hook, DO NOT EDIT!

    private OptimalSettings optimalSettings = new OptimalSettings();
    private int optimalKVP;
    private float optimalMAS;
    private List<int> kvpRanges;
    private List<float> masRanges;

    private int kvpGrade;
    private int masGrade;
    private int maxGrade = 0; // add up len of all setting arrays
    private int grade = 0;

    // collimation handling
    public RandomizeXrayPosition randomizeXrayPosition;
    private float xRangeMoved;
    private float zRangeMoved;
    private Vector3 originOfTracker;
    private Vector3 currentPosOfTracker;

    // collimation accuracy
    [SerializeField] private TrackCollimationAccuracy trackCollimationAccuracy;


    // ui references
    [SerializeField] private TextMeshProUGUI finalScoreUIText;

    [SerializeField] private TextMeshProUGUI optimalkVpUIText;
    [SerializeField] private TextMeshProUGUI yourkVpUIText;
    [SerializeField] private TextMeshProUGUI optimalmASUIText;
    [SerializeField] private TextMeshProUGUI yourmASUIText;
    [SerializeField] private TextMeshProUGUI collimationAccuracyText;

    private int kVp;
    private float mAS;


    private void GetCollimationAccuracy()
    {
        string collimationClassification = trackCollimationAccuracy.collimationAccuracy;
        collimationAccuracyText.text = collimationClassification;

        if (collimationClassification == "high")
        {
            grade += 2;
        }
        else if (collimationClassification == "medium")
        {
            grade += 1;
        }
        else
        {
            // no increase in grade
        }
    }

    public void CalculateGrade()
    {
        // get some information from the cameras and how much they init moved so we can figure out how close usr is to right collimation point
        //GetCameraSettingsFromRandomizer();
        GetCollimationAccuracy();
        // old method of getting vals from sliders
        //kVp = (int)kVpSlider.mainSlider.value;
        //mAS = mASSlider.mainSlider.value;
        //mAS = (float)Math.Round(mAS, 1);

        kVp = int.Parse(kvpValue.text);
        mAS = float.Parse(masValue.text);

        // need to load optimal settings for this scene type
        Settings sceneSettings = optimalSettings.GetSetting(selectedSceneType);
        SetValues(sceneSettings);

        // figure out how far from the optimal values we are
        GetScoreFromInt(kvpRanges, optimalKVP, kVp);
        GetScoreFromFloat(masRanges, optimalMAS, mAS);

        // update the UI
        UpdateUIElements();
    }

    private void UpdateUIElements()
    {
        finalScoreUIText.text = $"{grade.ToString()} / {maxGrade.ToString()}";

        optimalkVpUIText.text = optimalKVP.ToString();
        yourkVpUIText.text = kVp.ToString();

        optimalmASUIText.text = optimalMAS.ToString();
        yourmASUIText.text = mAS.ToString();
    }

    private void GetScoreFromInt(List<int> ranges, int optimal, int current)
    {
        foreach (var range in ranges.Select((x, i) => new { Value = x, Index = i }))
        {
            if (Mathf.Abs(current - optimal) <= range.Value)
            {
                grade += 1;
            }
        }
    }

    private void GetScoreFromFloat(List<float> ranges, float optimal, float current)
    {
        foreach (var range in ranges.Select((x, i) => new { Value = x, Index = i }))
        {
            if (Mathf.Abs(current - optimal) <= range.Value)
            {
                grade += 1;
            }
        }
    }

    private void SetValues(Settings sceneSettings)
    {
        optimalKVP = sceneSettings.optimalKVP;
        optimalMAS = sceneSettings.optimalMAS;
        kvpRanges = sceneSettings.kvpRanges;
        masRanges = sceneSettings.masRanges;

        maxGrade += kvpRanges.Count;
        maxGrade += masRanges.Count;
        maxGrade += 2; // 3 for 3 tiers of collimation accuracy
    }

}
