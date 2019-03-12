using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorAdd : MonoBehaviour, IClickable {

    public void ChangeVisibility()
    {
        switch (DecorManager.Instance.UiState)
        {
            case (int)DecorManager.UIStatesEnum.AutoPlace:
                gameObject.SetActive(false);
                break;
            case (int)DecorManager.UIStatesEnum.Idle:
                gameObject.SetActive(true);
                break;
            case (int)DecorManager.UIStatesEnum.Loading:
                gameObject.SetActive(false);
                break;
        }
    }

    public void ClickHandler() {}

    void Start () {
        DecorManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }
	
	void Update () {}
}
