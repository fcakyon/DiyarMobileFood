using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor3DTouchManager : MonoBehaviour {

	void Start () {}
	
	void Update () {
        OneFingerRotate();
	}

    public void OneFingerRotate()
    {
        if (DecorManager.Instance.DecorModelConnection != null)
        {
            if (DecorManager.Instance.DecorModelConnection.DecorModel != null)
            {
                if (Input.touchCount == 0)
                    DecorManager.Instance.DecorModelConnection.DecorModel.transform.Rotate(0f, 15 * Time.deltaTime, 0f);


                else
                {
                    Touch touch0 = Input.GetTouch(0);
                    if (touch0.phase == TouchPhase.Moved)
                    {
                        var angleY = -0.4f * touch0.deltaPosition.x;
                        DecorManager.Instance.DecorModelConnection.DecorModel.transform.Rotate(0f, angleY, 0f);
                    }
                }
            }
        }
       
    }
}
