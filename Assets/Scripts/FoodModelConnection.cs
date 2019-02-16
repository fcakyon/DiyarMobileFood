using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodModelConnection : MonoBehaviour
{

    public bool hasFoodModelBeenChanged = false;
    public GameObject foodModel;
    public MainFoodUI MainFoodUIScript;

    // Use this for initialization
    void Start()
    {
        if (hasFoodModelBeenChanged == false)
        {
            foodModel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked()
    {
        VanishFoodModel();
        MainFoodUIScript.SetFoodModel(this);
    }

    public GameObject GetGameObjectToPlace()
    {
        return foodModel;
    }

    public void VanishFoodModel()
    {
        hasFoodModelBeenChanged = false;
        MainFoodUIScript.ShouldWeHideFoodModel();
        MainFoodUIScript.RemoveFoodModelConnection();

    }
}
