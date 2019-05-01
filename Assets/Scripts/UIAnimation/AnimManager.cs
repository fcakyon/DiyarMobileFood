using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour {

    public static AnimManager Instance { get; private set; }

    const float animationInterval = 0.3f;
    public GameObject cameraMask;
    public GameObject subColorMask;
    public GameObject mainColorPanel;
    public GameObject planeNotificationBox;
    public GameObject arPlaneIcon;
    private GameObject circularPlane;
    public GameObject appLogo;
    Animator cameraMaskAnimator;
    Animator subColorMaskAnimator;
    Animator mainColorPanelAnimator;
    Animator planeNotificationBoxAnimator;
    Animator arPlaneIconAnimator;
    Animator appLogoAnimator;
    Animator circularPlaneAnimator;
    AnimationStates cameraMaskStates;
    AnimationStates mainColorPanelStates;
    AnimationStates appLogoStates;
    AnimationStates subColorMaskStates;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        cameraMaskAnimator = cameraMask.GetComponent<Animator>();
        subColorMaskAnimator = subColorMask.GetComponent<Animator>();
        mainColorPanelAnimator = mainColorPanel.GetComponent<Animator>();
        planeNotificationBoxAnimator = planeNotificationBox.GetComponent<Animator>();
        arPlaneIconAnimator = arPlaneIcon.GetComponent<Animator>();
        appLogoAnimator = appLogo.GetComponent<Animator>();

        cameraMaskStates = cameraMask.GetComponent<AnimationStates>();
        mainColorPanelStates = mainColorPanel.GetComponent<AnimationStates>();
        appLogoStates = appLogo.GetComponent<AnimationStates>();
        subColorMaskStates = subColorMask.GetComponent<AnimationStates>();

        if (CircularPlane != null)
        {
            circularPlaneAnimator = CircularPlane.GetComponent<Animator>();
        }
    }

    public GameObject CircularPlane
    {
        get { return circularPlane; }
        set
        {
            circularPlane = value;
            circularPlaneAnimator = CircularPlane.GetComponent<Animator>();
        }
    }

    public void Full2None()
    {
        mainColorPanelAnimator.Play("MainColor2Black");
        cameraMaskAnimator.Play("Full2None");
        subColorMaskAnimator.Play("Full2None");
        appLogoAnimator.Play("IconDisappear");
    }

    public void Full2Border()
    {
        StartCoroutine(Full2Border_Coroutine());
    }

    IEnumerator Full2Border_Coroutine()
    {
        mainColorPanelAnimator.Play("MainColor2Black");
        cameraMaskAnimator.Play("Full2Border");
        subColorMaskAnimator.Play("Full2Border");
        appLogoAnimator.Play("IconDisappear");
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
        appLogoAnimator.Play("IconDisappear");
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
        appLogoAnimator.Play("IconAppear");
        appLogo.SetActive(true);
        cameraMaskAnimator.Play("None2Full");
        subColorMaskAnimator.Play("None2Full");
    }

    public IEnumerator None2FullCoroutine()     {         mainColorPanelAnimator.Play("Black2MainColor");
        appLogoAnimator.Play("IconAppear");
        appLogo.SetActive(true);
        cameraMaskAnimator.Play("None2Full");
        subColorMaskAnimator.Play("None2Full");
         bool isPlaying = false;

        // Play does not work instantly, so hasEnded flag is not false here.
        // First check if it is started then check if it is ended. 
        while(!isPlaying)
        {
            isPlaying = !mainColorPanelStates.hasEnded ||
                !appLogoStates.hasEnded ||
                !cameraMaskStates.hasEnded ||
                !subColorMaskStates.hasEnded;
            yield return null;
        }

        while (isPlaying)
        {
            isPlaying = !(mainColorPanelStates.hasEnded &&
                appLogoStates.hasEnded &&
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
