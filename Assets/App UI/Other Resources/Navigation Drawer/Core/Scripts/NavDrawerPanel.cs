using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NavigationDrawer.UI
{
    public class NavDrawerPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public enum ENavigation
        {
            Left,
            Right
        }

        [SerializeField, Header("Components")]
        public Image ImgBackground;
        public GameObject PanelLayer;

        [SerializeField, Header("Properties")]
        public ENavigation NavigationType;
        public bool DarkenBackground = true;
        public bool TapBackgroundToClose;
        public bool OpenOnStart;
        public float AnimationDuration = 0.5f;

        private int _animState;
        private float _maxPosition;
        private float _minPosition;
        private float _animStartTime;
        private float _animDeltaTime;
        private float _currentBackgroundAlpha;

        private RectTransform _rectTransform;
        private RectTransform _backgroundRectTransform;

        private GameObject _backgroundGameObject;

        private CanvasGroup _backgroundCanvasGroup;
        
        private Vector2 _currentPos;
        private Vector2 _tempVector2;

        private void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _backgroundRectTransform = ImgBackground.GetComponent<RectTransform>();
            _backgroundCanvasGroup = ImgBackground.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            if (NavigationType == ENavigation.Left)
            {
                _maxPosition = _rectTransform.rect.width / 2;
            }
            else if (NavigationType == ENavigation.Right)
            {
                _maxPosition = -_rectTransform.rect.width / 2;
            }

            _minPosition = -_maxPosition;

            RefreshBackgroundSize();

            _backgroundGameObject = ImgBackground.gameObject;

            if (OpenOnStart)
            {
                Open();
            }
            else
            {
                _backgroundGameObject.SetActive(false);
                PanelLayer.SetActive(false);
            }
        }

        private void Update()
        {
            if (_animState == 1)
            {
                _animDeltaTime = Time.realtimeSinceStartup - _animStartTime;

                if (_animDeltaTime <= AnimationDuration)
                {
                    _rectTransform.anchoredPosition = QuintOut(_currentPos, new Vector2(_maxPosition, _rectTransform.anchoredPosition.y), _animDeltaTime, AnimationDuration);
                    if (DarkenBackground)
                    {
                        _backgroundCanvasGroup.alpha = QuintOut(_currentBackgroundAlpha, 1f, _animDeltaTime, AnimationDuration);
                    }
                }
                else
                {
                    _rectTransform.anchoredPosition = new Vector2(_maxPosition, _rectTransform.anchoredPosition.y);
                    if (DarkenBackground)
                    {
                        _backgroundCanvasGroup.alpha = 1f;
                    }
                    _animState = 0;
                }
            }
            else if (_animState == 2)
            {
                _animDeltaTime = Time.realtimeSinceStartup - _animStartTime;
                if (_animDeltaTime <= AnimationDuration)
                {
                    _rectTransform.anchoredPosition = QuintOut(_currentPos, new Vector2(_minPosition, _rectTransform.anchoredPosition.y), _animDeltaTime, AnimationDuration);
                    if (DarkenBackground)
                    {
                        _backgroundCanvasGroup.alpha = QuintOut(_currentBackgroundAlpha, 0f, _animDeltaTime, AnimationDuration);
                    }
                }
                else
                {
                    _rectTransform.anchoredPosition = new Vector2(_minPosition, _rectTransform.anchoredPosition.y);
                    if (DarkenBackground)
                    {
                        _backgroundCanvasGroup.alpha = 0f;
                    }
                    _backgroundGameObject.SetActive(false);
                    PanelLayer.SetActive(false);

                    _animState = 0;
                }
            }

            if (NavigationType == ENavigation.Left)
            {
                _rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(_rectTransform.anchoredPosition.x, _minPosition, _maxPosition), _rectTransform.anchoredPosition.y);
            }
            else if (NavigationType == ENavigation.Right)
            {
                _rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(_rectTransform.anchoredPosition.x, _maxPosition, _minPosition), _rectTransform.anchoredPosition.y);
            }
        }

        public void BackgroundTap()
        {
            if (TapBackgroundToClose)
            {
                Close();
            }
        }

        public void Open()
        {
            RefreshBackgroundSize();
            _backgroundGameObject.SetActive(true);
            PanelLayer.SetActive(true);
            _currentPos = _rectTransform.anchoredPosition;
            _currentBackgroundAlpha = _backgroundCanvasGroup.alpha;
            _backgroundCanvasGroup.blocksRaycasts = true;
            _animStartTime = Time.realtimeSinceStartup;
            _animState = 1;
        }

        public void Close()
        {
            _currentPos = _rectTransform.anchoredPosition;
            _currentBackgroundAlpha = _backgroundCanvasGroup.alpha;
            _backgroundCanvasGroup.blocksRaycasts = false;
            _animStartTime = Time.realtimeSinceStartup;
            _animState = 2;
        }

        private void RefreshBackgroundSize()
        {
            if (NavigationType == ENavigation.Left)
            {
                _backgroundRectTransform.sizeDelta = new Vector2(Screen.width + 1f, _backgroundRectTransform.sizeDelta.y);
            }
            else if(NavigationType == ENavigation.Right)
            {
                _backgroundRectTransform.sizeDelta = new Vector2(Screen.width, _backgroundRectTransform.sizeDelta.y);
                _backgroundRectTransform.localPosition = new Vector2(-(_rectTransform.rect.width / 2), 0);
            }
        }

        public void OnBeginDrag(PointerEventData data)
        {
            RefreshBackgroundSize();

            _animState = 0;

            _backgroundGameObject.SetActive(true);
            PanelLayer.SetActive(true);
        }

        public void OnDrag(PointerEventData data)
        {
            _tempVector2 = _rectTransform.anchoredPosition;
            _tempVector2.x += data.delta.x;

            _rectTransform.anchoredPosition = _tempVector2;

            if (DarkenBackground)
            {
                _backgroundCanvasGroup.alpha = 1 - (_maxPosition - _rectTransform.anchoredPosition.x) / (_maxPosition - _minPosition);
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (NavigationType == ENavigation.Left)
            {
                if (Mathf.Abs(data.delta.x) >= 0.5f)
                {
                    if (data.delta.x > 0.5f)
                    {
                        Open();
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    if ((_rectTransform.anchoredPosition.x - _minPosition) >
                        (_maxPosition - _rectTransform.anchoredPosition.x))
                    {
                        Open();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            else if(NavigationType == ENavigation.Right)
            {
                if (Mathf.Abs(data.delta.x) >= 0.5f)
                {
                    if (data.delta.x < 0.5f)
                    {
                        Open();
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    if ((_rectTransform.anchoredPosition.x - _minPosition) <
                        (_maxPosition - _rectTransform.anchoredPosition.x))
                    {
                        Open();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        private Vector2 QuintOut(Vector2 startValue, Vector2 endValue, float time, float duration)
        {
            var tempVector2 = startValue;
            tempVector2.x = QuintOut(startValue.x, endValue.x, time, duration);
            tempVector2.y = QuintOut(startValue.y, endValue.y, time, duration);
            return tempVector2;
        }

        protected virtual float QuintOut(float startValue, float endValue, float time, float duration)
        {
            var differenceValue = endValue - startValue;
            time = Mathf.Clamp(time, 0f, duration);
            time /= duration;

            if (time == 0f)
            {
                return startValue;
            }

            if (time == 1f)
            {
                return endValue;
            }

            time--;
            return differenceValue * (time * time * time * time * time + 1) + startValue;
        }
    }
}