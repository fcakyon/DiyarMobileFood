using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        DecorAnimManager.Instance.Border2None();
        DecorAnimManager.Instance.CircularPlaneAnim();
        DecorManager.Instance.hasSurfaceFound = true;
        // Setting model visible after surface has found
        if (DecorManager.Instance.hasConnectionAndModel) {
            float modelScale = 1f;
            DecorManager.Instance.DecorModelConnection.SetModelScale(modelScale);
        }
        else
        {
            DecorAnimManager.Instance.DummyAdd();
        }
        canvas.SetActive(true);
    }

    public void OnSurfaceSwitch()
    {
        DecorAnimManager.Instance.CircularPlaneAnim();
    }
}
