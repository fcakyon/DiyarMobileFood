using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class DecorManager : MonoBehaviour
{
    public static DecorManager Instance { get; private set; }
    [HideInInspector]
    private DecorModelConnection decorModelConnection;
    public LayerMask planeLayerMask;
    public float modelLerpSpeed = 4f;
    public bool isPlacing;
    public Vector3 lastPlacementPos;
    GameObject surfacePlane;
    public Dictionary<GameObject, DecorModelConnection> allModelsDict = new Dictionary<GameObject, DecorModelConnection>();
    public bool is3DScene;
    public enum UIStates { Idle, Loading, AutoPlace };
    private UIStates uiState; // Set to Idle Initially
    [HideInInspector]
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (is3DScene) lastPlacementPos = new Vector3(0, 0, 0);
        DecorAnimManager.Instance.Full2None();
    }

    void Update()
    {
        if (DecorModelConnection != null && DecorModelConnection.DecorModel != null && !DecorModelConnection.hasModelBeenPlaced)
        {
            AutoPlaceModel();
        }
    }


    public DecorModelConnection DecorModelConnection
    {
        get { return decorModelConnection; }
        set
        {
            if (value != null)
            {
                DecorModelConnection decorModelConnectionGameObject = Instantiate(value);
                decorModelConnection = decorModelConnectionGameObject;
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
            PlaceDecorModel(hit.point);
            DecorModelConnection.DecorModel.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void PlaceDecorModel(Vector3 newPos)
    {
        lastPlacementPos = newPos;
        DecorModelConnection.DecorModel.SetActive(true);
        DecorModelConnection.DecorModel.transform.SetParent(null);
        DecorModelConnection.DecorModel.transform.position = Vector3.Lerp(DecorModelConnection.DecorModel.transform.position, newPos, Time.deltaTime * modelLerpSpeed);
        if (!is3DScene)
        {
            if (surfacePlane == null) surfacePlane = GameObject.Find("Plane");
            DecorModelConnection.DecorModel.transform.SetParent(surfacePlane.transform);
            DecorModelConnection.transform.SetParent(surfacePlane.transform);
        }
    }

    public void ChangeStateAfterLoading()
    {
        if (is3DScene)
            UiState = UIStates.Idle;
        else
            UiState = UIStates.AutoPlace;
    }

    public void Fix()
    {
        if (!DecorModelConnection.hasModelBeenPlaced)
        {
            UiState = UIStates.Idle;
            DecorModelConnection.hasModelBeenPlaced = true;
            DecorModelConnection.DecorModel.transform.position = lastPlacementPos;
            Vector3 localPosition = DecorModelConnection.DecorModel.transform.localPosition;
            localPosition.y = 0;
            DecorModelConnection.DecorModel.transform.localPosition = localPosition;
            xRSurfaceController.shouldSurfaceBeUpdated = false;
        }
    }

    public void SetDecorModelConnectionUsingModel(GameObject decorModel)
    {
        if (DecorModelConnection.DecorModel != decorModel)
            decorModelConnection = allModelsDict[decorModel];
    }

    public void Delete()
    {
        UiState = UIStates.Idle;
        RemoveConnection();
    }

    public void RemoveConnection()
    {
        if (DecorModelConnection != null)
         DecorModelConnection.DestroyDecorModel();
    }

    public void AddModelToDict(GameObject decorModel, DecorModelConnection decorModelConnection)
    {
        allModelsDict.Add(decorModel, decorModelConnection);
    }

    public void ToggleScene() {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        hasConnectionAndModel = DecorModelConnection != null && DecorModelConnection.DecorModel != null;
        if (is3DScene)
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(DecorModelConnection);
                DontDestroyOnLoad(DecorModelConnection.DecorModel);
            }

            // hide canvas before starting scene transition animation
            GameObject canvas = GameObject.Find("Canvas").gameObject;
            canvas.SetActive(false);

            // hide current active animations
            DecorAnimManager.Instance.OnARSceneWillLoad();

            // start scene transition animation
            yield return StartCoroutine(DecorAnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("DecorARScene");

            // Setting model invisible after scene change till surface is found
            if (hasConnectionAndModel)
            {
                DecorModelConnection.DecorModel.transform.localScale = new Vector3(0, 0, 0);
                DecorModelConnection.hasModelBeenPlaced = false;
            }
            DecorAnimManager.Instance.OnARSceneDidLoad();
            is3DScene = false;
            xRSurfaceController = GameObject.Find("Plane").GetComponent<XRSurfaceController>();
            if (DecorModelConnection != null) UiState = UIStates.AutoPlace;
            else UiState = UIStates.Idle;
            xRSurfaceController.shouldSurfaceBeUpdated = true;
        }
        else
        {
            if (hasConnectionAndModel)
            {
                DecorModelConnection.transform.SetParent(null);
                DecorModelConnection.DecorModel.transform.SetParent(null);
                DontDestroyOnLoad(DecorModelConnection);
                DontDestroyOnLoad(DecorModelConnection.DecorModel);
            }

            // hide canvas before starting scene transition animation
            GameObject canvas = GameObject.Find("Canvas").gameObject;
            canvas.SetActive(false);

            yield return StartCoroutine(DecorAnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("Decor3DScene");

            // show canvas after scene is loaded
            canvas = GameObject.Find("Canvas").gameObject;
            canvas.SetActive(true);

            DecorAnimManager.Instance.On3DSceneDidLoad();
            is3DScene = true;
            Destroy(surfacePlane);
            if (hasConnectionAndModel)
            {
                // reset model scale for 3d scene
                float modelScale = 1f;
                DecorModelConnection.SetModelScale(modelScale);

                // remove model and connection parent
                DecorModelConnection.transform.SetParent(null);
                DecorModelConnection.DecorModel.transform.SetParent(null);

                // reset model position for 3d scene
                DecorModelConnection.DecorModel.transform.position = Vector3.zero;
            }
            UiState = UIStates.Idle;
        }
    }

    public void Reset()
    {
        foreach (GameObject key in allModelsDict.Keys.ToList())
        {
            allModelsDict[key].DestroyDecorModel();
        }
        allModelsDict.Clear();
        xRSurfaceController.shouldSurfaceBeUpdated = true;
        UiState = UIStates.Idle;
    }

}
