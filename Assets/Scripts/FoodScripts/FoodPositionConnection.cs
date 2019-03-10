using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPositionConnection : MonoBehaviour
{
    
    public bool hasFoodPositionBeenPlaced = false;
    public GameObject foodPositionModel;
    public FoodManager foodManager;

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
        //VanishFoodPosition();
        //foodManager.SetFoodPosition(this);
        //foodPositionModel.SetActive(false);
        foodManager.fixButton.SetActive(true);
    }

    public GameObject GetGameObjectToPlace()
    {
        return foodPositionModel;
    }
}
