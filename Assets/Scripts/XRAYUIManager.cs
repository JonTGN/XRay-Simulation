using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

/*
ATTRIBUTIONS:
<a href="https://www.flaticon.com/free-icons/radiation" title="radiation icons">Radiation icons created by juicy_fish - Flaticon</a>
*/

public class XRAYUIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> colElems = new List<GameObject>();

    [SerializeField]
    private List<GameObject> uiElements = new List<GameObject>();

    [SerializeField]
    private List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();

    [SerializeField]
    private GameObject outroScreen;

    [SerializeField]
    private GameObject menu;

    [SerializeField] private EndingSplashScreenManager ESSManager;

    public void MachineSettings()
    {
        // this is the first setting that is loaded
        LoadUIElement(0);
    }

    public void PatientPositioning()
    {
        LoadUIElement(1);
    }

    public void Collimate()
    {
        LoadUIElement(2);
    }

    public void XRAY()
    {
        LoadUIElement(3);
    }

    public void HideColElems()
    {
        foreach (var elem in colElems)
        {
            elem.SetActive(!elem.activeSelf);
        }
    }

    private void LoadUIElement(int idx)
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (i != idx)
            {
                uiElements[i].SetActive(false);
                vCams[i].Priority = 0;
            }
            else
            {
                vCams[i].Priority = 1;  // set this cam as new default
                uiElements[i].SetActive(true);
            }
        }
    }

    public void LoadFinalXRayResult()
    {
        ESSManager.CalculateGrade(); // this function will apply correct scoring to the outro
        LoadUIElement(999999); // hide all UI elements
        vCams[vCams.Count - 1].Priority = 1; // keep last cam hidden


        outroScreen.SetActive(true);
        menu.SetActive(false);
    }

}
