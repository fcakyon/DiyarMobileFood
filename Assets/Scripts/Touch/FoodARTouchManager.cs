using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodARTouchManager : MonoBehaviour {

    public GameObject foodCanvas;
    private FoodPanelController foodPanelController;
    private bool isUITouch;

    void Start () {}

    bool IsUITouch()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.GetTouch(0).position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 4) //number of border panel elements
        {
            return true;
        }
        return false;
    }

    void Update () {
        if (Input.touchCount == 1)
        {
            isUITouch = IsUITouch();
            if (isUITouch == true)
            {
                return;
            }
            else
            {
                foodPanelController = foodCanvas.GetComponent<FoodPanelController>();
                foodPanelController.CloseAllPanels();
            }
        }

        OneFingerRotate();
    }

    public void OneFingerRotate()
    {
        if (FoodManager.Instance.FoodModelConnection != null)
        {
            if (FoodManager.Instance.FoodModelConnection.FoodModel != null)
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
                    FoodManager.Instance.FoodModelConnection.FoodModel.transform.Rotate(0f, 15 * Time.deltaTime, 0f);
                else
                {
                    Touch touch0 = Input.GetTouch(0);
                    if (touch0.phase == TouchPhase.Moved)
                    {
                        var angleY = -0.4f * touch0.deltaPosition.x;
                        FoodManager.Instance.FoodModelConnection.FoodModel.transform.Rotate(0f, angleY, 0f);
                    }
                }
            }
        }
    }
}
