using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour {

    public GameObject cameraMask;
    public GameObject subColorMask;
    public GameObject mainColorPanel;
    public GameObject planeNotificationBox;
    public GameObject arPlaneIcon;
    public GameObject decorz3dLogo;
    public GameObject foodz3dLogo;
    public GameObject circularPlane;

    Animator cameraMaskAnimator;
    Animator subColorMaskAnimator;
    Animator mainColorPanelAnimator;
    Animator planeNotificationBoxAnimator;
    Animator arPlaneIconAnimator;
    Animator decorz3dLogoAnimator;
    Animator foodz3dLogoAnimator;
    Animator circularPlaneAnimator;
    const float animationInterval = 0.3f;

    public AnimationStates cameraMaskStates;
    public AnimationStates mainColorPanelStates;
    public AnimationStates decorz3dLogoStates;
    public AnimationStates subColorMaskStates;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        cameraMaskAnimator = cameraMask.GetComponent<Animator>();
        subColorMaskAnimator = subColorMask.GetComponent<Animator>();
        mainColorPanelAnimator = mainColorPanel.GetComponent<Animator>();
        planeNotificationBoxAnimator = planeNotificationBox.GetComponent<Animator>();
        arPlaneIconAnimator = arPlaneIcon.GetComponent<Animator>();
        decorz3dLogoAnimator = decorz3dLogo.GetComponent<Animator>();
        foodz3dLogoAnimator = foodz3dLogo.GetComponent<Animator>();
        if (circularPlane != null)
        {
            circularPlaneAnimator = circularPlane.GetComponent<Animator>();
        }
    }

    public void Full2None()
    {
        mainColorPanelAnimator.Play("MainColor2Black");
        cameraMaskAnimator.Play("Full2None");
        subColorMaskAnimator.Play("Full2None");
        decorz3dLogoAnimator.Play("IconDisappear");
    }

    public void Full2Border()
    {
        mainColorPanelAnimator.Play("MainColor2Black");
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
        mainColorPanelAnimator.Play("SetColor2Black");
        arPlaneIconAnimator.Play("IconDisappear");
        arPlaneIcon.SetActive(false);
        planeNotificationBoxAnimator.Play("PlaneNotificationDisappear");
        yield return new WaitForSeconds(animationInterval);
        subColorMaskAnimator.Play("Border2None");
        yield return new WaitForSeconds(animationInterval);
        cameraMaskAnimator.Play("Border2None");
    }

    public void None2Border()
    {
        StartCoroutine(None2BorderCoroutine());
    }

    public IEnumerator None2BorderCoroutine()
    {
        mainColorPanelAnimator.Play("SetColor2Black");
        decorz3dLogoAnimator.Play("IconDisappear");
        yield return new WaitForSeconds(animationInterval);
        subColorMaskAnimator.Play("None2Border");
        cameraMaskAnimator.Play("None2Border");
        planeNotificationBoxAnimator.Play("PlaneNotificationAppear");
        arPlaneIcon.SetActive(true);
        arPlaneIconAnimator.Play("IconAppear");
        arPlaneIconAnimator.Play("ARPlaneIconMovement");
    }

    public void None2Full()
    {
        mainColorPanelAnimator.Play("Black2MainColor");
        decorz3dLogoAnimator.Play("IconAppear");
        decorz3dLogo.SetActive(true);
        cameraMaskAnimator.Play("None2Full");
        subColorMaskAnimator.Play("None2Full");
    }

    public IEnumerator None2FullCoroutine()     {         mainColorPanelAnimator.Play("Black2MainColor");
        decorz3dLogoAnimator.Play("IconAppear");
        decorz3dLogo.SetActive(true);
        cameraMaskAnimator.Play("None2Full");
        subColorMaskAnimator.Play("None2Full");
         bool isPlaying = false;

        // Play does not work instantly, so hasEnded flag is not false here.
        // First check if it is started then check if it is ended. 
        while(!isPlaying)
        {
            isPlaying = !mainColorPanelStates.hasEnded ||
                !decorz3dLogoStates.hasEnded ||
                !cameraMaskStates.hasEnded ||
                !subColorMaskStates.hasEnded;
            yield return null;
        }

        while (isPlaying)
        {
            isPlaying = !(mainColorPanelStates.hasEnded &&
                decorz3dLogoStates.hasEnded &&
                cameraMaskStates.hasEnded &&
                subColorMaskStates.hasEnded);
            yield return null;
        }
        yield return null;     }

    public void SilentInit()
    {
        cameraMaskAnimator.Play("SilentInit");
        subColorMaskAnimator.Play("SilentInit");
    }   

    public void CircularPlaneAnim()
    {
        circularPlaneAnimator.Play("PlaneCircleAnim");
    }

}
