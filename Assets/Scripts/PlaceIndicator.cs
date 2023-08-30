using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class PlaceIndicator : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public GameObject placementIndicator;
    public Button button;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool isFurnitueOnline = false;
    private TextMeshProUGUI buttonText;

    void Awake()
    {
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        
        IsFurnitureOnline();
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    // Collocate the indicator at the position by UpdatePlacementPose().
    private void UpdatePlacementIndicator()
    {
        
        if (placementPoseIsValid)
        {
            if (!isFurnitueOnline)
            {
                placementIndicator.SetActive(true);
            }

            placementIndicator.transform.SetPositionAndRotation(
                new Vector3(placementPose.position.x, placementPose.position.y + 0.1f, placementPose.position.z), placementPose.rotation);
        }
        else
        {
            if (!isFurnitueOnline)
            {
                placementIndicator.SetActive(false);
            }
        }
    }

    // Update position of the indicator by center of raycast hit and center of the screen.
    private void UpdatePlacementPose()
    {
        Camera arCamera = Camera.main;
        var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        
        // Saving position to locate the indicator.
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }

    //Hiding the indicator when furniture is spawned.
    private void IsFurnitureOnline()
    {
        
        if (buttonText.text.Equals("Image_Input"))
        {
            isFurnitueOnline = false;
        }
        else if (buttonText.text.Equals("Delete"))
        {
            isFurnitueOnline = true;
        }
    }
}
