using System;
using NavigationDrawer.UI;
using UnityEngine;
using UnityEngine.UI;

namespace NavigationDrawer.Controller
{
    public class NavDrawerController : MonoBehaviour
    {
        [SerializeField, Header("Nav panels")]
        public NavDrawerPanelController NavDrawerPanelController;
        public NavDrawerPanel NavDrawerPanel;

        [SerializeField, Header("Nav buttons")]
        public Button BtnProfile;
        public Button BtnRating;
        public Button BtnHelp;
        public Button BtnAbout;
        public Button BtnTerms;

        private void Start()
        {
            BtnProfile.onClick.AddListener(NavDrawerPanelOnProfile);
            BtnRating.onClick.AddListener(NavDrawerPanelOnRating);
            BtnHelp.onClick.AddListener(NavDrawerPanelOnHelp);
            BtnAbout.onClick.AddListener(NavDrawerPanelOnAbout);
            BtnTerms.onClick.AddListener(NavDrawerPanelOnTerms);
        }

        public void InitNavDrawer()
        {
            NavDrawerPanel.Open();
        }

        public void CloseAllPanel()
        {
            NavDrawerPanelController.CloseAllPanel();
        }

        private void NavDrawerPanelOnProfile()
        {
            NavDrawerPanelController.OpenProfilePanel();
        }
        private void NavDrawerPanelOnRating()
        {
            NavDrawerPanelController.OpenRatingPanel();
        }
        private void NavDrawerPanelOnHelp()
        {
            NavDrawerPanelController.OpenHelpPanel();
        }
        private void NavDrawerPanelOnAbout()
        {
            NavDrawerPanelController.OpenAboutPanel();
        }
        private void NavDrawerPanelOnTerms()
        {
            NavDrawerPanelController.OpenTermsPanel();
        }
    }
}