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
    //[HideInInspector]
    public string prefabName = "model";
    [HideInInspector]
    public AssetBundle bundle;
    public AssetDownloader assetDownloader;
    public float height;
    public bool hasModelBeenPlaced = false;
    public string info;

    public GameObject FoodModel
    {
        get {return foodModel; }

        set
        {
            foodModel = value;
            SetModelScale(1f);
        }
    }

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if(button != null) button.onClick.AddListener(ButtonClicked);
    }

    void Update()
    {}

    public void SetModelScale(float modelScale)
    {
        if (FoodManager.Instance.is3DScene)
        {
            ModelScaleTransformer.CustomModelScale3D(foodModel, modelScale);
        }
        else
        {
            ModelScaleTransformer.ResetModelScaleAR(foodModel, height);
        }
    }

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
