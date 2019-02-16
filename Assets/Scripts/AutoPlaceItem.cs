using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;

public class AutoPlaceItem : MonoBehaviour {


    public ItemPlacerConnection ItemPlacedController;
    public ItemPlacerConnectionMain ItemPlacedController2;

    //ARSessionOrigin m_SessionOrigin;

    //static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public LayerMask layerMask;

    public GameObject[] TestingGround;

    public GameObject fixButton;

    public float speed = 3f;

    public bool isPlacing = false;

    public Vector3 lastPlacementPos;



    void Awake()
    {
        if (Application.isEditor)
        {
            for (int i = 0; i < TestingGround.Length; i++)
            {
                TestingGround[i].SetActive(true);
            }

        }else{

            for (int i = 0; i < TestingGround.Length; i++)
            {
                TestingGround[i].SetActive(false);
            }

        }

        //m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    public void GameCode(){
        if (ItemPlacedController != null)
        {
            
            //var angleY = 1000 * Time.deltaTime;
            //Debug.Log("angleY: "+angleY);
            //ItemPlacedController.GetGameObjectToPlace().transform.Rotate(transform.up, 2);
            //Debug.Log("hasItemBeenPlaced: "+ItemPlacedController.hasItemBeenPlaced);
            if (ItemPlacedController.hasItemBeenPlaced == false)
            {

                isPlacing = true;
                ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                ItemPlacedController.GetGameObjectToPlace().transform.parent = null;
                ItemPlacedController.GetGameObjectToPlace().transform.position = lastPlacementPos;
                //ItemPlacedController.GetGameObjectToPlace().transform.position = Vector3.Lerp(ItemPlacedController.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * speed);
                ItemPlacedController.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
                if (!ItemPlacedController.GetGameObjectToPlace().activeSelf)
                {
                    ItemPlacedController.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }

    public void GameCode2(Vector3 newPos)
    {
        if (ItemPlacedController2 != null)
        {
            //Debug.Log("hasItemBeenPlaced: "+ItemPlacedController.hasItemBeenPlaced);
            if (ItemPlacedController2.hasItemBeenPlaced == false)
            {

                isPlacing = true;
                lastPlacementPos = newPos;
                ItemPlacedController2.GetGameObjectToPlace().SetActive(true);
                ItemPlacedController2.GetGameObjectToPlace().transform.parent = null;
                //ItemPlacedController.GetGameObjectToPlace().transform.position = newPos;
                ItemPlacedController2.GetGameObjectToPlace().transform.position = Vector3.Lerp(ItemPlacedController2.GetGameObjectToPlace().transform.position, newPos, Time.deltaTime * speed);
                if (!ItemPlacedController2.GetGameObjectToPlace().activeSelf)
                {
                    ItemPlacedController2.GetGameObjectToPlace().SetActive(true);
                }
            }
        }
    }



    void Update()
    {
        if (ItemPlacedController != null)
        {
              
            GameCode();
            //ItemPlacedController.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);

            if (isPlacing == false && ItemPlacedController.hasItemBeenPlaced == false)
            {
                HideItem();

            }else{

                CheckTouchType();

            }

            isPlacing = false;
        }

        if (ItemPlacedController2 != null)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hit, 500.0f, layerMask))
            {
                GameCode2(hit.point);
                ItemPlacedController2.GetGameObjectToPlace().transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            if (isPlacing == false && ItemPlacedController2.hasItemBeenPlaced == false)
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

        if (ItemPlacedController.hasItemBeenPlaced == false)
        {
            ItemPlacedController.hasItemBeenPlaced = true;
            ItemPlacedController.GetGameObjectToPlace().transform.position = lastPlacementPos;
        }
    }

    public void TapHasOccured2()
    {

        if (ItemPlacedController2.hasItemBeenPlaced == false)
        {
            fixButton.SetActive(false);
            ItemPlacedController2.hasItemBeenPlaced = true;
            ItemPlacedController2.GetGameObjectToPlace().transform.position = lastPlacementPos;
        }
    }

    public void SetNewGameObjectToPlace(ItemPlacerConnection ItemPlacedController){

        ShouldWeHideIt();
        //GameObjectToPlace = newItem;
        this.ItemPlacedController = ItemPlacedController;

    }

    public void SetNewGameObjectToPlace2(ItemPlacerConnectionMain ItemPlacedController2)
    {

        ShouldWeHideIt2();
        //GameObjectToPlace = newItem;
        this.ItemPlacedController2 = ItemPlacedController2;

    }

    public void ShouldWeHideIt(){
        if (ItemPlacedController != null)
        {
                HideItem();
        }

    }

    public void ShouldWeHideIt2()
    {
        if (ItemPlacedController2 != null)
        {
            if (ItemPlacedController2.hasItemBeenPlaced == false)
            {
                HideItem2();
            }
        }

    }

    public void HideItem(){
        if (ItemPlacedController != null)
        {
            ItemPlacedController2.GetGameObjectToPlace().SetActive(false);
            ItemPlacedController.GetGameObjectToPlace().SetActive(false);
            ItemPlacedController.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            ItemPlacedController.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void HideItem2()
    {
        if (ItemPlacedController != null)
        {
            ItemPlacedController2.GetGameObjectToPlace().SetActive(false);
            ItemPlacedController2.GetGameObjectToPlace().transform.parent = Camera.main.transform;
            ItemPlacedController2.GetGameObjectToPlace().transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItemToPlace(){
        ItemPlacedController = null;

    }

    public void RemoveItemToPlace2()
    {
        ItemPlacedController2 = null;

    }

}
