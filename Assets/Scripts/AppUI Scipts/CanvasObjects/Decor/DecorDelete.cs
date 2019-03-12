using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorDelete : MonoBehaviour, IClickable
{
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
        DecorManager.Instance.RemoveConnection();
    }

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ClickHandler);
        DecorManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }

    void Update()
    {

    }
}
