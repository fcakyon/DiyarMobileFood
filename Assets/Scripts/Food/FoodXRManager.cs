using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        AnimManager.Instance.Border2None();
        AnimManager.Instance.CircularPlaneAnim();
        if (FoodManager.Instance.hasConnectionAndModel) {
            FoodManager.Instance.FoodModelConnection.SetModelScale();
        }
        canvas.SetActive(true);
    }

    public void OnSurfaceSwitch()
    {
        AnimManager.Instance.CircularPlaneAnim();
    }
}
