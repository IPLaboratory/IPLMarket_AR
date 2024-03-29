using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private float initialDistance;
    private Vector3 prevScale;
    private Vector3 scaleUnit = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 maxScale = new Vector3(2f, 2f, 2f);
    private float prevHeight;

    private Vector2 prevTouchPos;
    private const float rotationSpeed = 0.05f;
    private float prevRotationAmount;
    private float rotationAngle = 0;

    private Vector3 correctionValueForScale = new Vector3(0.35f, 0.35f, 0.35f);

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
                    if (Input.touchCount == 3 && (controllingStatus == string.Empty || controllingStatus == "rotation"))
                    {
                        controllingStatus = "rotation";
                        FurnitureRotationByThreeFingers(touches[1]);
                    }
                    else if (Input.touchCount == 2 && (controllingStatus == string.Empty || controllingStatus == "zoom"))
                    {
                        controllingStatus = "zoom";
                        if (prevHeight == 0)
                        {
                            prevHeight = GetHeight(placeFurniture);
                        }
                        PinchZoom(touches[0], touches[1]);
                    }
                    else if (Input.touchCount == 1 && (controllingStatus == string.Empty || controllingStatus == "moving"))
                    {
                        controllingStatus = "moving";
                        FurnitureLocationByOneFinger(touches[0]);
                    }
                }

                if (touches[0].phase == TouchPhase.Ended)
                {
                    prevTouchPos = Vector2.zero;
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
                prevScale = correctionValueForScale;
                prevHeight = 0f;
                prevRotationAmount = 0f;
            }
            catch (Exception e)
            {

            }
        }
    }

    // Calculate object's height.
    private float GetHeight(GameObject gameObject)
    {
        MeshFilter meshFilter = gameObject.GetComponentInChildren<MeshFilter>();
        Bounds bounds = meshFilter.sharedMesh.bounds;
        return bounds.size.y;
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
                placeFurniture.transform.position = new Vector3(
                    hitPose.position.x, hitPose.position.y + (GetHeight(placeFurniture) / 2f * prevScale.x), hitPose.position.z);
            }
        }
    }

    // Pinchzoom to zoom/unzoom object(furniture).
    private void PinchZoom(Touch touch1, Touch touch2)
    {
        if (Input.touchCount == 2)
        {
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            if (initialDistance == 0f)
            {
                initialDistance = currentDistance;

            }
            else
            {
                if (initialDistance > currentDistance)
                {
                    if (minScale.x >= prevScale.x)
                        return;
                    furniturePool.transform.localScale = prevScale - scaleUnit;
                }
                else if (initialDistance < currentDistance)
                {
                    if (prevScale.x >= maxScale.x)
                        return;
                    furniturePool.transform.localScale = prevScale + scaleUnit;
                }

                prevScale = furniturePool.transform.localScale;

            }
        }
    }

    // Rotate object(furniture) by user's three finger touch and swipe.
    private void FurnitureRotationByThreeFingers(Touch touch2)
    {
        if (Input.touchCount == 3)
        {
            if (prevTouchPos == Vector2.zero)
            {
                prevTouchPos = touch2.position;
            }
            else
            {
                float horizontalMovement = touch2.position.x - prevTouchPos.x;
                float rotationAmount = horizontalMovement * rotationSpeed * -1;

                if (rotationAmount != prevRotationAmount)
                {
                    prevRotationAmount = rotationAmount;
                    rotationAngle = (rotationAngle + rotationAmount) % 360;

                    furniturePool.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);
                }
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
