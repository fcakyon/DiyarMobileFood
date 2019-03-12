using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPositionConnection : MonoBehaviour
{
    
    public bool hasFoodPositionBeenPlaced = false;
    public GameObject foodPositionModel;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {}

    public GameObject GetGameObjectToPlace()
    {
        return foodPositionModel;
    }
}
