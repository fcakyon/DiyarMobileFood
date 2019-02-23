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
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked()
    {
        VanishFoodPosition();
        MainFoodUIScript.SetFoodPosition(this);
        foodPositionModel.SetActive(false);
        MainFoodUIScript.fixButton.SetActive(true);
    }

    public GameObject GetGameObjectToPlace()
    {
        return foodPositionModel;
    }

    public void VanishFoodPosition()
    {
        hasFoodPositionBeenPlaced = false;
        MainFoodUIScript.HideFoodPosition();
        MainFoodUIScript.RemoveFoodPositionConnection();
    }
}
