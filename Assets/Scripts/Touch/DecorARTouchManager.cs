using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;

public class DecorARTouchManager : MonoBehaviour
{

    public GameObject currentSelected;
    public LayerMask modelLayerMask;
    public LayerMask planeLayerMask;
    private bool isUITouch;
    public GameObject decorCanvas;
    private DecorPanelController decorPanelController;

    void Start() {}

    bool IsUITouch()
    {
        //if (Input.touchCount == 1)
        //{
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.GetTouch(0).position;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 4) //number of border panel elements
            {
                return true;
            }
                return false;
        //}
            //return false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.touchCount == 1)
        {
            // check if touch is passing through ui
            isUITouch = IsUITouch();
            // dont continue if touch is passing through ui
            if (isUITouch == true) return;

            // close panels if canvas is active and touch is not passing through ui
            if (decorCanvas.activeSelf == true)
            { 
            decorPanelController = decorCanvas.GetComponent<DecorPanelController>();
            decorPanelController.CloseAllPanels();
            }

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit, 500f, modelLayerMask))
                {
                    currentSelected = hit.collider.gameObject.transform.parent.gameObject;
                    DecorManager.Instance.SetDecorModelConnectionUsingModel(currentSelected);
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (currentSelected != null)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit, 500f, planeLayerMask))
                    {
                        currentSelected.transform.position = hit.point;
                    }
                }
            }
        }

        else if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                TouchMovementCalculator.Calculate();
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(TouchMovementCalculator.avgTouchPosition);
                if (Physics.Raycast(ray, out hit, 500f, modelLayerMask))
                {
                    currentSelected = hit.collider.gameObject.transform.parent.gameObject;
                    DecorManager.Instance.SetDecorModelConnectionUsingModel(currentSelected);
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (currentSelected != null)
                {
                    if (currentSelected.activeSelf == true)
                    {
                        //if (scaleText.IsActive() == false)
                        //{
                        //    scaleText.enabled = true;
                        //}

                        //float pinchAmount = 0;

                        Quaternion desiredRotation = currentSelected.transform.rotation;

                        TouchMovementCalculator.Calculate();

                        //if (Mathf.Abs(TouchMovementCalculator.pinchDistanceDelta) > 0)
                        //{ // zoom
                        //    pinchAmount = TouchMovementCalculator.pinchDistanceDelta;
                        //}

                        //pinchAmount = pinchAmount * 0.001f;
                        //Vector3 newScale = currentSelected.transform.localScale;

                        //newScale += pinchAmount * currentSelected.transform.localScale;

                        //if (newScale.x > 0.5 && newScale.x < 2)
                        //{
                        //    currentSelected.transform.localScale = newScale;
                        //    //scaleText.text = "Scale is: " + (Math.Round(newScale.x, 1)).ToString();
                        //}

                        //Debug.Log(TouchMovementCalculator.turnAngleDelta);
                        if (Mathf.Abs(TouchMovementCalculator.turnAngleDelta) > 0)
                        { // rotate
                            Vector3 rotationDeg = Vector3.zero;
                            rotationDeg.y = -TouchMovementCalculator.turnAngleDelta;
                            desiredRotation *= Quaternion.Euler(rotationDeg);
                        }

                        desiredRotation.x = 0;
                        desiredRotation.z = 0;

                        currentSelected.transform.rotation = desiredRotation;
                    }
                }
            }
            else
            {
                //scaleText.enabled = false;
                //currentSelected = null;
            }
        }
    }
}
