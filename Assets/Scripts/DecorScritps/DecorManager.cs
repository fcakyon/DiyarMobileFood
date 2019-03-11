using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DecorManager : MonoBehaviour
{
    //[HideInInspector]
    public DecorModelConnection decorModelConnection;
    public LayerMask planeLayerMask;

    public GameObject loadingCircle;
    public GameObject toggleButton;
    public GameObject addButton;
    public GameObject fixButton;
    public GameObject deleteButton;
    public GameObject resetButton;

    public float modelLerpSpeed = 4f;
    public bool isPlacing = false;
    public Vector3 lastPlacementPos;

    public UnityEvent onLoadingStarted;
    public UnityEvent onLoadingFinished;
    GameObject surfacePlane;
    public Dictionary<GameObject, DecorModelConnection> allModelsDict = new Dictionary<GameObject, DecorModelConnection>();

    public bool is3DScene;

    private void OnEnable()
    {
        if (onLoadingStarted == null) onLoadingStarted = new UnityEvent();
        if (onLoadingFinished == null) onLoadingFinished = new UnityEvent();
    }

    private void Start()
    {
        surfacePlane = GameObject.Find("Plane");
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (decorModelConnection != null && decorModelConnection.decorModel != null && decorModelConnection.hasDecorModelBeenPlaced != true)
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
            PlaceFoodModel(hit.point);
            decorModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void PlaceFoodModel(Vector3 newPos)
    {
        lastPlacementPos = newPos;
        decorModelConnection.GetGameObjectToPlace().SetActive(true);
        decorModelConnection.GetGameObjectToPlace().transform.SetParent(null);
        if (is3DScene == false)
        {
            decorModelConnection.GetGameObjectToPlace().transform.SetParent(surfacePlane.transform);
        }
        decorModelConnection.GetGameObjectToPlace().transform.position = Vector3.Lerp(decorModelConnection.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * modelLerpSpeed);
    }

    public void FixDecorModelPlace()
    {
        if (decorModelConnection.hasDecorModelBeenPlaced == false)
        {
            UIIdleState();
            decorModelConnection.hasDecorModelBeenPlaced = true;
            decorModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
            Vector3 localPosition = decorModelConnection.GetGameObjectToPlace().transform.localPosition;
            localPosition.y = 0;
            decorModelConnection.GetGameObjectToPlace().transform.localPosition = localPosition;
        }
    }

    public void SetDecorModelConnection(DecorModelConnection newDecorModelConnection)
    {
        this.decorModelConnection = newDecorModelConnection;
    }

    public void SetDecorModelConnectionUsingModel(GameObject decorModel)
    {
        DecorModelConnection newDecorModelConnection = allModelsDict[decorModel];
        SetDecorModelConnection(newDecorModelConnection);
    }

    public void DestroyDecorModel()
    {
        if (decorModelConnection != null) decorModelConnection.DestroyDecorModel();
        RemoveDecorModelConnection();
    }

    public void RemoveDecorModelConnection()
    {
        decorModelConnection = null;
    }

    public void AddModelToDict(GameObject decorModel, DecorModelConnection decorModelConnection)
    {
        allModelsDict.Add(decorModel, decorModelConnection);
    }

    public void LoadDecorARScene()
    {
        DestroyDecorModel();
        SceneManager.LoadScene("DecorARScene");
    }

    public void LoadDecor3DScene()
    {
        DestroyDecorModel();
        SceneManager.LoadScene("Decor3DScene");
    }

    public void UILoadingState()
    {
        loadingCircle.SetActive(true);
        toggleButton.SetActive(false);
        addButton.SetActive(false);

        if (is3DScene == false)
        {
            fixButton.SetActive(false);
            deleteButton.SetActive(false);
        }
    }

    public void UIIdleState()
    {
        loadingCircle.SetActive(false);
        toggleButton.SetActive(true);
        addButton.SetActive(true);

        if (is3DScene == false)
        {
            fixButton.SetActive(false);
            deleteButton.SetActive(true);
        }
    }

    public void UIAutoPlaceState()
    {
        if (is3DScene == false)
        {
            loadingCircle.SetActive(false);
            toggleButton.SetActive(false);
            addButton.SetActive(false);
            fixButton.SetActive(true);
            deleteButton.SetActive(true);
        }
    }
}
