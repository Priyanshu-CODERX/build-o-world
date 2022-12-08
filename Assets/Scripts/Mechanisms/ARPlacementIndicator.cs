using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacementIndicator : MonoBehaviour
{
    public GameObject ARObject;
    public GameObject Reticle;

    private ARSessionOrigin sessionOrigin { get; set; }
    private ARRaycastManager raycastManager { get; set; }

    private GameObject spawnedObject;
    private Pose PlacementPose;
    private bool placementPoseIsValid = false;

    private ARPlaneManager planeManager;

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    // need to update placement indicator, placement pose and spawn 
    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }


        UpdatePlacementPose();
        UpdateReticle();


    }
    void UpdateReticle()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            Reticle.SetActive(true);
            Reticle.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            Reticle.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = sessionOrigin.camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;

            // Calibrate Camera Orientation
            var cameraForward = sessionOrigin.camera.transform.forward;
            var cameraOrientation = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraOrientation);
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(ARObject, PlacementPose.position, PlacementPose.rotation);

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

    }
}
