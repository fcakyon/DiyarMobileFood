using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetDownloader : MonoBehaviour {

    private IEnumerator coroutine;
    private UnityWebRequest request;

    void Start () {
        Caching.ClearCache();
    }
	
	void Update () {}

    public void ModelSelectHandler(FoodModelConnection foodModelConnection)
    {
        if(FoodManager.Instance.FoodModelConnection == null ||
            FoodManager.Instance.FoodModelConnection.assetBundleUrl != foodModelConnection.assetBundleUrl) 
        {
            if (coroutine != null) StopCoroutine(coroutine);
            if (request != null && !request.isDone) request.Abort();
            coroutine = DownloadAssetBundleAndSetFoodModel();
            FoodManager.Instance.RemoveConnection();
            FoodManager.Instance.FoodModelConnection = foodModelConnection;
            if (FoodManager.Instance.hasFoodModelBeenPlaced == true)
                FoodManager.Instance.isChanging = true;
            StartCoroutine(coroutine);
        }
    }

    public void ModelSelectHandler(DecorModelConnection decorModelConnection)
    {
        if (DecorManager.Instance.DecorModelConnection == null ||
            DecorManager.Instance.DecorModelConnection.assetBundleUrl != decorModelConnection.assetBundleUrl)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            if (request != null && !request.isDone) request.Abort();
            coroutine = DownloadAssetBundleAndSetDecorModel();
            if (DecorManager.Instance.is3DScene == true) DecorManager.Instance.RemoveConnection();
            DecorManager.Instance.DecorModelConnection = decorModelConnection;
            StartCoroutine(coroutine);
        }
    }

    IEnumerator DownloadAssetBundleAndSetFoodModel()
    {
        FoodManager.Instance.UiState = (int)FoodManager.UIStatesEnum.Loading;
        request = UnityWebRequestAssetBundle.GetAssetBundle(FoodManager.Instance.FoodModelConnection.assetBundleUrl, 0, 0);
        yield return request.SendWebRequest();
        FoodManager.Instance.FoodModelConnection.bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject foodModelAsset = FoodManager.Instance.FoodModelConnection.bundle.LoadAsset<GameObject>(FoodManager.Instance.FoodModelConnection.prefabName);
        GameObject foodModel = Instantiate(foodModelAsset, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        DontDestroyOnLoad(foodModel);
        FoodManager.Instance.FoodModelConnection.foodModel = foodModel;
        if(FoodManager.Instance.is3DScene) FoodManager.Instance.UiState = (int)FoodManager.UIStatesEnum.Idle;
        else FoodManager.Instance.UiState = (int)FoodManager.UIStatesEnum.AutoPlace;
    }

    IEnumerator DownloadAssetBundleAndSetDecorModel()
    {
        DecorManager.Instance.UiState = (int)DecorManager.UIStatesEnum.Loading;
        request = UnityWebRequestAssetBundle.GetAssetBundle(DecorManager.Instance.DecorModelConnection.assetBundleUrl, 0, 0);
        yield return request.SendWebRequest();
        DecorManager.Instance.DecorModelConnection.bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject decorModelAsset = DecorManager.Instance.DecorModelConnection.bundle.LoadAsset<GameObject>(DecorManager.Instance.DecorModelConnection.prefabName);
        GameObject decorModel = Instantiate(decorModelAsset, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        DontDestroyOnLoad(decorModel);
        DecorManager.Instance.DecorModelConnection.decorModel = decorModel;
        DecorManager.Instance.AddModelToDict(DecorManager.Instance.DecorModelConnection.decorModel, DecorManager.Instance.DecorModelConnection);
        if (DecorManager.Instance.is3DScene) DecorManager.Instance.UiState = (int)DecorManager.UIStatesEnum.Idle;
        else DecorManager.Instance.UiState = (int)DecorManager.UIStatesEnum.AutoPlace;
    }
}
