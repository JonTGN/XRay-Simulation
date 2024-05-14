using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomizeXrayPosition : MonoBehaviour
{
    public GameObject xrayGO; // track machine so we can kind of move machine in the right spot
    public GameObject collimationTrackerGO; // need to move this too

    public Camera xraySkinCam;
    public Camera xrayBoneCam;

    public float xRangeMoved = 0;
    public float zRangeMoved = 0;
    public Vector3 originOfTracker;

    void Start()
    {
        // TODO: Get these values from settings mngr
        // only for pa hands, send to settings mngr
        //x: -0.1 -> 0.2
        //z: -0.25 -> 0.2

        var currentPosOfCollimationTracker = collimationTrackerGO.transform.position;

        // TODO:
        // current pos is going off the xray item
        // all the cams are going off the skin/bones xray
        // these need to be aligned to track proper positioning

        // make sure the origin is right (based off collimation)
        // make sure distance traveled is right (this can just be debug, contstantly checking)
        // only really need to figure out the diff between current pos @ end pos when you score!

        originOfTracker = currentPosOfCollimationTracker;
        xRangeMoved = Random.Range(-0.1f, 0.2f);
        zRangeMoved = Random.Range(-0.25f, 0.2f);

        // init move xray model and collimation tracker object randomly
        //xrayGO.transform.position = new Vector3(currentPosOfCollimationTracker.x + xRangeMoved, currentPosOfCollimationTracker.y, currentPosOfCollimationTracker.z + zRangeMoved);
        collimationTrackerGO.transform.position = new Vector3(currentPosOfCollimationTracker.x + xRangeMoved, currentPosOfCollimationTracker.y, currentPosOfCollimationTracker.z + zRangeMoved);

        // offset the xray cameras by the same amount we init'd above ^^
        var skinPos = xraySkinCam.transform.position;
        var bonePos = xrayBoneCam.transform.position;
        xraySkinCam.transform.position = new Vector3(skinPos.x + xRangeMoved, skinPos.y, skinPos.z + zRangeMoved);
        xrayBoneCam.transform.position = new Vector3(bonePos.x + xRangeMoved, bonePos.y, bonePos.z + zRangeMoved);
    }
}
