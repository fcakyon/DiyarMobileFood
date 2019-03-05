using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetDownloader : MonoBehaviour {

    public MainFoodUI MainFoodUIScript;
    private IEnumerator coroutine;
    UnityWebRequest request;


    // Use this for initialization
    void Start () {
        Caching.ClearCache();
    }
	
	// Update is called once per frame
	void Update () {}

    public void ModelButtonClickHandler(FoodModelConnection foodModelConnection)
    {
        if(MainFoodUIScript.foodModelConnection != foodModelConnection) {
            if (coroutine != null) StopCoroutine(coroutine);
            if (request != null && !request.isDone) request.Abort();
            coroutine = DownloadAssetBundleAndSetFoodModel(foodModelConnection);
            MainFoodUIScript.HideFoodModel();
            MainFoodUIScript.RemoveFoodModelConnection();
            MainFoodUIScript.SetFoodModel(foodModelConnection);
            StartCoroutine(coroutine);
        }
    }


    IEnumerator DownloadAssetBundleAndSetFoodModel(FoodModelConnection foodModelConnection)
    {
        request = UnityWebRequestAssetBundle.GetAssetBundle(foodModelConnection.assetBundleUrl, 0, 0);
        yield return request.SendWebRequest();
        foodModelConnection.bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject foodModelAsset = foodModelConnection.bundle.LoadAsset<GameObject>(foodModelConnection.prefabName);
        foodModelConnection.foodModel = Instantiate(foodModelAsset, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }
}
