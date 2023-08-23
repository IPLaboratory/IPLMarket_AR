using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceIndicator : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public GameObject placementIndicator;

    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    // Collocate the indicator at the position by UpdatePlacementPose().
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(
                new Vector3(placementPose.position.x, placementPose.position.y + 0.1f, placementPose.position.z), placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    // Update position of the indicator by center of raycast hit and center of the screen.
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        // Saving position to locate the indicator.
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }
}
