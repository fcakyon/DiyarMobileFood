using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FoodModelConnection : MonoBehaviour
{
    //[HideInInspector]
    private GameObject foodModel;
    [HideInInspector]
    public string assetBundleUrl;
    [HideInInspector]
    public string prefabName = "model";
    [HideInInspector]
    public AssetBundle bundle;
    public AssetDownloader assetDownloader;

    public GameObject FoodModel
    {
        get {return foodModel; }

        set
        {
            foodModel = value;
            if (FoodManager.Instance.is3DScene) 
            {   
                ModelScaleTransformer.ModelScaler3D(foodModel);
            }
            else
            {
                ModelScaleTransformer.ModelScalerAR(foodModel, 30);
            }

        }
    }

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if(button != null) button.onClick.AddListener(ButtonClicked);
    }

    void Update()
    {}

    public void ButtonClicked()
    {
        assetDownloader.ModelSelectHandler(this);
    }

    public void DestroyGameObject()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            Destroy(FoodModel);
            Destroy(gameObject);
        }
    }
}
