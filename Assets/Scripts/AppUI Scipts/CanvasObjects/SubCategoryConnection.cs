using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCategoryConnection : MonoBehaviour {

    [HideInInspector]
    public string subCategory;
    public DecorModelLister decorModelLister;

    // Use this for initialization
    void Start () {
	}

	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    void OnClick ()
    {
        decorModelLister.GetModelsBySubCategory(subCategory);
    }
}
