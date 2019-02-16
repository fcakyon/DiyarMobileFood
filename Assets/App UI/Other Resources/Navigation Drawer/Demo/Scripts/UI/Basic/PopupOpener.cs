using UnityEngine;

namespace NavigationDrawer.UI
{
    public class PopupOpener : MonoBehaviour
    {
        public GameObject popupPrefab;

        public virtual void OpenWindow()
        {
            popupPrefab.SetActive(true);
            popupPrefab.transform.localScale = Vector3.one;
            popupPrefab.GetComponent<Popup>().Open();
        }
    }
}