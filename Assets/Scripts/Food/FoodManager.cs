using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }
    [HideInInspector]
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
    private XRSurfaceController xRSurfaceController;
    public bool hasConnectionAndModel;

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
        if (is3DScene == true) lastPlacementPos = Vector3.zero;
        AnimManager.Instance.Full2None();
    }


    void Update()
    {
        if (FoodModelConnection != null && FoodModelConnection.FoodModel != null && hasFoodModelBeenPlaced != true)
        {
            AutoPlaceModel();
        }
        else if(isChanging == true)
        {
            ChangeFoodModel();
        }
    }

    public FoodModelConnection FoodModelConnection
    {
        get { return foodModelConnection; }
        set
        {
            if (value != null)
            {
                FoodModelConnection foodModelConnectionGameObject = Instantiate(value);
                foodModelConnection = foodModelConnectionGameObject;
            }
        }
    }

    public UIStates UiState
    {
        get { return uiState; }
        set
        {
            uiState = value;
            OnUIStateChange.Invoke();
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
        if (!is3DScene)
        {
            if (surfacePlane == null) surfacePlane = GameObject.Find("Plane");
            FoodModelConnection.transform.SetParent(surfacePlane.transform);
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
           UiState = (int)UIStates.Idle;
        else
        {
            if(hasFoodModelBeenPlaced)
                UiState = UIStates.Fixed;
            else
                UiState = UIStates.AutoPlace;
        }
    }

    public void Fix()
    {
        if (hasFoodModelBeenPlaced == false)
        {
            UiState = UIStates.Fixed;
            hasFoodModelBeenPlaced = true;
            FoodModelConnection.FoodModel.transform.position = lastPlacementPos;
            Vector3 localPosition = FoodModelConnection.FoodModel.transform.localPosition;
            localPosition.y = 0;
            FoodModelConnection.FoodModel.transform.localPosition = localPosition;
            xRSurfaceController.shouldSurfaceBeUpdated = false;
        }
    }

    public void RemoveConnection()
    {
        if (FoodModelConnection != null) 
            FoodModelConnection.DestroyGameObject();
    }

    public void ToogleScene()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        hasConnectionAndModel = FoodModelConnection != null && FoodModelConnection.FoodModel != null;
        if (is3DScene)
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(FoodModelConnection);
                DontDestroyOnLoad(FoodModelConnection.FoodModel);
            }
            yield return StartCoroutine(AnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("FoodARScene");
            if (hasConnectionAndModel) {
                FoodModelConnection.FoodModel.transform.localScale = new Vector3(0, 0, 0);
                UiState = UIStates.AutoPlace;
            } else UiState = UIStates.Idle;
            AnimManager.Instance.Full2Border();
            AnimManager.Instance.CircularPlane = GameObject.Find("Plane/CircularPlane");
            is3DScene = false;
            xRSurfaceController = GameObject.Find("Plane").GetComponent<XRSurfaceController>();
            xRSurfaceController.shouldSurfaceBeUpdated = true;
        }
        else
        {
            if (hasConnectionAndModel)
            {
                FoodModelConnection.transform.SetParent(null);
                FoodModelConnection.FoodModel.transform.SetParent(null);
                DontDestroyOnLoad(FoodModelConnection);
                DontDestroyOnLoad(FoodModelConnection.FoodModel);
            }
            yield return StartCoroutine(AnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("Food3DScene");
            AnimManager.Instance.Full2None();
            Destroy(surfacePlane);
            if (hasConnectionAndModel)
            {
                FoodModelConnection.SetModelScale();
                FoodModelConnection.FoodModel.transform.SetParent(null);
                FoodModelConnection.FoodModel.transform.position = Vector3.zero;
            }
            is3DScene = true;
            UiState = UIStates.Idle;
        }
    }

    public void Reset()
    {
        hasFoodModelBeenPlaced = false;
        RemoveConnection();
        xRSurfaceController.shouldSurfaceBeUpdated = true;
        UiState = (int)UIStates.Idle;
    }

}
