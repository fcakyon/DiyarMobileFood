using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }
    //[HideInInspector]
    public FoodModelConnection foodModelConnection;
    public LayerMask planeLayerMask;
    public GameObject fixButton;
    public GameObject resetButton;

    public float modelLerpSpeed = 4f;
    public bool isPlacing = false;
    public bool isChanging = false;
    public bool hasFoodModelBeenPlaced;
    public Vector3 lastPlacementPos;
    GameObject surfacePlane;
    public bool is3DScene;

    public FoodModelConnection FoodModelConnection
    {
        get {return foodModelConnection;}
        set
        {
            if (value != null) {
                FoodModelConnection foodModelConnectionGameObject = Instantiate(value);
                DontDestroyOnLoad(foodModelConnectionGameObject);
                Instance.foodModelConnection = foodModelConnectionGameObject;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        surfacePlane = GameObject.Find("Plane");
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }


    void Update()
    {
        if (Instance.FoodModelConnection != null && Instance.FoodModelConnection.foodModel != null && hasFoodModelBeenPlaced != true)
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
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void PlaceFoodModel(Vector3 newPos)
    {
        lastPlacementPos = newPos;
        Instance.FoodModelConnection.GetGameObjectToPlace().SetActive(true);
        Instance.FoodModelConnection.GetGameObjectToPlace().transform.SetParent(null);
        if (is3DScene == false)
        {
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.SetParent(GameObject.Find("Plane").transform);
        }
        Instance.FoodModelConnection.GetGameObjectToPlace().transform.position = Vector3.Lerp(Instance.FoodModelConnection.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * modelLerpSpeed);
    }

    public void ChangeFoodModel()
    {
        if (Instance.FoodModelConnection != null && Instance.FoodModelConnection.foodModel != null)
        {
            Instance.FoodModelConnection.GetGameObjectToPlace().SetActive(true);
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.SetParent(null);
            if (is3DScene == false)
            {
                Instance.FoodModelConnection.GetGameObjectToPlace().transform.SetParent(surfacePlane.transform);
            }
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0); // bu gerekli
            isChanging = false;
        }
    }

    public void FixFoodModelPlace()
    {
        if (hasFoodModelBeenPlaced == false)
        {
            HideFixButton();
            hasFoodModelBeenPlaced = true;
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
            Vector3 localPosition = Instance.FoodModelConnection.GetGameObjectToPlace().transform.localPosition;
            localPosition.y = 0;
            Instance.FoodModelConnection.GetGameObjectToPlace().transform.localPosition = localPosition;
        }
    }

    public void DestroyFoodModel()
    {
        if (Instance.FoodModelConnection != null) Instance.FoodModelConnection.DestroyFoodModel();
    }

    public void RemoveFoodModelConnection()
    {
        Instance.FoodModelConnection = null;
    }

    public void LoadFoodARScene()
    {
        SceneManager.LoadScene("FoodARScene");
        is3DScene = false;
    }

    public void LoadFood3DScene()
    {
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
