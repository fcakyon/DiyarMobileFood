using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FoodManager : MonoBehaviour {

    //[HideInInspector]
    public FoodModelConnection foodModelConnection;
    //[HideInInspector]
    public FoodPositionConnection FoodPositionConnectionScript;
    public LayerMask layerMask;
    public GameObject fixButton;
    public float foodPositionSpeed = 4f;
    public bool isPlacing = false;
    public Vector3 lastPlacementPos;
    public bool is3DScene;

    private void Start()
    {
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (foodModelConnection != null)
        {
            FoodModelChange();
        }

        if (FoodPositionConnectionScript != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hit, 500.0f, layerMask))
            {
                FoodPositionPlacement(hit.point);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            if (isPlacing == false && FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {
                HideFoodPosition();
            }
            isPlacing = false;
        }
    }

    public void FoodModelChange()
    {
        if (foodModelConnection != null)
        {
            if (foodModelConnection.foodModel != null)
            {
                if (foodModelConnection.hasFoodModelBeenChanged == false)
                {
                    foodModelConnection.GetGameObjectToPlace().SetActive(true);
                    foodModelConnection.GetGameObjectToPlace().transform.parent = null;
                    if (is3DScene == false)
                    {
                        foodModelConnection.GetGameObjectToPlace().transform.SetParent(GameObject.Find("Plane").transform);
                    }
                    foodModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
                    foodModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0); // bu gerekli
                    foodModelConnection.hasFoodModelBeenChanged = true;
                    if (!foodModelConnection.GetGameObjectToPlace().activeSelf)
                    {
                        foodModelConnection.GetGameObjectToPlace().SetActive(true);
                    }
                }
            }
        }
    }

    public void FoodPositionPlacement(Vector3 newPos)
    {
        if (FoodPositionConnectionScript != null)
        {
            if (FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {
                isPlacing = true;
                lastPlacementPos = newPos;
                FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(true);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.SetParent(null);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.SetParent(GameObject.Find("Plane").transform);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.position = Vector3.Lerp(FoodPositionConnectionScript.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * foodPositionSpeed);
                if (!FoodPositionConnectionScript.GetGameObjectToPlace().activeSelf)
                {
                    FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }

    public void FixFoodPosition()
    {
        if (FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
        {
            fixButton.SetActive(false);
            FoodPositionConnectionScript.hasFoodPositionBeenPlaced = true;
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.position = lastPlacementPos;
        }
    }

    public void SetFoodModel(FoodModelConnection foodModelConnection)
    {
        ShouldWeHideFoodModel();
        this.foodModelConnection = foodModelConnection;
    }

    public void SetFoodPosition(FoodPositionConnection FoodPositionConnectionScript)
    {
        ShouldWeHideFoodPosition();
        this.FoodPositionConnectionScript = FoodPositionConnectionScript;
    }

    public void ShouldWeHideFoodModel(){
        if (foodModelConnection != null)
        {
            HideFoodModel();
        }
    }

    public void ShouldWeHideFoodPosition()
    {
        if (FoodPositionConnectionScript != null)
        {
            if (FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {
                HideFoodPosition();
            }
        }
    }

    public void HideFoodModel(){
        if (foodModelConnection != null)
        {
            if (is3DScene == false)
            {
                FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(false); // bu gerekli, reset atıp fixleyince position modeli silinmiyor yoksa
            }
            foodModelConnection.DestroyFoodModel();
        }
    }

    public void HideFoodPosition()
    {
        if (FoodPositionConnectionScript != null)
        {
            FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(false);
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.SetParent(Camera.main.transform);
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveFoodModelConnection(){
        foodModelConnection = null;
    }

    public void RemoveFoodPositionConnection()
    {
        FoodPositionConnectionScript = null;
    }

    public void LoadFoodARScene()
    {
        HideFoodModel();
        SceneManager.LoadScene("FoodARScene");
    }

    public void LoadFood3DScene()
    {
        HideFoodModel();
        SceneManager.LoadScene("Food3DScene");
    }

}
