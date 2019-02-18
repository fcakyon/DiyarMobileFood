using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NavigationDrawer.UI
{
    public class Popup : MonoBehaviour
    {
        public Sprite PopupBackground;
        public Material Blur;
        public Color BackgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

        private const float DestroyTime = 0.2f;
        private GameObject _background;

        public void Open()
        {
            AddBackground();
        }

        public void CloseWindow()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }

            RemoveBackground();
            StartCoroutine(RunPopupDeactive());
        }

        private IEnumerator RunPopupDeactive()
        {
            yield return new WaitForSeconds(DestroyTime);
            Destroy(_background);
            gameObject.SetActive(false);
        }

        private void AddBackground()
        {
            _background = new GameObject("PopupBackground");

            var image = _background.AddComponent<Image>();
            var sprite = PopupBackground;
            var material = Blur;
            var newColor = image.color;

            image.material = material;
            image.sprite = sprite;
            image.color = newColor;

            var canvas = GameObject.Find("UICanvas");
            _background.transform.localScale = new Vector3(1, 1, 1);
            _background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            _background.transform.SetParent(canvas.transform, false);
            _background.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        private void RemoveBackground()
        {
            var image = _background.GetComponent<Image>();
            if (image != null)
            {
                image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }
    }
}