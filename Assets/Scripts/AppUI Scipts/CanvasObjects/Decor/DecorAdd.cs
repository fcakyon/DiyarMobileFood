using UnityEngine;

public class 
DecorAdd : MonoBehaviour, IClickable {

    public void ChangeVisibility()
    {
        switch (DecorManager.Instance.UiState)
        {
            case DecorManager.UIStates.AutoPlace:
                gameObject.SetActive(false);
                break;
            case DecorManager.UIStates.Idle:
                gameObject.SetActive(true);
                break;
            case DecorManager.UIStates.Loading:
                gameObject.SetActive(false);
                break;
        }
    }

    public void ClickHandler() {}

    void Start () {
        DecorManager.Instance.OnUIStateChange.AddListener(ChangeVisibility);
        ChangeVisibility();
    }
	
	void Update () {}
}
