using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAdd : MonoBehaviour, IClickable {

    public void ChangeVisibility()
    {
        switch (FoodManager.Instance.UiState)
        {
            case (int)FoodManager.UIStatesEnum.AutoPlace:
                gameObject.SetActive(false);
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

    public void ClickHandler() {}

    void Start () {
        FoodManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }
	
	void Update () {}
}
