using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour {

    public GameObject cameraMask;
    public GameObject subColorMask;
    public GameObject planeNotificationBox;
    public GameObject arPlaneIcon;
    public GameObject decorz3dLogo;
    public GameObject foodz3dLogo;
    public GameObject circularPlane;

    Animator cameraMaskAnimator;
    Animator subColorMaskAnimator;
    Animator planeNotificationBoxAnimator;
    Animator arPlaneIconAnimator;
    Animator decorz3dLogoAnimator;
    Animator foodz3dLogoAnimator;
    Animator circularPlaneAnimator;
    const float animationInterval = 0.3f;

    private void Awake()
    {
        cameraMaskAnimator = cameraMask.GetComponent<Animator>();
        subColorMaskAnimator = subColorMask.GetComponent<Animator>();
        planeNotificationBoxAnimator = planeNotificationBox.GetComponent<Animator>();
        arPlaneIconAnimator = arPlaneIcon.GetComponent<Animator>();
        decorz3dLogoAnimator = decorz3dLogo.GetComponent<Animator>();
        foodz3dLogoAnimator = foodz3dLogo.GetComponent<Animator>();
        circularPlaneAnimator = circularPlane.GetComponent<Animator>();
    }

    public void Full2None()
    {
        cameraMaskAnimator.Play("Full2None");
        subColorMaskAnimator.Play("Full2None");
        decorz3dLogoAnimator.Play("IconDisappear");
    }

    public void Full2Border()
    {
        StartCoroutine(Full2Border_Coroutine());
    }

    IEnumerator Full2Border_Coroutine()
    {
        cameraMaskAnimator.Play("Full2Border");
        subColorMaskAnimator.Play("Full2Border");
        decorz3dLogoAnimator.Play("IconDisappear");
        yield return new WaitForSeconds(animationInterval);
        planeNotificationBoxAnimator.Play("PlaneNotificationAppear");
        arPlaneIcon.SetActive(true);
        arPlaneIconAnimator.Play("IconAppear");
        arPlaneIconAnimator.Play("ARPlaneIconMovement");
    }

    public void Border2None() {
        StartCoroutine(Border2None_Coroutine());
    }

    IEnumerator Border2None_Coroutine()
    {
        arPlaneIconAnimator.Play("IconDisappear");
        arPlaneIcon.SetActive(false);
        planeNotificationBoxAnimator.Play("PlaneNotificationDisappear");
        yield return new WaitForSeconds(animationInterval);
        subColorMaskAnimator.Play("Border2None");
        yield return new WaitForSeconds(animationInterval);
        cameraMaskAnimator.Play("Border2None");
    }

    public void None2Full()
    {
        decorz3dLogoAnimator.Play("IconAppear");
        decorz3dLogo.SetActive(true);
        cameraMaskAnimator.Play("None2Full");
        subColorMaskAnimator.Play("None2Full");
    }

    public void CircularPlaneAnim()
    {
        circularPlaneAnimator.Play("PlaneCircleAnim");
    }

}
