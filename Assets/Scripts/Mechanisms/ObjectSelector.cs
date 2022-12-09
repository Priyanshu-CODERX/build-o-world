using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class ObjectSelector : MonoBehaviour
{
    private ARSessionOrigin sessionOrigin { get; set; }
    public TMP_Text debug;

    private void Start()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

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

                    hit.collider.gameObject.GetComponent<SelectedBehaviour>().SelectionIndicator.SetActive(true);
                }
            }
        }
    }
}
