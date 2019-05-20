using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAnimManager : MonoBehaviour {

    public static FoodAnimManager Instance { get; private set; }

    bool showedToggleAnimation;
    public bool showFixAnimation = true;

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

    #region Dummies
    public GameObject dummyAdd;
    Animator dummyAddAnimator;
    AnimationStates dummyAddStates;

    public GameObject dummyToggle;
    Animator dummyToggleAnimator;
    AnimationStates dummyToggleStates;

    public GameObject dummyFix;
    Animator dummyFixAnimator;
    AnimationStates dummyFixStates;
    #endregion

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

        #region Dummies
        dummyAddAnimator = dummyAdd.GetComponent<Animator>();
        dummyAddStates = dummyAdd.GetComponent<AnimationStates>();

        dummyToggleAnimator = dummyToggle.GetComponent<Animator>();
        dummyToggleStates = dummyToggle.GetComponent<AnimationStates>();

        dummyFixAnimator = dummyFix.GetComponent<Animator>();
        dummyFixStates = dummyFix.GetComponent<AnimationStates>();
        #endregion

        if (CircularPlane != null)
        {
            circularPlaneAnimator = CircularPlane.GetComponent<Animator>();
        }
    }

    private void Start()
    {
        DummyAdd();
    }

    private void Update()
    {
        if(!showedToggleAnimation
            && FoodManager.Instance.is3DScene
            && FoodManager.Instance.FoodModelConnection != null 
            && FoodManager.Instance.FoodModelConnection.FoodModel != null)
        {
            StartCoroutine(DummyToggleCoroutine());
        }

        if(showFixAnimation 
            && !FoodManager.Instance.is3DScene
            && FoodManager.Instance.hasSurfaceFound
            && FoodManager.Instance.FoodModelConnection != null
            && FoodManager.Instance.FoodModelConnection.FoodModel != null)
        {
            StartCoroutine(DummyFixCoroutine());
        }
    }

    public void OnARSceneWillLoad()
    {
        dummyToggleStates.isActive = false;
    }

    public void OnARSceneDidLoad()
    {
        Full2Border();
        CircularPlane = GameObject.Find("Plane/CircularPlane");
    }

    public void On3DSceneDidLoad()
    {
        Full2None();
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

    public void DummyAdd()
    {
        StartCoroutine(DummyAddCoroutine());
    }

    private IEnumerator DummyAddCoroutine()
    {
        bool initialScene = FoodManager.Instance.is3DScene;
        dummyAdd.SetActive(true);
        //dummyAddAnimator.Play("Dummy");
        while (FoodManager.Instance.FoodModelConnection == null && FoodManager.Instance.is3DScene == initialScene)
        {
            yield return null;
        }
        dummyAdd.SetActive(false);
    }

    private IEnumerator DummyToggleCoroutine()
    {
        dummyToggle.SetActive(true);
        //dummyToggleAnimator.Play("Dummy");
        yield return new WaitForSeconds(5);
        dummyToggle.SetActive(false);
        showedToggleAnimation = true;
    }

    private IEnumerator DummyFixCoroutine()
    {
        dummyFix.SetActive(true);
        //dummyFixAnimator.Play("Dummy");
        while (showFixAnimation)
        {
            yield return null;
        }
        dummyFix.SetActive(false);
    }

}
