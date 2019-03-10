using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FoodModelConnection : MonoBehaviour
{
    //[HideInInspector]
    public GameObject foodModel;
    [HideInInspector]
    public string assetBundleUrl;
    [HideInInspector]
    public string prefabName = "model";
    [HideInInspector]
    public AssetBundle bundle;
    public AssetDownloader assetDownloader;

    // Use this for initialization
    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if(button != null) button.onClick.AddListener(ButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {}

    public void ButtonClicked()
    {
        assetDownloader.ModelSelectHandler(this);
    }

    public GameObject GetGameObjectToPlace()
    {
        return foodModel;
    }

    public void DestroyFoodModel()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            Destroy(foodModel);
        }
    }
}
