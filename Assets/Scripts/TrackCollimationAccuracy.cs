using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TrackCollimationAccuracy : MonoBehaviour
{
    [SerializeField] private RandomizeXrayPosition randomizeXrayPosition;
    [SerializeField] TextMeshProUGUI accuracyClassificationTMP;
    [SerializeField] TextMeshProUGUI accuracyValueTMP;
    public float collimationDistance;
    public string collimationAccuracy;

    // TODO: When we are done debugging, remove this update since we no longer need updates to UI all the time, just let
    // final score screen call the update function below
    void Update()
    {
        UpdateCollimationAccuracy();
    }

    public void UpdateCollimationAccuracy()
    {
        Vector3 origin = randomizeXrayPosition.originOfTracker;
        GameObject collimationTracker = randomizeXrayPosition.collimationTrackerGO;
        Vector3 currentCollimationPos = collimationTracker.transform.position;
        collimationDistance = Vector3.Distance(origin, currentCollimationPos);
        collimationDistance = Mathf.Round(collimationDistance * 100.0f) * 0.01f; // round to 2 points

        //Debug.Log("Distance: " + collimationDistance);

        // +- .2 medium
        // +- .4 low

        collimationAccuracy = "";
        if (collimationDistance < 0.02f)
            collimationAccuracy = "high";
        else if (collimationDistance < 0.04f)
            collimationAccuracy = "medium";
        else
            collimationAccuracy = "low";

        accuracyClassificationTMP.text = collimationAccuracy;
        accuracyValueTMP.text = collimationDistance.ToString();
    }
}
