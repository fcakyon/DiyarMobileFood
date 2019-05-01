﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class DecorManager : MonoBehaviour
{
    public static DecorManager Instance { get; private set; }
    [HideInInspector]
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
    [HideInInspector]
    public UnityEvent OnUIStateChange = new UnityEvent();
    private XRSurfaceController xRSurfaceController;

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
        if (!DecorModelConnection.hasDecorModelBeenPlaced)
        {
            UiState = UIStates.Idle;
            DecorModelConnection.hasDecorModelBeenPlaced = true;
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
        bool hasConnectionAndModel = DecorModelConnection != null && DecorModelConnection.DecorModel != null;
        if (is3DScene)
        {
            if (hasConnectionAndModel)
            {
                DontDestroyOnLoad(DecorModelConnection);
                DontDestroyOnLoad(DecorModelConnection.DecorModel);
            }
            yield return StartCoroutine(AnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("DecorARScene");
            AnimManager.Instance.Full2Border();
            AnimManager.Instance.CircularPlane = GameObject.Find("Plane/CircularPlane");
            is3DScene = false;
            xRSurfaceController = GameObject.Find("Plane").GetComponent<XRSurfaceController>();
            if (hasConnectionAndModel)
            {
                DecorModelConnection.hasDecorModelBeenPlaced = false;
                DecorModelConnection.SetModelScale();
            }
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
            yield return StartCoroutine(AnimManager.Instance.None2FullCoroutine());
            yield return SceneManager.LoadSceneAsync("Decor3DScene");
            AnimManager.Instance.Full2None();
            is3DScene = true;
            Destroy(surfacePlane);
            if (hasConnectionAndModel)
            {
                DecorModelConnection.SetModelScale();
                DecorModelConnection.transform.SetParent(null);
                DecorModelConnection.DecorModel.transform.SetParent(null);
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
