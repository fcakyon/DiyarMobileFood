using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadModelAssetBundles : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        StartCoroutine(InstantiateObject());
    }

    IEnumerator InstantiateObject()

    {
        string uri = "https://s3.eu-west-3.amazonaws.com/diyar-mobile-food-models/kebab"; 
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        Debug.Log(bundle);
        GameObject pasta = bundle.LoadAsset<GameObject>("3d kebab");
        //GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        Instantiate(pasta);
        //Instantiate(sprite);
    }
}
