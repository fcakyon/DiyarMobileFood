using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class DecorManager : MonoBehaviour
{
    public static DecorManager Instance { get; private set; }
    [HideInInspector]
    public DecorModelConnection decorModelConnection;
    public LayerMask planeLayerMask;
    public float modelLerpSpeed = 4f;
    public bool isPlacing;
    public Vector3 lastPlacementPos;
    public bool shouldSurfaceBeUpdated = true;
    GameObject surfacePlane;
    public Dictionary<GameObject, DecorModelConnection> allModelsDict = new Dictionary<GameObject, DecorModelConnection>();
    public bool is3DScene;
    public enum UIStates { Idle, Loading, AutoPlace };
    private UIStates uiState; // Set to Idle Initially
    [HideInInspector]
    public UnityEvent OnUIStateChange = new UnityEvent();

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

        AnimManager.Instance.Full2None();
    }

    void Update()
    {
        if (DecorModelConnection != null && DecorModelConnection.DecorModel != null && DecorModelConnection.hasDecorModelBeenPlaced != true)
        {
            AutoPlaceModel();
        }
    }

    public DecorModelConnection DecorModelConnection
    {
        get { return Instance.decorModelConnection; }
        set
        {
            if (value != null)
            {
                DecorModelConnection decorModelConnectionGameObject = Instantiate(value);
                Instance.decorModelConnection = decorModelConnectionGameObject;
            }
        }
    }

    public UIStates UiState
    {
        get { return Instance.uiState; }
        set
        {
            Instance.uiState = value;
            Instance.OnUIStateChange.Invoke();
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
            Instance.UiState = UIStates.Idle;
        else
            Instance.UiState = UIStates.AutoPlace;
    }

    public void Fix()
    {
        if (!Instance.DecorModelConnection.hasDecorModelBeenPlaced)
        {
            UiState = UIStates.Idle;
            Instance.DecorModelConnection.hasDecorModelBeenPlaced = true;
            Instance.DecorModelConnection.DecorModel.transform.position = lastPlacementPos;
            Vector3 localPosition = Instance.DecorModelConnection.DecorModel.transform.localPosition;
            localPosition.y = 0;
            Instance.DecorModelConnection.DecorModel.transform.localPosition = localPosition;
            shouldSurfaceBeUpdated = false;
        }
    }

    public void SetDecorModelConnectionUsingModel(GameObject decorModel)
    {
        if (Instance.DecorModelConnection.DecorModel != decorModel)
            Instance.decorModelConnection = allModelsDict[decorModel];
    }

    public void Delete()
    {
        Instance.UiState = UIStates.Idle;
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
        bool hasConnectionAndModel = Instance.DecorModelConnection != null && Instance.DecorModelConnection.DecorModel != null;
        if (is3DScene)
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(Instance.DecorModelConnection);
                DontDestroyOnLoad(Instance.DecorModelConnection.DecorModel);
            }
            yield return StartCoroutine(AnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("DecorARScene");
            AnimManager.Instance.Full2Border();
            //xrController = GameObject.Find("XRController").GetComponent<XRController>();
            is3DScene = false;
            if (hasConnectionAndModel)
            {
                Instance.DecorModelConnection.hasDecorModelBeenPlaced = false;
                DecorModelConnection.SetModelScale();
            }
            if (Instance.DecorModelConnection != null) UiState = UIStates.AutoPlace;
            else UiState = UIStates.Idle;
            shouldSurfaceBeUpdated = true;
        }
        else
        {
            if (hasConnectionAndModel)
            {
                Instance.DecorModelConnection.transform.SetParent(null);
                Instance.DecorModelConnection.DecorModel.transform.SetParent(null);
                DontDestroyOnLoad(Instance.DecorModelConnection);
                DontDestroyOnLoad(Instance.DecorModelConnection.DecorModel);
            }
            yield return StartCoroutine(AnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("Decor3DScene");
            AnimManager.Instance.Full2None();
            is3DScene = true;
            Destroy(surfacePlane);
            if (hasConnectionAndModel)
            {
                DecorModelConnection.SetModelScale();
                Instance.DecorModelConnection.transform.SetParent(null);
                Instance.DecorModelConnection.DecorModel.transform.SetParent(null);
                Instance.DecorModelConnection.DecorModel.transform.position = Vector3.zero;
            }
            UiState = UIStates.Idle;
        }
    }

    public void Reset()
    {
        foreach (var connection in allModelsDict.Values)
        {
            connection.DestroyDecorModel();
        }
        allModelsDict.Clear();
        shouldSurfaceBeUpdated = true;
        Instance.UiState = UIStates.Idle;
    }
}
