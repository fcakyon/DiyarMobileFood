using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorLoadingHandler : MonoBehaviour {

	void Start () {
        DecorManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        gameObject.SetActive(false);
    }

    void ChangeVisibility()
    {
        if (DecorManager.Instance.UiState == (int)DecorManager.UIStatesEnum.Loading)
            gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
