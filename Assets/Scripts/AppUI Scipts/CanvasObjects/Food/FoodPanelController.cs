using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPanelController : MonoBehaviour
{

    public GameObject foodPopupPanel;
    public GameObject foodInfoPanel;
    private FoodPopupPanelController foodPopupPanelController;
    private FoodPopupPanelController foodInfoPanelController;

    public void ClosePopupPanel()
    {
        foodPopupPanelController = foodPopupPanel.GetComponent<FoodPopupPanelController>();
        foodPopupPanelController.ClosePanel();
    }

    public void OpenPopupPanel()
    {
        foodPopupPanelController = foodPopupPanel.GetComponent<FoodPopupPanelController>();
        foodPopupPanelController.OpenPanel();
    }

    public void CloseInfoPanel()
    {
        foodInfoPanelController = foodInfoPanel.GetComponent<FoodPopupPanelController>();
        foodInfoPanelController.ClosePanel();
    }

    public void OpenInfoPanel()
    {
        foodInfoPanelController = foodInfoPanel.GetComponent<FoodPopupPanelController>();
        foodInfoPanelController.OpenPanel();
    }

    public void CloseAllPanels()
    {
        ClosePopupPanel();
        CloseInfoPanel();
    }

}
