using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ModelListInit : MonoBehaviour {

    public GameObject modelButtonPrefab;

    [System.Serializable]
    public class Model
    {
        public string name;
        public string category;
        public string prefabLink;
        public string imageUrl;
    }

    [System.Serializable]
    public class ModelsObjectArray
    {
        public Model[] models;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(GetModelsAndCreateButtons());
    }
	
	// Update is called once per frame
	void Update () {}

    IEnumerator GetModelsAndCreateButtons()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://diyar-server.herokuapp.com/v0/models/");
        yield return request.SendWebRequest();
        ModelsObjectArray modelsObjectArray = JsonUtility.FromJson<ModelsObjectArray>("{\"models\":" + request.downloadHandler.text + "}");
        foreach(Model model in modelsObjectArray.models)
        {
            GameObject modelButton = Instantiate(modelButtonPrefab);

            modelButton.GetComponent<FoodModelConnection>().assetBundleUrl = model.prefabLink;
            modelButton.GetComponent<FoodModelConnection>().prefabName = model.name;

            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(model.imageUrl);
            yield return imageRequest.SendWebRequest();

            Transform content = modelButton.transform.Find("Content");
            Text text = content.Find("Text").GetComponent<Text>();
            text.text = model.name;
            Image image = content.Find("Image").GetComponent<Image>();
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            modelButton.name = model.name;
            modelButton.SetActive(true);
            modelButton.transform.SetParent(gameObject.transform);
            modelButton.transform.localScale = Vector3.one;
        }
    }
}


