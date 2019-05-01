using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        AnimManager.Instance.Border2None();
        AnimManager.Instance.CircularPlaneAnim();
        canvas.SetActive(true);
    }

    public void OnSurfaceSwitch()
    {
        AnimManager.Instance.CircularPlaneAnim();
    }
}
