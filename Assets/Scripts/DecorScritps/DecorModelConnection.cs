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

    public GameObject DecorModel
    {
        get { return decorModel; }

        set
        {
            decorModel = value;
            SetModelScale();
        }
    }

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(ButtonClicked);
    }

    void Update()
    {}

    public void SetModelScale()
    {
        if (DecorManager.Instance.is3DScene)
        {
            ModelScaleTransformer.ModelScaler3D(decorModel);
        }
        else
        {
            ModelScaleTransformer.ModelScalerAR(decorModel, height);
        }
    }

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
