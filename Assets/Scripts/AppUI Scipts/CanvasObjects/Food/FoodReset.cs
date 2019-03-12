using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodReset : MonoBehaviour, IClickable {

    public void ChangeVisibility()
    {
        switch (FoodManager.Instance.UiState)
        {
            case (int)FoodManager.UIStatesEnum.AutoPlace:
                gameObject.SetActive(false);
                break;
            case (int)FoodManager.UIStatesEnum.Idle:
                gameObject.SetActive(false);
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
        FoodManager.Instance.Reset();
    }

    void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(ClickHandler);
        FoodManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }
	
	void Update () {}
}
