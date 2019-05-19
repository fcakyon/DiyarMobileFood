using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Food3DTouchManager : MonoBehaviour {

    public GameObject foodCanvas;
    private FoodPanelController foodPanelController;
    public GameObject cameraParentAzimuth;
    private GameObject cameraParentElevation;
    private bool isUITouch;

    void Start () 
    {
        cameraParentElevation = cameraParentAzimuth.transform.GetChild(0).gameObject;
    }

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
                {
                    //FoodManager.Instance.FoodModelConnection.FoodModel.transform.Rotate(0f, 15 * Time.deltaTime, 0f);

                    // move camera in azimuth
                    cameraParentAzimuth.transform.Rotate(0f, -15 * Time.deltaTime, 0f);

                }
                else if (Input.touchCount == 1)
                {
                    Touch touch0 = Input.GetTouch(0);
                    if (touch0.phase == TouchPhase.Moved)
                    {
                        // calculate touch shift amounts in x and y directions
                        var angleY = -0.4f * touch0.deltaPosition.x;
                        var angleX = -0.4f * touch0.deltaPosition.y;
                        //FoodManager.Instance.FoodModelConnection.FoodModel.transform.Rotate(0f, angleY, 0f);

                        // move camera in azimuth
                        cameraParentAzimuth.transform.Rotate(0f, -angleY, 0f);

                        // get camera elevation angle
                        var cameraParentElevationDegree = cameraParentElevation.transform.eulerAngles.x;

                        // to convert the eulerangles to rotation value in editor
                        if (cameraParentElevationDegree > 180)
                        {
                            cameraParentElevationDegree = cameraParentElevationDegree - 360;
                        }

                        // move camera in elevation
                        if (cameraParentElevationDegree + angleX > 60)
                        {
                            // dont let the camera look from above
                            Vector3 currentRotationValues = cameraParentElevation.transform.eulerAngles;
                            cameraParentElevation.transform.eulerAngles = new Vector3 (60f, currentRotationValues.y, currentRotationValues.z);
                        }
                        else if (cameraParentElevationDegree + angleX < -30)
                        {
                            // dont let the camera look from below
                            Vector3 currentRotationValues = cameraParentElevation.transform.eulerAngles;
                            cameraParentElevation.transform.eulerAngles = new Vector3(330f, currentRotationValues.y, currentRotationValues.z);
                        }
                        else
                        {
                            cameraParentElevation.transform.Rotate(angleX, 0f, 0f);
                        }

                    }
                }
                else
                {
                    // pinch to zoom
                    float pinchAmount = 0;

                    // calculate pinch amount
                    TouchMovementCalculator.Calculate();
                    if (Mathf.Abs(TouchMovementCalculator.pinchDistanceDelta) > 0)
                    {
                        pinchAmount = TouchMovementCalculator.pinchDistanceDelta;
                    }

                    // sclae pinch amount
                    pinchAmount = pinchAmount * 0.003f;

                    // get current model scale (in terms of max collider size)
                    float newScale = ModelScaleTransformer.newModelScale;

                    // update scale based on pinch amount
                    newScale += pinchAmount * newScale;

                    // rescale model
                    ModelScaleTransformer.CustomModelScale3D(FoodManager.Instance.FoodModelConnection.FoodModel, newScale);

                }
            }
        }
    }
}
//}