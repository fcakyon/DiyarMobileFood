using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPositionConnection : MonoBehaviour
{
    
    public bool hasFoodPositionBeenPlaced = false;
    public GameObject foodPositionModel;
    public MainFoodUI MainFoodUIScript;

    // Use this for initialization
    void Start()
    {
        if (hasFoodPositionBeenPlaced == false)
        {
            ButtonClicked();
            foodPositionModel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked()
    {
        PutItemAway2();
        if (MainFoodUIScript.FoodPositionConnectionScript != this)
        {
            MainFoodUIScript.SetNewGameObjectToPlace2(this);
        }

    }

    public void ButtonClicked2()
    {
        ButtonClicked();
        foodPositionModel.SetActive(false);
        MainFoodUIScript.fixButton.SetActive(true);
    }

    public GameObject GetGameObjectToPlace()
    {
        return foodPositionModel;
    }

    public void PutItemAway2()
    {
        MainFoodUIScript.SetNewGameObjectToPlace2(this);
        hasFoodPositionBeenPlaced = false;
        MainFoodUIScript.HideItem2();
        MainFoodUIScript.RemoveItemToPlace2();

    }
}
