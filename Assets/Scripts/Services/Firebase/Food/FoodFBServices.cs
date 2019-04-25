using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.DynamicLinks;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FoodFBServices : MonoBehaviour {

    public Text debugger;
    public AssetDownloader assetDownloader;

    // Use this for initialization
    void Start()
    {
        DynamicLinks.DynamicLinkReceived += OnDynamicLink;
    }

    // Display the dynamic link received by the application.
    void OnDynamicLink(object sender, EventArgs args)
    {
        var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
        debugger.text += "Received dynamic link: " +
                        dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString;

        Dictionary<string, string> queryParams = QueryParser(dynamicLinkEventArgs.ReceivedDynamicLink.Url);
        if (queryParams.ContainsKey("menu"))
        {
            switch (queryParams["menu"])
            {
                case "food":
                    if (queryParams.ContainsKey("model"))
                    {
                       string modelId = queryParams["model"];
                       StartCoroutine(GetModelData(modelId));
                    }
                break;
            }
        } 
    }

    public Dictionary<string, string> QueryParser(Uri uri)
    {
        string query = uri.Query.Split('?')[1];
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] parameters = query.Split('&');
        foreach (string s in parameters)
        {
            string[] pairs = s.Split('=');
            dictionary.Add(pairs[0], pairs[1]);
        }
        return dictionary;
    }


    IEnumerator GetModelData(string modelId)
    {
        UnityWebRequest request = UnityWebRequest.Get(Api.AllModels + modelId);
        yield return request.SendWebRequest();
        Model model = JsonUtility.FromJson<Model>(request.downloadHandler.text);
        gameObject.AddComponent<FoodModelConnection>();
        FoodModelConnection foodModelConnection = gameObject.GetComponent<FoodModelConnection>();

        if (Application.platform == RuntimePlatform.Android)
        {
            foodModelConnection.assetBundleUrl = model.prefabLinks.android;
        }
        else
        {
            foodModelConnection.assetBundleUrl = model.prefabLinks.ios;
        }
        foodModelConnection.assetDownloader = assetDownloader;
        foodModelConnection.ButtonClicked();
    }

    [System.Serializable]
    public class Model
    {
        public string name;
        public string category;
        public PrefabLinks prefabLinks;
        public string imageUrl;
    }

    [System.Serializable]
    public class PrefabLinks
    {
        public string ios;
        public string android;
    }
}
