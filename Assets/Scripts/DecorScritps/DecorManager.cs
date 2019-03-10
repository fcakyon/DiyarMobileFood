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
    public float foodPositionSpeed = 4f;
    public bool isPlacing = false;
    public Vector3 lastPlacementPos;
    public bool is3DScene;

    private void Start()
    {
        //Application.targetFrameRate = 60;
        if (is3DScene == true) lastPlacementPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        //CheckTouchType();

        //if (foodModelConnection != null)
        //{
        //    FoodModelChange();
        //}

        if (decorModelConnection != null && decorModelConnection.hasDecorModelBeenPlaced != true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hit, 500.0f, planeLayerMask))
            {
                DecorModelPlacement(hit.point);
                decorModelConnection.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            isPlacing = false;
        }
    }

    public void DecorModelPlacement(Vector3 newPos)
    {
        if (decorModelConnection != null)
        {
            if (decorModelConnection.hasDecorModelBeenPlaced == false)
            {
                isPlacing = true;
                lastPlacementPos = newPos;
                decorModelConnection.GetGameObjectToPlace().SetActive(true);
                decorModelConnection.GetGameObjectToPlace().transform.SetParent(null);
                decorModelConnection.GetGameObjectToPlace().transform.SetParent(GameObject.Find("Plane").transform);
                decorModelConnection.GetGameObjectToPlace().transform.position = Vector3.Lerp(decorModelConnection.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * foodPositionSpeed);
                if (!decorModelConnection.GetGameObjectToPlace().activeSelf)
                {
                    decorModelConnection.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
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
        SceneManager.LoadScene("DecorARScene");
    }

    public void LoadDecor3DScene()
    {
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
