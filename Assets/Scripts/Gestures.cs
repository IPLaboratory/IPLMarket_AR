using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Gestures : MonoBehaviour
{
    public Transform furniturePool;
    public ARRaycastManager arRaycastManager;

    private GameObject placeFurniture;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private string controllingStatus = "";
    private bool ignoreTouchInput = false;
    private float touchIgnoreTimer = 0f;
    private float touchIgnoreDuration = 0.3f;

    private bool isButtonTouched = false;
    private bool buttonTouched = false;

    // Update is called once per frame
    void Update()
    {
        if (placeFurniture)
        {
            if (Input.touchCount > 0)
            {
                Touch[] touches = Input.touches;

                if (touches[0].phase == TouchPhase.Began)
                {
                    ignoreTouchInput = true;
                }

                if (ignoreTouchInput)
                {
                    touchIgnoreTimer += Time.deltaTime;
                    if (touchIgnoreTimer >= touchIgnoreDuration)
                    {
                        ignoreTouchInput = false;
                    }
                }

                if (!ignoreTouchInput)
                {
                    if (Input.touchCount == 1 && (controllingStatus == string.Empty || controllingStatus == "moving"))
                    {
                        controllingStatus = "moving";
                        FurnitureLocationByOneFinger(touches[0]);
                    }
                }

                if (touches[0].phase == TouchPhase.Ended)
                {
                    controllingStatus = "";
                    ignoreTouchInput = true;
                    touchIgnoreTimer = 0f;
                }
            }
        }
        else
        {
            try
            {
                placeFurniture = furniturePool.GetChild(0).gameObject;
            }
            catch (Exception e)
            {

            }
        }
    }

    // Change Position of object(furniture) by user's one finger touch.
    private void FurnitureLocationByOneFinger(Touch touch1)
    {
        if (Input.touchCount == 1)
        {
            Vector2 touchPosition = touch1.position;

            if (isButtonTouched)
            {
                return;
            }

            if (!isButtonTouched && buttonTouched)
            {
                buttonTouched = false;
                return;
            }

            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                furniturePool.transform.position = new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z);
            }
        }
    }

    // Checking UI button event to ignore touch above button.(buttonDown)
    public void ButtonDown()
    {
        isButtonTouched = true;
        buttonTouched = true;
    }

    // Checking UI button event to ignore touch above button.(buttonUP)
    public void ButtonUp()
    {
        isButtonTouched = false;
    }
}
