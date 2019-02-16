using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;

public class MainFoodUI : MonoBehaviour {
    
    public FoodModelConnection FoodModelConnectionScript;
    public FoodPositionConnection FoodPositionConnectionScript;

    public LayerMask layerMask;

    public GameObject fixButton;

    public float speed = 3f;

    public bool isPlacing = false;

    public Vector3 lastPlacementPos;

    public void GameCode(){
        if (FoodModelConnectionScript != null)
        {
            
            if (FoodModelConnectionScript.hasItemBeenPlaced == false)
            {

                isPlacing = true;
                FoodModelConnectionScript.GetGameObjectToPlace().SetActive(true);
                FoodModelConnectionScript.GetGameObjectToPlace().transform.parent = null;
                FoodModelConnectionScript.GetGameObjectToPlace().transform.position = lastPlacementPos;
                //ItemPlacedController.GetGameObjectToPlace().transform.position = Vector3.Lerp(ItemPlacedController.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * speed);
                FoodModelConnectionScript.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
                if (!FoodModelConnectionScript.GetGameObjectToPlace().activeSelf)
                {
                    FoodModelConnectionScript.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }

    public void GameCode2(Vector3 newPos)
    {
        if (FoodPositionConnectionScript != null)
        {
            //Debug.Log("hasItemBeenPlaced: "+ItemPlacedController.hasItemBeenPlaced);
            if (FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {

                isPlacing = true;
                lastPlacementPos = newPos;
                FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(true);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.parent = null;
                //ItemPlacedController.GetGameObjectToPlace().transform.position = newPos;
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.position = Vector3.Lerp(FoodPositionConnectionScript.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * speed);
                if (!FoodPositionConnectionScript.GetGameObjectToPlace().activeSelf)
                {
                    FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }



    void Update()
    {
        if (FoodModelConnectionScript != null)
        {
              
            GameCode();
            //ItemPlacedController.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);

            if (isPlacing == false && FoodModelConnectionScript.hasItemBeenPlaced == false)
            {
                HideItem();

            }else{

                CheckTouchType();

            }

            isPlacing = false;
        }

        if (FoodPositionConnectionScript != null)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hit, 500.0f, layerMask))
            {
                GameCode2(hit.point);
                FoodPositionConnectionScript.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            if (isPlacing == false && FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {
                HideItem2();

            }
            else
            {

                //CheckTouchType2();

            }

            isPlacing = false;
        }

    }

    public void CheckTouchType()
    {
        
        if (Input.touchCount > 0)
        {
            
            //Debug.Log("1: "+EventSystem.current.IsPointerOverGameObject(0).ToString());
        //Debug.Log("2: " +(EventSystem.current.currentSelectedGameObject != null).ToString());

            if (EventSystem.current.IsPointerOverGameObject(0) ||
            EventSystem.current.currentSelectedGameObject != null)
        {
            //Debug.Log("return etti");
            return;
        }

        //Debug.Log("return etmedi");

            Touch touch = Input.GetTouch(0);
            RaycastHit hit;
            //Debug.Log(touch.position);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            //Debug.Log(Physics.Raycast(ray, out hit, 500.0f, layerMask));
            if (Physics.Raycast(ray, out hit, 500.0f, layerMask))
            {
                TapHasOccured();
            }
        }

    }

    public void CheckTouchType2()
    {

        if (Input.touchCount > 0)
        {

            //Debug.Log("1: " + EventSystem.current.IsPointerOverGameObject(0).ToString());
            //Debug.Log("2: " +(EventSystem.current.currentSelectedGameObject != null).ToString());

            if (EventSystem.current.IsPointerOverGameObject(0) ||
            EventSystem.current.currentSelectedGameObject != null)
            {
                //Debug.Log("return etti");
                return;
            }

            //Debug.Log("return etmedi");

            Touch touch = Input.GetTouch(0);
            RaycastHit hit;
            //Debug.Log(touch.position);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            //Debug.Log(Physics.Raycast(ray, out hit, 500.0f, layerMask));
            if (Physics.Raycast(ray, out hit, 500.0f, layerMask))
            {
                TapHasOccured2();
            }
        }

    }

    public void TapHasOccured()
    {

        if (FoodModelConnectionScript.hasItemBeenPlaced == false)
        {
            FoodModelConnectionScript.hasItemBeenPlaced = true;
            FoodModelConnectionScript.GetGameObjectToPlace().transform.position = lastPlacementPos;
        }
    }

    public void TapHasOccured2()
    {

        if (FoodPositionConnectionScript. hasFoodPositionBeenPlaced == false)
        {
            fixButton.SetActive(false);
            FoodPositionConnectionScript.hasFoodPositionBeenPlaced = true;
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.position = lastPlacementPos;
        }
    }

    public void SetNewGameObjectToPlace(FoodModelConnection FoodModelConnectionScript){

        ShouldWeHideIt();
        //GameObjectToPlace = newItem;
        this.FoodModelConnectionScript = FoodModelConnectionScript;

    }

    public void SetNewGameObjectToPlace2(FoodPositionConnection FoodPositionConnectionScript)
    {

        ShouldWeHideIt2();
        //GameObjectToPlace = newItem;
        this.FoodPositionConnectionScript = FoodPositionConnectionScript;

    }

    public void ShouldWeHideIt(){
        if (FoodModelConnectionScript != null)
        {
                HideItem();
        }

    }

    public void ShouldWeHideIt2()
    {
        if (FoodPositionConnectionScript != null)
        {
            if (FoodPositionConnectionScript.hasFoodPositionBeenPlaced == false)
            {
                HideItem2();
            }
        }

    }

    public void HideItem(){
        if (FoodModelConnectionScript != null)
        {
            FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(false);
            FoodModelConnectionScript.GetGameObjectToPlace().SetActive(false);
            FoodModelConnectionScript.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            FoodModelConnectionScript.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void HideItem2()
    {
        if (FoodModelConnectionScript != null)
        {
            FoodPositionConnectionScript.GetGameObjectToPlace().SetActive(false);
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            FoodPositionConnectionScript.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItemToPlace(){
        FoodModelConnectionScript = null;

    }

    public void RemoveItemToPlace2()
    {
        FoodPositionConnectionScript = null;

    }

}
