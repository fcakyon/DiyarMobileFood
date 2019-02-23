using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{

    public MainFoodUI MainFoodUIScript;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OneFingerRotate();
    }

    public void OneFingerRotate()
    {
        if (MainFoodUIScript.FoodModelConnectionScript != null)
        {
            if (MainFoodUIScript.FoodModelConnectionScript.foodModel != null)
            {
            if (Input.touchCount == 0)
            {
                MainFoodUIScript.FoodModelConnectionScript.GetGameObjectToPlace().transform.Rotate(0f, 15 * Time.deltaTime, 0f);
            }

            else
            {
                //Debug.Log("touch geldi");
                // GET TOUCH 0
                Touch touch0 = Input.GetTouch(0);

                // APPLY ROTATION

                if (touch0.phase == TouchPhase.Moved)
                {
                    var angleY = -0.4f * touch0.deltaPosition.x;
                    MainFoodUIScript.FoodModelConnectionScript.GetGameObjectToPlace().transform.Rotate(0f, angleY, 0f);
                }


            }
            }
        }
    }
}
