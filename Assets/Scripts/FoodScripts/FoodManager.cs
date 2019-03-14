using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class FoodManager : MonoBehaviour
{
    //TODO Instance values will be checked for all Instance
    public static FoodManager Instance { get; private set; }
    //[HideInInspector]
    public FoodModelConnection foodModelConnection;
    public LayerMask planeLayerMask;
    public float modelLerpSpeed = 4f;
    public bool isPlacing;
    public bool isChanging;
    public bool hasFoodModelBeenPlaced;
    public Vector3 lastPlacementPos;
    GameObject surfacePlane;
    public bool is3DScene;
    public enum UIStates { Idle, Loading, AutoPlace, Fixed};
    private UIStates uiState; // Set to Idle Initially
    public UnityEvent OnUIStateChange = new UnityEvent();

    public FoodModelConnection FoodModelConnection
    {
        get { return Instance.foodModelConnection; }
        set {
            if (value != null) {
                FoodModelConnection foodModelConnectionGameObject = Instantiate(value);
                Instance.foodModelConnection = foodModelConnectionGameObject;
            }
        }
    }

    public UIStates UiState
    {
        get { return Instance.uiState; }
        set {
            Instance.uiState = value;
            Instance.OnUIStateChange.Invoke();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        surfacePlane = GameObject.Find("Plane");
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = Vector3.zero;
    }


    void Update()
    {
        Debug.Log("State: " + uiState);
        if (FoodModelConnection != null && FoodModelConnection.FoodModel != null && hasFoodModelBeenPlaced != true)
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
            FoodModelConnection.FoodModel.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void PlaceFoodModel(Vector3 newPos)
    {
        lastPlacementPos = newPos;
        FoodModelConnection.FoodModel.SetActive(true);
        FoodModelConnection.FoodModel.transform.SetParent(null);
        if (is3DScene == false)
        {
            if (surfacePlane == null) surfacePlane = GameObject.Find("Plane");
            FoodModelConnection.FoodModel.transform.SetParent(surfacePlane.transform);
        }
        FoodModelConnection.FoodModel.transform.position = Vector3.Lerp(FoodModelConnection.FoodModel.transform.position, newPos, Time.deltaTime * modelLerpSpeed);
    }

    public void ChangeFoodModel()
    {
        if (FoodModelConnection != null && FoodModelConnection.FoodModel != null)
        {
            FoodModelConnection.FoodModel.SetActive(true);
            FoodModelConnection.FoodModel.transform.SetParent(null);
            if (is3DScene == false)
            {
                FoodModelConnection.FoodModel.transform.SetParent(surfacePlane.transform);
            }
            FoodModelConnection.FoodModel.transform.position = lastPlacementPos;
            FoodModelConnection.FoodModel.transform.rotation = new Quaternion(0, 0, 0, 0); // bu gerekli
            isChanging = false;
        }
    }

    public void ChangeStateAfterLoading()
    {
        if (is3DScene)
            Instance.UiState = (int)UIStates.Idle;
        else
        {
            if(hasFoodModelBeenPlaced)
                Instance.UiState = UIStates.Fixed;
            else
                Instance.UiState = UIStates.AutoPlace;
        }
    }

    public void FixFoodModelPlace()
    {
        if (hasFoodModelBeenPlaced == false)
        {
            UiState = UIStates.Fixed;
            hasFoodModelBeenPlaced = true;
            FoodModelConnection.FoodModel.transform.position = lastPlacementPos;
            Vector3 localPosition = FoodModelConnection.FoodModel.transform.localPosition;
            localPosition.y = 0;
            FoodModelConnection.FoodModel.transform.localPosition = localPosition;
        }
    }

    public void RemoveConnection()
    {
        if (FoodModelConnection != null) 
            FoodModelConnection.DestroyGameObject();
    }

    public void LoadARScene()
    {
        if(Instance.FoodModelConnection != null && Instance.FoodModelConnection.FoodModel != null) { 
            DontDestroyOnLoad(Instance.FoodModelConnection);
            DontDestroyOnLoad(Instance.FoodModelConnection.FoodModel);
        }
        SceneManager.LoadScene("FoodARScene");
        is3DScene = false;
        if(Instance.foodModelConnection != null)
            UiState = UIStates.AutoPlace;
    }

    public void Load3DScene()
    {
        UiState = (int)UIStates.Idle;
        Instance.FoodModelConnection.FoodModel.transform.SetParent(null);
        Destroy(surfacePlane);
        if (Instance.FoodModelConnection != null && Instance.FoodModelConnection.FoodModel != null) { 
            DontDestroyOnLoad(Instance.FoodModelConnection);
            DontDestroyOnLoad(Instance.FoodModelConnection.FoodModel);
        }
        Instance.FoodModelConnection.FoodModel.transform.position = Vector3.zero;
        SceneManager.LoadScene("Food3DScene");
        is3DScene = true;
    }

    public void Reset()
    {
        hasFoodModelBeenPlaced = false;
        RemoveConnection();
        UiState = (int)UIStates.Idle;
    }

}
