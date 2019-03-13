using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTouchManager : MonoBehaviour {

	void Start () {}

	void Update () {
        OneFingerRotate();
    }

    public void OneFingerRotate()
    {
        if (FoodManager.Instance.foodModelConnection != null)
        {
            if (FoodManager.Instance.foodModelConnection.foodModel != null)
            {
                if (Input.touchCount == 0)
                    FoodManager.Instance.foodModelConnection.foodModel.transform.Rotate(0f, 15 * Time.deltaTime, 0f);
                else
                {
                    Touch touch0 = Input.GetTouch(0);
                    if (touch0.phase == TouchPhase.Moved)
                    {
                        var angleY = -0.4f * touch0.deltaPosition.x;
                        FoodManager.Instance.foodModelConnection.foodModel.transform.Rotate(0f, angleY, 0f);
                    }
                }
            }
        }
    }
}
