﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorReset : MonoBehaviour, IClickable {

    public void ChangeVisibility()
    {
        switch (DecorManager.Instance.UiState)
        {
            case DecorManager.UIStates.AutoPlace:
                gameObject.SetActive(false);
                break;
            case DecorManager.UIStates.Idle:
                gameObject.SetActive(false);
                break;
            case DecorManager.UIStates.Loading:
                gameObject.SetActive(false);
                break;
        }
    }

    public void ClickHandler()
    {
        //DecorManager.Instance.Reset(); //TODO No Reset function in DecorManager
    }

    void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(ClickHandler);
        DecorManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }
	
	void Update () {}
}
