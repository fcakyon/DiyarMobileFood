using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorModelConnection : MonoBehaviour
{
    
    public bool hasDecorModelBeenPlaced = false;
    public GameObject decorModel;
    public string assetBundleUrl;
    [HideInInspector]
    public string prefabName = "model";
    [HideInInspector]
    public AssetBundle bundle;
    public AssetDownloader assetDownloader;
    public float height;

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(ButtonClicked);
    }

    void Update()
    {}

    public void ButtonClicked()
    {
        assetDownloader.ModelSelectHandler(this);
    }

    public void DestroyDecorModel()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            Destroy(decorModel);
            Destroy(gameObject);
        }
    }
}
