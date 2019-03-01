using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FoodModelConnection : MonoBehaviour
{

    public bool hasFoodModelBeenChanged = false;
    public GameObject foodModel;
    public string assetBundleUrl;
    public string prefabName;
    public MainFoodUI MainFoodUIScript;
    AssetBundle bundle;
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

    public void DestroyFoodModel()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            Destroy(foodModel);
        }
    }

    public void VanishFoodModel()
    {
        hasFoodModelBeenChanged = false;
        MainFoodUIScript.HideFoodModel();
        MainFoodUIScript.RemoveFoodModelConnection();
    }

    IEnumerator DownloadAssetBundleAndSetFoodModel(string url, string modelName)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0, 0);
            yield return request.SendWebRequest();
            bundle = DownloadHandlerAssetBundle.GetContent(request);
            Debug.Log(bundle);
            GameObject foodModelAsset = bundle.LoadAsset<GameObject>(modelName);
            foodModel = Instantiate(foodModelAsset, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            Debug.Log(foodModel);
            MainFoodUIScript.SetFoodModel(this);
        }
}
