using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorXRManager : MonoBehaviour {

    public GameObject canvas;

    public void OnSurfaceAttach()
    {
        GameObject.Find("AnimationManager").GetComponent<AnimManager>().Border2None();
        canvas.SetActive(true);
    }
}
