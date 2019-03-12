using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorToggle : MonoBehaviour, IClickable {
    public void ChangeVisibility()
    {
        switch (DecorManager.Instance.UiState)
        {
            case (int)DecorManager.UIStatesEnum.AutoPlace:
                gameObject.SetActive(true);
                break;
            case (int)DecorManager.UIStatesEnum.Idle:
                gameObject.SetActive(true);
                break;
            case (int)DecorManager.UIStatesEnum.Loading:
                gameObject.SetActive(false);
                break;
        }
    }

    public void ClickHandler()
    {
        if(DecorManager.Instance.is3DScene)
            DecorManager.Instance.LoadARScene();
        else DecorManager.Instance.Load3DScene();
    }

    void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(ClickHandler);
        DecorManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
    }
	
	void Update () {}
}
