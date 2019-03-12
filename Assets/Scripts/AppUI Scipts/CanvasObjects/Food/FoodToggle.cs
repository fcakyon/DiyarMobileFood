using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodToggle : MonoBehaviour, IClickable {
    public void ChangeVisibility()
    {
        switch (FoodManager.Instance.UiState)
        {
            case (int)FoodManager.UIStatesEnum.AutoPlace:
                gameObject.SetActive(true);
                break;
            case (int)FoodManager.UIStatesEnum.Idle:
                gameObject.SetActive(true);
                break;
            case (int)FoodManager.UIStatesEnum.Loading:
                gameObject.SetActive(false);
                break;
            case (int)FoodManager.UIStatesEnum.Fixed:
                gameObject.SetActive(true);
                break;
        }
    }

    public void ClickHandler()
    {
        if(FoodManager.Instance.is3DScene)
            FoodManager.Instance.LoadARScene();
        else FoodManager.Instance.Load3DScene();
    }

    void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(ClickHandler);
        FoodManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
    }
	
	void Update () {}
}
