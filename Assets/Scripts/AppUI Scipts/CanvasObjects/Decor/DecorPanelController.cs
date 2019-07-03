using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorPanelController : MonoBehaviour {

    public GameObject decorPopupPanel;
    public GameObject decorInfoPanel;
    public List<GameObject> gameObjectsToToggleWithPopulPanel = new List<GameObject>();
    private DecorPopupPanelController decorPopupPanelController;
    private DecorPopupPanelController decorInfoPanelController;

    public void ClosePopupPanel()
    {
        decorPopupPanelController = decorPopupPanel.GetComponent<DecorPopupPanelController>();
        decorPopupPanelController.ClosePanel();
        gameObjectsToToggleWithPopulPanel.ForEach((GameObject obj) => obj.SetActive(true));
    }

    public void OpenPopupPanel()
    {
        decorPopupPanelController = decorPopupPanel.GetComponent<DecorPopupPanelController>();
        decorPopupPanelController.OpenPanel();
        gameObjectsToToggleWithPopulPanel.ForEach((GameObject obj) => obj.SetActive(false));
    }

    public void CloseInfoPanel()
    {
        decorInfoPanelController = decorInfoPanel.GetComponent<DecorPopupPanelController>();
        decorInfoPanelController.ClosePanel();
    }

    public void OpenInfoPanel()
    {
        decorInfoPanelController = decorInfoPanel.GetComponent<DecorPopupPanelController>();
        decorInfoPanelController.OpenPanel();
    }

    public void CloseAllPanels()
    {
        ClosePopupPanel();
        CloseInfoPanel();
    }

}
