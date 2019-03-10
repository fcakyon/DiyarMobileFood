using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FoodManager : MonoBehaviour
{

    //[HideInInspector]
    public FoodModelConnection foodModelConnection;
    //[HideInInspector]
    public FoodPositionConnection FoodPositionConnectionScript;
    public LayerMask planeLayerMask;
    public GameObject fixButton;
    public GameObject resetButton;

    public float foodModelLerpSpeed = 4f;
    public bool isPlacing = false;
    public bool isChanging = false;
    public bool hasFoodModelBeenPlaced;
    public Vector3 lastPlacementPos;

    public bool is3DScene;

    private void Start()
    {
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (foodModelConnection != null && foodModelConnection.foodModel != null && hasFoodModelBeenPlaced != true)
        {
            AutoPlaceModel();
        }
        else if(isChanging == true)
        {
            ChangeFoodModel();
        }
    }

    public void AutoPlaceModel()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 500.0f, planeLayerMask))
        {
            PlaceFoodModel(hit.point);
            foodModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void PlaceFoodModel(Vector3 newPos)
    {
        lastPlacementPos = newPos;
        foodModelConnection.GetGameObjectToPlace().SetActive(true);
        foodModelConnection.GetGameObjectToPlace().transform.SetParent(null);
        if (is3DScene == false)
        {
            foodModelConnection.GetGameObjectToPlace().transform.SetParent(GameObject.Find("Plane").transform);
        }
        foodModelConnection.GetGameObjectToPlace().transform.position = Vector3.Lerp(foodModelConnection.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * foodModelLerpSpeed);
    }

    public void ChangeFoodModel()
    {
        if (foodModelConnection != null && foodModelConnection.foodModel != null)
        {
            foodModelConnection.GetGameObjectToPlace().SetActive(true);
            foodModelConnection.GetGameObjectToPlace().transform.SetParent(null);
            if (is3DScene == false)
            {

                foodModelConnection.GetGameObjectToPlace().transform.SetParent(GameObject.Find("Plane").transform);
            }
            foodModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
            foodModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0); // bu gerekli
            isChanging = false;
        }
    }

    public void FixFoodModelPlace()
    {
        if (hasFoodModelBeenPlaced == false)
        {
            HideFixButton();
            hasFoodModelBeenPlaced = true;
            foodModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
            Vector3 localPosition = foodModelConnection.GetGameObjectToPlace().transform.localPosition;
            localPosition.y = 0;
            foodModelConnection.GetGameObjectToPlace().transform.localPosition = localPosition;
        }
    }

    public void SetFoodModelConnection(FoodModelConnection foodModelConnection)
    {
        DestroyFoodModel();
        this.foodModelConnection = foodModelConnection;
    }

    public void DestroyFoodModel()
    {
        if (foodModelConnection != null)
        {
            foodModelConnection.DestroyFoodModel();
        }
    }

    public void RemoveFoodModelConnection()
    {
        foodModelConnection = null;
    }

    public void LoadFoodARScene()
    {
        DestroyFoodModel();
        SceneManager.LoadScene("FoodARScene");
    }

    public void LoadFood3DScene()
    {
        DestroyFoodModel();
        SceneManager.LoadScene("Food3DScene");
    }

    public void ApearFixButton()
    {
        if (hasFoodModelBeenPlaced == false) fixButton.SetActive(true);
    }

    public void HideFixButton()
    {
        fixButton.SetActive(false); ;
    }

    public void ApearResetButton()
    {
        resetButton.SetActive(true); ;
    }

    public void HideResetButton()
    {
        resetButton.SetActive(false); ;
    }

    public void Reset()
    {
        hasFoodModelBeenPlaced = false;
        DestroyFoodModel();
        RemoveFoodModelConnection();
    }

}
