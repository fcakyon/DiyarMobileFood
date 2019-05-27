using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DecorCategoryListInit : MonoBehaviour {

    public GameObject subCategoryPrefab;

    [System.Serializable]
    public class CategoryObject
    {
        public string name;
        public string imageURL;
        public SubCategoryObject[] subCategories;
    }

    [System.Serializable]
    public class SubCategoryObject
    {
        public string name;
        public string imageURL;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(GetCategoriesAndCreateButtons());
    }
    
    // Update is called once per frame
    void Update () {}

    IEnumerator GetCategoriesAndCreateButtons()
    {
        UnityWebRequest request = UnityWebRequest.Get(Api.DecorSubCategories);
        //UnityWebRequest request = UnityWebRequest.Get(Api.DecorModels);
        yield return request.SendWebRequest();

        CategoryObject category = JsonUtility.FromJson<CategoryObject>(request.downloadHandler.text);
        foreach(SubCategoryObject subCategory in category.subCategories)
        {
            GameObject subCategoryClone = Instantiate(subCategoryPrefab);

            subCategoryClone.name = subCategory.name;
            subCategoryClone.SetActive(true);
            subCategoryClone.transform.SetParent(gameObject.transform);
            subCategoryClone.transform.localScale = subCategoryPrefab.transform.localScale;

            subCategoryClone.GetComponent<SubCategoryConnection>().subCategory = subCategory.name;

            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(subCategory.imageURL);
            yield return imageRequest.SendWebRequest();
            Image image = subCategoryClone.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}


