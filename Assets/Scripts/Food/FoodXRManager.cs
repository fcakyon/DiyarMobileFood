using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        FoodAnimManager.Instance.Border2None();
        FoodAnimManager.Instance.CircularPlaneAnim();
        FoodManager.Instance.hasSurfaceFound = true;
        // Setting model visible after surface has found
        if (FoodManager.Instance.hasConnectionAndModel)
        {
            float modelScale = 1f;
            FoodManager.Instance.FoodModelConnection.SetModelScale(modelScale);
        }
        else
        {
            FoodAnimManager.Instance.DummyAdd();
        }
        canvas.SetActive(true);
    }

    public void OnSurfaceSwitch()
    {
        FoodAnimManager.Instance.CircularPlaneAnim();
    }
}
