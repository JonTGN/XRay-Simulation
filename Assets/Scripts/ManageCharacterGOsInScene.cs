using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCharacterGOsInScene : MonoBehaviour
{
    public PatientPositioningManager patientPositioningManager;


    public void EnableAndDisable(int idx)
    {
        // loop thru each GO, and disable them, enable the one with same idx from above
        for (int i = 0; i < patientPositioningManager.XrayObjects.Count; i++)
        {
            if (idx == i)
            {
                patientPositioningManager.XrayObjects[i].SetActive(true);
                patientPositioningManager.VisibleObjects[i].SetActive(true);
            }
            else
            {
                patientPositioningManager.XrayObjects[i].SetActive(false);
                patientPositioningManager.VisibleObjects[i].SetActive(false);
            }
        }
    }
}
