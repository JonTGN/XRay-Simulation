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

    public void Tilt()
    {
        // for tilt set look at object in collimate vcam and adjust body params I think if it is off
    }

    // hard-coded based on orientation of xray/patient!!
    public void Move(string pos)
    {
        if (isPositioning)
        {
            if (pos == "Up")
                vCamTracker.position = vCamTracker.position - new Vector3(sensitivity * Time.deltaTime, 0, 0);
            else if (pos == "Down")
                vCamTracker.position = vCamTracker.position + new Vector3(sensitivity * Time.deltaTime, 0, 0);
            else if (pos == "Left")
                vCamTracker.position = vCamTracker.position - new Vector3(0, 0, sensitivity * Time.deltaTime);
            else if (pos == "Right")
                vCamTracker.position = vCamTracker.position + new Vector3(0, 0, sensitivity * Time.deltaTime);
        }
    }
}
