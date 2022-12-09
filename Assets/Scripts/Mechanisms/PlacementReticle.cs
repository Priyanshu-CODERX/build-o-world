using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class PlacementReticle : MonoBehaviour
{
    public GameObject Object;
    private GameObject spawnedObject;
    //public TMP_Text debug;
    
    private ARSessionOrigin m_SessionOrigin;
    public ARSessionOrigin sessionOrigin
    {
        get => m_SessionOrigin;
        set => m_SessionOrigin = value;
    } 

    private ARRaycastManager m_RaycastManager;
    public ARRaycastManager raycastManager
    {
        get => m_RaycastManager;
        set => m_RaycastManager = value;
    }

    [SerializeField]
    GameObject m_ReticlePrefab;

    public GameObject reticlePrefab
    {
        get => m_ReticlePrefab;
        set => m_ReticlePrefab = value;
    }

    GameObject m_SpawnedReticle;
    TrackableType m_RaycastMask;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    //static List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    void Start()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        raycastManager = GetComponent<ARRaycastManager>();
        m_RaycastMask = TrackableType.PlaneWithinPolygon;
        m_SpawnedReticle = Instantiate(m_ReticlePrefab);
        m_SpawnedReticle.SetActive(false);
    }

    void Update()
    {
        /*if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }*/
        var screenCenter = m_SessionOrigin.camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (m_RaycastManager.Raycast(screenCenter, s_Hits, m_RaycastMask))
        {
            Pose hitPose = s_Hits[0].pose;
            m_SpawnedReticle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            m_SpawnedReticle.SetActive(true);
        }

        /*if (Input.touchCount == 0) return;

        RaycastHit hit;
        Ray ray = sessionOrigin.camera.ScreenPointToRay(Input.GetTouch(0).position);

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "ARObject")
                {
                    Debug.Log(hit.collider.gameObject.name);
                    debug.text = hit.collider.gameObject.name.ToString();
                }
            }
        }*/

    }

    public Transform GetTransform()
    {
        return m_SpawnedReticle.transform;
    }

    public void PlaceObject()
    {
        spawnedObject = Instantiate(Object, m_SpawnedReticle.transform.position, m_SpawnedReticle.transform.rotation);
        if (spawnedObject.GetComponent<ARAnchor>() == null) spawnedObject.AddComponent<ARAnchor>();
    }

    public void ChangeARObject(GameObject ReplaceObject) => Object = ReplaceObject;

}
