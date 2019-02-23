using System;
using System.Collections;
using Loading;
using UnityEngine;

namespace NavigationDrawer.Controller
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private Controller Controllers;

        [Serializable]
        private class Controller
        {
#pragma warning disable 649
            public NavDrawerController NavDrawerController;
            public NavDrawerPanelController NavDrawerPanelController;
#pragma warning restore 649
        }

        private IEnumerator Start()
        {
            LoadingPanel.Instance.LoadingStart(ELoading.LoadData);

            yield return StartCoroutine(InitAsync());

            LoadingPanel.Instance.LoadingStop();
        }

        private IEnumerator InitAsync()
        {
            yield return new WaitForSeconds(2.0f);

            Init();
        }

        private void Init()
        {
            Controllers.NavDrawerController.InitNavDrawer();
        }
    }
}