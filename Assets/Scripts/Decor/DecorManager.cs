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

    //[HideInInspector]
    public DecorModelConnection decorModelConnection;
    public LayerMask planeLayerMask;
    public float modelLerpSpeed = 4f;
    public bool isPlacing;
    public Vector3 lastPlacementPos;
    GameObject surfacePlane;
    public Dictionary<GameObject, DecorModelConnection> allModelsDict = new Dictionary<GameObject, DecorModelConnection>();
    public bool is3DScene;
    public enum UIStates { Idle, Loading, AutoPlace };
    private UIStates uiState; // Set to Idle Initially
    public UnityEvent OnUIStateChange = new UnityEvent();

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
        if (is3DScene) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        Debug.Log("State: " + uiState);
        if (DecorModelConnection != null && DecorModelConnection.DecorModel != null && DecorModelConnection.hasDecorModelBeenPlaced != true)
        {
            AutoPlaceModel();
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
        if (!is3DScene)
        {
            if (surfacePlane == null) surfacePlane = GameObject.Find("Plane");
            DecorModelConnection.DecorModel.transform.SetParent(surfacePlane.transform);
            DecorModelConnection.transform.SetParent(surfacePlane.transform);
        }
        DecorModelConnection.DecorModel.transform.position = Vector3.Lerp(DecorModelConnection.DecorModel.transform.position, newPos, Time.deltaTime * modelLerpSpeed);
    }

    public void FixDecorModelPlace()
    {
        if (!Instance.DecorModelConnection.hasDecorModelBeenPlaced)
        {
            UiState = UIStates.Idle;
            Instance.DecorModelConnection.hasDecorModelBeenPlaced = true;
            Instance.DecorModelConnection.DecorModel.transform.position = lastPlacementPos;
            Vector3 localPosition = Instance.DecorModelConnection.DecorModel.transform.localPosition;
            localPosition.y = 0;
            Instance.DecorModelConnection.DecorModel.transform.localPosition = localPosition;
        }
    }

    public void ChangeStateAfterLoading()
    {
        if (is3DScene)
            Instance.UiState = UIStates.Idle;
        else
            Instance.UiState = UIStates.AutoPlace;
    }

    public void SetDecorModelConnectionUsingModel(GameObject decorModel)
    {
        if (Instance.DecorModelConnection.DecorModel != decorModel)
            Instance.decorModelConnection = allModelsDict[decorModel];
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
        UiState = UIStates.Loading;
        if (is3DScene)
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(Instance.DecorModelConnection);
                DontDestroyOnLoad(Instance.DecorModelConnection.DecorModel);
            }
            yield return SceneManager.LoadSceneAsync("DecorARScene");
            if (hasConnectionAndModel)
            {
                Instance.DecorModelConnection.hasDecorModelBeenPlaced = false;
                DecorModelConnection.SetModelScale();
            }
            is3DScene = false;
            if (Instance.DecorModelConnection != null) UiState = UIStates.AutoPlace;
            else UiState = UIStates.Idle;
        }
        else
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(Instance.DecorModelConnection);
                DontDestroyOnLoad(Instance.DecorModelConnection.DecorModel);
            }
            yield return SceneManager.LoadSceneAsync("Decor3DScene");
            Destroy(surfacePlane);
            if (hasConnectionAndModel)
            {
                DecorModelConnection.SetModelScale();
                Instance.DecorModelConnection.transform.SetParent(null);
                Instance.DecorModelConnection.DecorModel.transform.SetParent(null);
                Instance.DecorModelConnection.DecorModel.transform.position = Vector3.zero;
            }
            is3DScene = true;
            UiState = UIStates.Idle;
        }
    }

}
