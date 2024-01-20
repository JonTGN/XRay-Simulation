using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HandleXRayMovement : MonoBehaviour
{
    private bool isPositioning = true;

    [SerializeField]
    private Transform vCamTracker;

    float sensitivity = 1f;

    public void Reset()
    {
        // todo: code functionality to reset camera pos/tilt (only reset whichever is selected?)
    }

    public void HandleSwitch()
    {
        isPositioning = !isPositioning;
    }

    // hard-coded based on orientation of xray/patient!!
    public void HandleMovement(string pos)
    {
        if (pos == "Up")
        {
            if (isPositioning)
                vCamTracker.position = vCamTracker.position - new Vector3(sensitivity * Time.deltaTime, 0, 0);
        }

        else if (pos == "Down")
        {
            if (isPositioning)
                vCamTracker.position = vCamTracker.position + new Vector3(sensitivity * Time.deltaTime, 0, 0);
        }

        else if (pos == "Left")
        {
            if (isPositioning)
                vCamTracker.position = vCamTracker.position - new Vector3(0, 0, sensitivity * Time.deltaTime);
        }

        else if (pos == "Right")
        {
            if (isPositioning)
                vCamTracker.position = vCamTracker.position + new Vector3(0, 0, sensitivity * Time.deltaTime);
        }
    }
}
