using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        AnimManager.Instance.Border2None();
        AnimManager.Instance.CircularPlaneAnim();
        DecorManager.Instance.hasSurfaceFound = true;
        // Setting model visible after surface has found
        if (DecorManager.Instance.hasConnectionAndModel) {
            DecorManager.Instance.DecorModelConnection.SetModelScale();
        }
        else
        {
            AnimManager.Instance.DummyAdd();
        }
        canvas.SetActive(true);
    }

    public void OnSurfaceSwitch()
    {
        AnimManager.Instance.CircularPlaneAnim();
    }
}
