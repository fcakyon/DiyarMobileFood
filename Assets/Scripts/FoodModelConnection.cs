using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodModelConnection : MonoBehaviour
{

    public bool hasFoodModelBeenChanged = false;
    public GameObject foodModel;
    public string assetBundleUrl;
    public string prefabName;
    public MainFoodUI MainFoodUIScript;

    // Use this for initialization
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}

    public void ButtonClicked()
    {
        VanishFoodModel();
        StartCoroutine(DownloadAssetBundleAndSetFoodModel(assetBundleUrl, prefabName));
    }

    public GameObject GetGameObjectToPlace()
    {
        return foodModel;
    }

    public void VanishFoodModel()
    {
        hasFoodModelBeenChanged = false;
        MainFoodUIScript.HideFoodModel();
        MainFoodUIScript.RemoveFoodModelConnection();
    }

    IEnumerator DownloadAssetBundleAndSetFoodModel(string url, string modelName)
        {
            UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
            yield return request.SendWebRequest();
            AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
            GameObject foodModelAsset = bundle.LoadAsset<GameObject>(modelName);
            foodModel = Instantiate(foodModelAsset, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            MainFoodUIScript.SetFoodModel(this);
        }

}
