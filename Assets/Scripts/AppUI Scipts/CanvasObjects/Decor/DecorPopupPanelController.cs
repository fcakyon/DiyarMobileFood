using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorPopupPanelController : MonoBehaviour, ICanvasPanel
{

    public bool isPanelOpen;
    private Animator cameraMaskAnimator;

    public void ClosePanel()
    {
        isPanelOpen = false;
        cameraMaskAnimator = gameObject.GetComponent<Animator>();
        cameraMaskAnimator.Play("Popup Panel Out");
    }

    public void OpenPanel()
    {
        isPanelOpen = true;
        cameraMaskAnimator = gameObject.GetComponent<Animator>();
        cameraMaskAnimator.Play("Popup Panel In");
    }

}
