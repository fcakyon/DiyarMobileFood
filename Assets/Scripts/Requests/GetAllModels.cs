using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetAllModels : MonoBehaviour {

    [System.Serializable]
    public class Model
    {
        public string name;
        public string category;
        public string prefabLink;
    }

    [System.Serializable]
    public class ModelsObject
    {
        public Model[] models;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(GetModels());
    }
	
	// Update is called once per frame
	void Update () {}

    IEnumerator GetModels()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://diyar-server.herokuapp.com/v0/models/");
        yield return request.SendWebRequest();
        Debug.Log("{\"models\":" + request.downloadHandler.text + "}");
        ModelsObject modelsObject = JsonUtility.FromJson<ModelsObject>("{\"models\":" + request.downloadHandler.text + "}");
        Debug.Log(modelsObject.models[0].name);
    }
}

