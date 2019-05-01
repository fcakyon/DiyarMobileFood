using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        AnimManager.Instance.Border2None();
        AnimManager.Instance.CircularPlaneAnim();
        if (DecorManager.Instance.hasConnectionAndModel) {
            DecorManager.Instance.DecorModelConnection.SetModelScale();
        }
        canvas.SetActive(true);
    }

    public void OnSurfaceSwitch()
    {
        AnimManager.Instance.CircularPlaneAnim();
    }
}
