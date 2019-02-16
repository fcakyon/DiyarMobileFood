using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacerConnectionMain : MonoBehaviour
{

    public bool hasItemBeenPlaced = false;
    public GameObject ItemToSetIntoPlacer;
    public AutoPlaceItem PlacerScript;

    // Use this for initialization
    void Start()
    {
        if (hasItemBeenPlaced == false)
        {
            ButtonClicked();
            ItemToSetIntoPlacer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked()
    {

        if (hasItemBeenPlaced == false)
        {

            if (PlacerScript.ItemPlacedController2 != this)
            {
                PlacerScript.SetNewGameObjectToPlace2(this);

            }
            else
            {

                PutItemAway2();
            }


        }
        else
        {
            PutItemAway2();
        }

    }

    public void ButtonClicked2()
    {

        PutItemAway2();
        ButtonClicked();
        ItemToSetIntoPlacer.SetActive(false);
        PlacerScript.fixButton.SetActive(true);

    }

    public GameObject GetGameObjectToPlace()
    {
        return ItemToSetIntoPlacer;
    }

    public void PutItemAway2()
    {
        PlacerScript.SetNewGameObjectToPlace2(this);
        hasItemBeenPlaced = false;
        PlacerScript.HideItem2();
        PlacerScript.RemoveItemToPlace2();

    }
}
