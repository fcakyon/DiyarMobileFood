using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class DecorManager : MonoBehaviour
{
    public static DecorManager Instance { get; private set; }

    //[HideInInspector]
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
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        Debug.Log("State: " + uiState);
        if (DecorModelConnection != null && DecorModelConnection.decorModel != null && DecorModelConnection.hasDecorModelBeenPlaced != true)
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
            DecorModelConnection.decorModel.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void PlaceDecorModel(Vector3 newPos)
    {
        lastPlacementPos = newPos;
        DecorModelConnection.decorModel.SetActive(true);
        DecorModelConnection.decorModel.transform.SetParent(null);
        if (is3DScene == false)
        {
            if (surfacePlane == null) surfacePlane = GameObject.Find("Plane");
            DecorModelConnection.decorModel.transform.SetParent(surfacePlane.transform);
        }
        DecorModelConnection.decorModel.transform.position = Vector3.Lerp(DecorModelConnection.decorModel.transform.position, newPos, Time.deltaTime * modelLerpSpeed);
    }

    public void FixDecorModelPlace()
    {
        if (DecorModelConnection.hasDecorModelBeenPlaced == false)
        {
            UiState = (int)UIStates.Idle;
            DecorModelConnection.hasDecorModelBeenPlaced = true;
            DecorModelConnection.decorModel.transform.position = lastPlacementPos;
            Vector3 localPosition = DecorModelConnection.decorModel.transform.localPosition;
            localPosition.y = 0;
            DecorModelConnection.decorModel.transform.localPosition = localPosition;
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
        DecorModelConnection newDecorModelConnection = allModelsDict[decorModel];
        DecorModelConnection = newDecorModelConnection;
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

    public void LoadARScene()
    {
        if (Instance.DecorModelConnection != null && Instance.DecorModelConnection.decorModel != null) { 
            DontDestroyOnLoad(Instance.DecorModelConnection);
            DontDestroyOnLoad(Instance.DecorModelConnection.decorModel);
        }
        SceneManager.LoadScene("DecorARScene");
        is3DScene = false;
        if (Instance.decorModelConnection != null)
            UiState = UIStates.AutoPlace;
    }

    public void Load3DScene()
    {
        UiState = (int)UIStates.Idle;
        Instance.DecorModelConnection.decorModel.transform.SetParent(null);
        Destroy(surfacePlane);
        if (Instance.DecorModelConnection != null && Instance.DecorModelConnection.decorModel != null)
        {
            DontDestroyOnLoad(Instance.DecorModelConnection);
            DontDestroyOnLoad(Instance.DecorModelConnection.decorModel);
        }
        Instance.DecorModelConnection.decorModel.transform.position = Vector3.zero;
        SceneManager.LoadScene("Decor3DScene");
        is3DScene = true;
    }

}
