using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Decor3DTouchManager : MonoBehaviour {

    public GameObject decorCanvas;
    private DecorPanelController decorPanelController;
    private bool isUITouch;

    void Start () {}

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

    void Update () {
        // check if touch is passing through ui
        if (Input.touchCount == 1)
        {
            isUITouch = IsUITouch();
            if (isUITouch == true) 
            {
                return;
            }
            else
            {
                decorPanelController = decorCanvas.GetComponent<DecorPanelController>();
                decorPanelController.CloseAllPanels();
            }
        }

        OneFingerRotate();
	}

    public void OneFingerRotate()
    {
        if (DecorManager.Instance.DecorModelConnection != null)
        {
            if (DecorManager.Instance.DecorModelConnection.DecorModel != null)
            {
                if (Input.touchCount == 1)
                {
                    isUITouch = IsUITouch();
                    if (isUITouch == true) return;
                }
                else
                {
                    isUITouch = false;
                }

                if (Input.touchCount == 0 || isUITouch)
                    DecorManager.Instance.DecorModelConnection.DecorModel.transform.Rotate(0f, 15 * Time.deltaTime, 0f);


                else
                {
                    Touch touch0 = Input.GetTouch(0);
                    if (touch0.phase == TouchPhase.Moved)
                    {
                        var angleY = -0.4f * touch0.deltaPosition.x;
                        DecorManager.Instance.DecorModelConnection.DecorModel.transform.Rotate(0f, angleY, 0f);
                    }
                }
            }
        }
       
    }
}
