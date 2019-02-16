using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacerConnection : MonoBehaviour
{

    public bool hasItemBeenPlaced = false;
    public GameObject ItemToSetIntoPlacer;
    public AutoPlaceItem PlacerScript;

    // Use this for initialization
    void Start()
    {
        if (hasItemBeenPlaced == false)
        {

            ItemToSetIntoPlacer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked(){

        if(hasItemBeenPlaced == false){

            if(PlacerScript.ItemPlacedController!=this){
                PlacerScript.SetNewGameObjectToPlace(this);

            }else{

                PutItemAway();
            }


        }else{
            PutItemAway();
        }


    }

    public void ButtonClicked2()
    {

        PutItemAway();
        ButtonClicked();

    }

    public GameObject GetGameObjectToPlace(){
        return ItemToSetIntoPlacer;
    }


    public void PutItemAway(){
        PlacerScript.SetNewGameObjectToPlace(this);
        hasItemBeenPlaced = false;
        PlacerScript.HideItem();
        PlacerScript.RemoveItemToPlace();

    }
}
