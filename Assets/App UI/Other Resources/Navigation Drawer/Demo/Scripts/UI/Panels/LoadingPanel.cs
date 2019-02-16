using UnityEngine;
using UnityEngine.UI;

public enum ELoading
{
    LoadData,
    UnloadData
}

namespace Loading
{
    public class LoadingPanel : Singleton<LoadingPanel>
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private Text _txtLoading;
        [SerializeField] private Slider _slider;

        public readonly float Speed = 0.1f;
        public bool Load { get; private set; }

        private void Update()
        {
            if (!Load) return;

            if (_slider.value < 1.0f)
            {
                _slider.value = _slider.value + Time.deltaTime * Speed;
            }
            else
            {
                _slider.value = 0.0f;
            }
        }

        public void LoadingStart(ELoading state)
        {
            _root.SetActive(true);
            Load = true;

            switch (state)
            {
                case ELoading.LoadData:
                    _txtLoading.text = "LOADING DATA ...";
                    break;
                case ELoading.UnloadData:
                    _txtLoading.text = "UNLOADING DATA ...";
                    break;
                default:
                    break;
            }
        }

        public void LoadingStop()
        {
            _slider.value = 0.0f;
            Load = false;
            _root.SetActive(false);
        }
    }
}