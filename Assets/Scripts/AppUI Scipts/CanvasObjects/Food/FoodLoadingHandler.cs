using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLoadingHandler : MonoBehaviour {

	void Start () {
        FoodManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        gameObject.SetActive(false);
    }

    void ChangeVisibility()
    {
        Debug.Log("Loading Animation changevisibility");
        if (FoodManager.Instance.UiState == (int)FoodManager.UIStatesEnum.Loading)
            gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
