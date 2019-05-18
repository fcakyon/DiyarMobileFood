using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPopupPanelController : MonoBehaviour, ICanvasPanel
{

    public bool isPanelOpen;
    private Animator cameraMaskAnimator;

    public void ClosePanel()
    {
        if (isPanelOpen)
        {
            isPanelOpen = false;
            cameraMaskAnimator = gameObject.GetComponent<Animator>();
            cameraMaskAnimator.Play("Popup Panel Out");
        }
    }

    public void OpenPanel()
    {
        if (!isPanelOpen)
        {
            isPanelOpen = true;
            cameraMaskAnimator = gameObject.GetComponent<Animator>();
            cameraMaskAnimator.Play("Popup Panel In");
        }
    }

}
