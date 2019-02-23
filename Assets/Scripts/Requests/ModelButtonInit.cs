using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModelButtonInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button button = gameObject.GetComponent<Button>();
        FoodModelConnection foodModelConnection = gameObject.GetComponent<FoodModelConnection>();
        button.onClick.AddListener(foodModelConnection.ButtonClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}