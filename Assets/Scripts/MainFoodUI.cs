using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;

public class MainFoodUI : MonoBehaviour {
    
    public FoodModelConnection FoodModelConnectionScript;
    public FoodPositionConnection FoodPositionConnectionScript;

    public LayerMask layerMask;

    public GameObject fixButton;

    public float foodPositionSpeed = 4f;

    public bool isPlacing = false;

    public Vector3 lastPlacementPos;


    void Update()
    {
        if (FoodModelConnectionScript != null)
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
        if (FoodModelConnectionScript != null)
        {
            if (FoodModelConnectionScript.hasFoodModelBeenChanged == false)
            {
                FoodModelConnectionScript.GetGameObjectToPlace().SetActive(true);
                FoodModelConnectionScript.GetGameObjectToPlace().transform.parent = null;
                FoodModelConnectionScript.GetGameObjectToPlace().transform.position = lastPlacementPos;
                FoodModelConnectionScript.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0); // bu gerekli
                FoodModelConnectionScript.hasFoodModelBeenChanged = true;
                if (!FoodModelConnectionScript.GetGameObjectToPlace().activeSelf)
                {
                    FoodModelConnectionScript.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }

    public void FoodPositionPlacement(Vector3 newPos)
    {
        if (FoodPositionConnectionScript != null)
        {
            //Debug.Log("hasItemBeenPlaced: "+ItemPlacedController.hasItemBeenPlaced);
            if (FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {
                isPlacing = true;
                lastPlacementPos = newPos;
                FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(true);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.parent = null;
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

    public void SetFoodModel(FoodModelConnection FoodModelConnectionScript)
    {
        ShouldWeHideFoodModel();
        this.FoodModelConnectionScript = FoodModelConnectionScript;
    }

    public void SetFoodPosition(FoodPositionConnection FoodPositionConnectionScript)
    {
        ShouldWeHideFoodPosition();
        this.FoodPositionConnectionScript = FoodPositionConnectionScript;
    }

    public void ShouldWeHideFoodModel(){
        if (FoodModelConnectionScript != null)
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
        if (FoodModelConnectionScript != null)
        {
            FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(false); // bu gerekli, reset atıp fixleyince position modeli silinmiyor yoksa
            FoodModelConnectionScript.GetGameObjectToPlace().SetActive(false);
            FoodModelConnectionScript.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            FoodModelConnectionScript.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void HideFoodPosition()
    {
        if (FoodModelConnectionScript != null)
        {
            FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(false);
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveFoodModelConnection(){
        FoodModelConnectionScript = null;
    }

    public void RemoveFoodPositionConnection()
    {
        FoodPositionConnectionScript = null;
    }

}
