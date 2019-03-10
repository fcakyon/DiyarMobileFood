using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorModelConnection : MonoBehaviour
{
    
    public bool hasDecorModelBeenPlaced = false;
    public GameObject decorModel;
    public DecorManager decorManager;
    public string assetBundleUrl;
    [HideInInspector]
    public string prefabName;
    [HideInInspector]
    public AssetBundle bundle;
    public AssetDownloader assetDownloader;

    // Use this for initialization
    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClicked()
    {
        hasDecorModelBeenPlaced = false;
        assetDownloader.ModelSelectHandler(this);
    }

    public GameObject GetGameObjectToPlace()
    {
        return decorModel;
    }

    public void VanishDecorModel()
    {
        hasDecorModelBeenPlaced = false;
        decorManager.HideDecorModel();
        decorManager.RemoveDecorModelConnection();
    }

    public void DestroyDecorModel()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            Destroy(decorModel);
        }
    }
}
