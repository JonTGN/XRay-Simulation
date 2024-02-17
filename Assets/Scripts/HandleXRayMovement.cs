using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HandleXRayMovement : MonoBehaviour
{
    private bool isPositioning = true;

    [SerializeField]
    private float sensitivity = 1f;

    [SerializeField]
    private Transform vCamTracker;

    // some variables to set the default camera's position to cinemachine
    [SerializeField]
    private Camera xrayPicCam;


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
            var moveAmount = sensitivity * Time.deltaTime;
            if (pos == "Up")
            {
                var vec3 = new Vector3(moveAmount, 0, 0);
                vCamTracker.position -= vec3;
                xrayPicCam.transform.position -= vec3;
            }
            else if (pos == "Down")
            {
                var vec3 = new Vector3(moveAmount, 0, 0);
                vCamTracker.position += vec3;
                xrayPicCam.transform.position += vec3;
            }
            else if (pos == "Left")
            {
                var vec3 = new Vector3(0, 0, moveAmount);
                vCamTracker.position -= vec3;
                xrayPicCam.transform.position -= vec3;
            }
            else if (pos == "Right")
            {
                var vec3 = new Vector3(0, 0, moveAmount);
                vCamTracker.position += vec3;
                xrayPicCam.transform.position += vec3;
            }
        }
    }
}
