using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodToggle : MonoBehaviour, IClickable {
    public void ChangeVisibility()
    {
        switch (FoodManager.Instance.UiState)
        {
            case FoodManager.UIStates.AutoPlace:
                gameObject.SetActive(false);
                break;
            case FoodManager.UIStates.Idle:
                gameObject.SetActive(true);
                break;
            case FoodManager.UIStates.Loading:
                gameObject.SetActive(false);
                break;
            case FoodManager.UIStates.Fixed:
                gameObject.SetActive(true);
                break;
        }
    }

    public void ClickHandler()
    {
        FoodManager.Instance.ToogleScene();
    }

    void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(ClickHandler);
        FoodManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }

	void Update () {}
}
