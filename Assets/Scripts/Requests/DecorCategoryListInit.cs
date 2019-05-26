using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DecorCategoryListInit : MonoBehaviour {

    public GameObject modelButtonPrefab;

    [System.Serializable]
    public class Model
    {
        public string name;
        public string category;
        public PrefabLinks prefabLinks;
        public string imageUrl;
        public float height;
        public string info;
    }

    [System.Serializable]
    public class ModelsObjectArray
    {
        public Model[] models;
    }

    [System.Serializable]
    public class PrefabLinks
    {
        public string ios;
        public string android;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(GetCategoriesAndCreateButtons());
    }
    
    // Update is called once per frame
    void Update () {}

    IEnumerator GetCategoriesAndCreateButtons()
    {
        UnityWebRequest request = UnityWebRequest.Get(Api.AllModels);
        //UnityWebRequest request = UnityWebRequest.Get(Api.DecorModels);
        yield return request.SendWebRequest();
        ModelsObjectArray modelsObjectArray = JsonUtility.FromJson<ModelsObjectArray>("{\"models\":" + request.downloadHandler.text + "}");
        foreach(Model model in modelsObjectArray.models)
        {
            GameObject modelButton = Instantiate(modelButtonPrefab);

            if(Application.platform == RuntimePlatform.Android)
                modelButton.GetComponent<DecorModelConnection>().assetBundleUrl = model.prefabLinks.android;
            else
                modelButton.GetComponent<DecorModelConnection>().assetBundleUrl = model.prefabLinks.ios;

            modelButton.GetComponent<DecorModelConnection>().height = model.height;
            modelButton.GetComponent<DecorModelConnection>().info = model.info;

            Transform content = modelButton.transform.Find("Content");

            modelButton.name = model.name;
            modelButton.SetActive(true);
            modelButton.transform.SetParent(gameObject.transform);
            modelButton.transform.localScale = modelButtonPrefab.transform.localScale;

            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(model.imageUrl);
            yield return imageRequest.SendWebRequest();
            Image image = content.Find("Image").GetComponent<Image>();
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}


