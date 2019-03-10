using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager3DScene : MonoBehaviour
{

    public FoodManager foodManager;
    public DecorManager decorManager;
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
        if (foodManager != null)
        {
            if (foodManager.foodModelConnection != null)
            {
                if (foodManager.foodModelConnection.foodModel != null)
                {
                    if (Input.touchCount == 0)
                    {
                        foodManager.foodModelConnection.GetGameObjectToPlace().transform.Rotate(0f, 15 * Time.deltaTime, 0f);
                    }

                    else
                    {
                        // GET TOUCH 0
                        Touch touch0 = Input.GetTouch(0);

                        // APPLY ROTATION

                        if (touch0.phase == TouchPhase.Moved)
                        {
                            var angleY = -0.4f * touch0.deltaPosition.x;
                            foodManager.foodModelConnection.GetGameObjectToPlace().transform.Rotate(0f, angleY, 0f);
                        }
                    }
                }
            }
        }

        if (decorManager != null)
        {
            if (decorManager.decorModelConnection != null)
            {
                if (decorManager.decorModelConnection.decorModel != null)
                {
                    if (Input.touchCount == 0)
                    {
                        decorManager.decorModelConnection.GetGameObjectToPlace().transform.Rotate(0f, 15 * Time.deltaTime, 0f);
                    }

                    else
                    {
                        // GET TOUCH 0
                        Touch touch0 = Input.GetTouch(0);

                        // APPLY ROTATION

                        if (touch0.phase == TouchPhase.Moved)
                        {
                            var angleY = -0.4f * touch0.deltaPosition.x;
                            decorManager.decorModelConnection.GetGameObjectToPlace().transform.Rotate(0f, angleY, 0f);
                        }
                    }
                }
            }
        }
    }
}
