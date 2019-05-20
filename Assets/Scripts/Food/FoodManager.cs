using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }
    [HideInInspector]
    private FoodModelConnection foodModelConnection;
    public LayerMask planeLayerMask;
    public float modelLerpSpeed = 4f;
    public bool isPlacing;
    public bool isChanging;
    public Vector3 lastPlacementPos;
    GameObject surfacePlane;
    public bool is3DScene;
    public enum UIStates { Idle, Loading, AutoPlace, Fixed};
    private UIStates uiState; // Set to Idle Initially
    public UnityEvent OnUIStateChange = new UnityEvent();
    private XRSurfaceController xRSurfaceController;
    public bool hasConnectionAndModel;
    public bool hasSurfaceFound;

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
        FoodAnimManager.Instance.Full2None();
    }


    void Update()
    {
        if (FoodModelConnection != null && FoodModelConnection.FoodModel != null && !FoodModelConnection.hasModelBeenPlaced)
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
            if(FoodModelConnection.hasModelBeenPlaced)
                UiState = UIStates.Fixed;
            else
                UiState = UIStates.AutoPlace;
        }
    }

    public void Fix()
    {
        if (!FoodModelConnection.hasModelBeenPlaced)
        {
            UiState = UIStates.Fixed;
            FoodModelConnection.hasModelBeenPlaced = true;
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
        if (is3DScene) // 3d to ar scene transition
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(FoodModelConnection);
                DontDestroyOnLoad(FoodModelConnection.FoodModel);
            }

            // hide canvas before starting scene transition animation
            GameObject canvas = GameObject.Find("Canvas").gameObject;
            canvas.SetActive(false);

            // hide current active animations
            FoodAnimManager.Instance.OnARSceneWillLoad();

            // start scene transition animation
            yield return StartCoroutine(FoodAnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("FoodARScene");

            // Setting model invisible after scene change till surface is found
            if (hasConnectionAndModel)
            {
                FoodModelConnection.FoodModel.transform.localScale = new Vector3(0, 0, 0);
                FoodModelConnection.hasModelBeenPlaced = false;
            }

            FoodAnimManager.Instance.OnARSceneDidLoad();
            is3DScene = false;
            xRSurfaceController = GameObject.Find("Plane").GetComponent<XRSurfaceController>();
            if (FoodModelConnection != null) UiState = UIStates.AutoPlace;
            else UiState = UIStates.Idle;
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

            // hide canvas before starting scene transition animation
            GameObject canvas = GameObject.Find("Canvas").gameObject;
            canvas.SetActive(false);

            yield return StartCoroutine(FoodAnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("Food3DScene");

            // show canvas after scene is loaded
            canvas = GameObject.Find("Canvas").gameObject;
            canvas.SetActive(true);

            // reset last model position for 3d scene
            lastPlacementPos = Vector3.zero;

            FoodAnimManager.Instance.On3DSceneDidLoad();
            Destroy(surfacePlane);
            if (hasConnectionAndModel)
            {
                // reset model scale for 3d scene
                float modelScale = 1f;
                FoodModelConnection.SetModelScale(modelScale);

                // remove model parent
                FoodModelConnection.FoodModel.transform.SetParent(null);

                // reset model position for 3d scene
                FoodModelConnection.FoodModel.transform.position = Vector3.zero;
            }
            is3DScene = true;
            UiState = UIStates.Idle;
        }
    }

    public void Reset()
    {
        FoodModelConnection.hasModelBeenPlaced = false;
        RemoveConnection();
        xRSurfaceController.shouldSurfaceBeUpdated = true;
        UiState = (int)UIStates.Idle;
    }

}
