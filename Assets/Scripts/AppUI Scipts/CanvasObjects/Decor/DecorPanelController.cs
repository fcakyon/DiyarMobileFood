using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorPanelController : MonoBehaviour {

    public GameObject decorPopupPanel;
    public GameObject decorInfoPanel;
    private DecorPopupPanelController decorPopupPanelController;
    private DecorPopupPanelController decorInfoPanelController;

    // Objects to toggle visibility
    public CanvasGroup addButton;
    public GameObject addNotification;

    private void Start()
    {
        decorPopupPanelController = decorPopupPanel.GetComponent<DecorPopupPanelController>();
        decorInfoPanelController = decorInfoPanel.GetComponent<DecorPopupPanelController>();
    }

    public void ClosePopupPanel()
    {
        if(decorPopupPanelController.isPanelOpen)
        {
            decorPopupPanelController.ClosePanel();

            addButton.alpha = 1;
        }
    }

    public void OpenPopupPanel()
    {
        if (!decorPopupPanelController.isPanelOpen)
        {
            decorPopupPanelController.OpenPanel();
            addButton.alpha = 0;
            if (DecorManager.Instance.is3DScene)
            {
                addNotification.SetActive(false);
            }
        }
    }

    public void CloseInfoPanel()
    {
        if (decorInfoPanelController.isPanelOpen)
        {
            decorInfoPanelController.ClosePanel();
        }
    }

    public void OpenInfoPanel()
    {
        if (!decorInfoPanelController.isPanelOpen)
        {
            decorInfoPanelController.OpenPanel();
        }
    }

    public void CloseAllPanels()
    {
        ClosePopupPanel();
        CloseInfoPanel();
    }

}
