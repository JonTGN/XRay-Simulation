using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
ATTRIBUTIONS:
<a href="https://www.flaticon.com/free-icons/radiation" title="radiation icons">Radiation icons created by juicy_fish - Flaticon</a>
*/

public class XRAYUIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> uiElements = new List<GameObject>();

    [SerializeField]
    private List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();

    public void MachineSettings()
    {
        // this is the first setting that is loaded
        LoadUIElement(0);
        Debug.Log("mac settings");
    }

    public void PatientPositioning()
    {
        LoadUIElement(1);
        Debug.Log("pat pos");
    }

    public void Collimate()
    {
        LoadUIElement(2);
        Debug.Log("collimate");
    }

    public void XRAY()
    {
        LoadUIElement(3);
        Debug.Log("xray");
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

}
