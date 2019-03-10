using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetDownloader : MonoBehaviour {

    public FoodManager foodManager;
    public DecorManager decorManager;
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
        if(foodManager.foodModelConnection != foodModelConnection) {
            if (coroutine != null) StopCoroutine(coroutine);
            if (request != null && !request.isDone) request.Abort();
            coroutine = DownloadAssetBundleAndSetFoodModel(foodModelConnection);
            foodManager.HideFoodModel();
            foodManager.RemoveFoodModelConnection();
            foodManager.SetFoodModel(foodModelConnection);
            StartCoroutine(coroutine);
        }
    }

    public void ModelButtonClickHandler(DecorModelConnection decorModelConnection)
    {
        if (decorManager.decorModelConnection != decorModelConnection)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            if (request != null && !request.isDone) request.Abort();
            coroutine = DownloadAssetBundleAndSetDecorModel(decorModelConnection);
            decorManager.RemoveDecorModelConnection();
            decorManager.SetDecorModel(decorModelConnection);
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

    IEnumerator DownloadAssetBundleAndSetDecorModel(DecorModelConnection decorModelConnection)
    {
        request = UnityWebRequestAssetBundle.GetAssetBundle(decorModelConnection.assetBundleUrl, 0, 0);
        yield return request.SendWebRequest();
        decorModelConnection.bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject foodModelAsset = decorModelConnection.bundle.LoadAsset<GameObject>(decorModelConnection.prefabName);
        decorModelConnection.decorModel = Instantiate(foodModelAsset, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }
}
