using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DecorManager : MonoBehaviour
{
    //[HideInInspector]
    public DecorModelConnection decorModelConnection;
    public LayerMask planeLayerMask;
    public GameObject fixButton;
    public float modelLerpSpeed = 4f;
    public bool isPlacing = false;
    public Vector3 lastPlacementPos;

    GameObject surfacePlane;

    public bool is3DScene;

    private void Start()
    {
        surfacePlane = GameObject.Find("Plane");
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (decorModelConnection != null && decorModelConnection.hasDecorModelBeenPlaced != true)
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

    //public void FixDecorPosition()
    //{
    //    if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    //        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Surface")))
    //        {
    //            Debug.Log("ray zemine degdi");
    //        }
    //        else
    //        {
    //            Debug.Log("ray zemine degmedi");
    //        }
    //    }
    //}

    public void FixDecorModelPlace()
    {
        if (decorModelConnection.hasDecorModelBeenPlaced == false)
        {
            HideFixButton();
            decorModelConnection.hasDecorModelBeenPlaced = true;
            decorModelConnection.GetGameObjectToPlace().transform.position = lastPlacementPos;
            Vector3 localPosition = decorModelConnection.GetGameObjectToPlace().transform.localPosition;
            localPosition.y = 0;
            decorModelConnection.GetGameObjectToPlace().transform.localPosition = localPosition;
        }
    }

    public void SetDecorModel(DecorModelConnection DecorModelConnectionScript)
    {
        ShouldWeHideDecorModel();
        this.decorModelConnection = DecorModelConnectionScript;
    }

    public void ShouldWeHideDecorModel()
    {
        if (decorModelConnection != null)
        {
            if (decorModelConnection.hasDecorModelBeenPlaced == false)
            {
                HideDecorModel();
            }
        }
    }

    public void HideDecorModel()
    {
        if (decorModelConnection != null) decorModelConnection.DestroyDecorModel();
    }

    public void RemoveDecorModelConnection()
    {
        decorModelConnection = null;
    }

    public void LoadDecorARScene()
    {
        HideDecorModel();
        SceneManager.LoadScene("DecorARScene");
    }

    public void LoadDecor3DScene()
    {
        HideDecorModel();
        SceneManager.LoadScene("Decor3DScene");
    }

    public void ApearFixButton()
    {
        fixButton.SetActive(true); ;
    }

    public void HideFixButton()
    {
        fixButton.SetActive(false); ;
    }

}
