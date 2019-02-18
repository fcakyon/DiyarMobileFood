using System;
using System.Collections;
using NavigationDrawer.UI;
using UnityEngine;

namespace NavigationDrawer.Controller
{
    public class NavDrawerPanelController : MonoBehaviour
    {
        [SerializeField]
        private Component Components;

        [Serializable]
        private class Component
        {
#pragma warning disable 649
            public ProfilePanel ProfilePanel;
            public RatingPanel RatingPanel;
            public AboutPanel AboutPanel;
            public TermsPanel TermsPanel;
            public HelpPanel HelpPanel;
            public GameObject BtnMenu;
#pragma warning restore 649
        }

        public void OpenProfilePanel()
        {
            OpenWindow(Components.ProfilePanel.gameObject);
        }
        public void OpenRatingPanel()
        {
            OpenWindow(Components.RatingPanel.gameObject);
        }
        public void OpenHelpPanel()
        {
            OpenWindow(Components.HelpPanel.gameObject);
        }
        public void OpenAboutPanel()
        {
            OpenWindow(Components.AboutPanel.gameObject);
        }
        public void OpenTermsPanel()
        {
            OpenWindow(Components.TermsPanel.gameObject);
        }

        public void SetActiveBtnMenu(bool value)
        {
            Components.BtnMenu.SetActive(value);
        }

        public void CloseAllPanel()
        {
            if (Components.ProfilePanel.isActiveAndEnabled) { CloseWindow(Components.ProfilePanel.gameObject); }
            if (Components.RatingPanel.isActiveAndEnabled) { CloseWindow(Components.RatingPanel.gameObject); }
            if (Components.HelpPanel.isActiveAndEnabled) { CloseWindow(Components.HelpPanel.gameObject); }
            if (Components.TermsPanel.isActiveAndEnabled) { CloseWindow(Components.TermsPanel.gameObject); }
            if (Components.AboutPanel.isActiveAndEnabled) { CloseWindow(Components.AboutPanel.gameObject); }
        }

        private void CloseWindow(GameObject popup)
        {
            popup.GetComponent<Popup>().CloseWindow();
        }

        private void OpenWindow(GameObject popup)
        {
            StartCoroutine(OpenWindowAsync(popup));
        }

        private IEnumerator OpenWindowAsync(GameObject popup)
        {
            yield return new WaitForSeconds(0.25f);

            popup.SetActive(true);
            popup.transform.localScale = Vector3.one;
            popup.GetComponent<Popup>().Open();
        }
    }
}