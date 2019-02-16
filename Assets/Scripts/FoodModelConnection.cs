using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodModelConnection : MonoBehaviour
{

    public bool hasItemBeenPlaced = false;
    public GameObject foodModel;
    public MainFoodUI MainFoodUIScript;

    // Use this for initialization
    void Start()
    {
        if (hasItemBeenPlaced == false)
        {

            foodModel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked(){

        if(hasItemBeenPlaced == false){

            if(MainFoodUIScript.FoodModelConnectionScript!=this){
                MainFoodUIScript.SetNewGameObjectToPlace(this);

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
        return foodModel;
    }


    public void PutItemAway(){
        MainFoodUIScript.SetNewGameObjectToPlace(this);
        hasItemBeenPlaced = false;
        MainFoodUIScript.HideItem();
        MainFoodUIScript.RemoveItemToPlace();

    }
}
