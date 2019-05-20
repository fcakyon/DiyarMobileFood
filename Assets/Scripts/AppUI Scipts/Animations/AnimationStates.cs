using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStates : MonoBehaviour {

    public bool hasEnded = false;
    public bool isActive = true;

    public void AnimationStarted()
    {
        //Debug.Log("AnimationStarted");
        hasEnded = false;
    }

    public void AnimationEnded()
    {
        //Debug.Log("AnimationEnded");
        hasEnded = true;

    }

    private void Update()
    {
        if (!isActive) gameObject.SetActive(false);
    }

}
