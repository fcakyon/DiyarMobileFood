using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPS : MonoBehaviour {

    float deltaTime;
    private TextMeshProUGUI tmPro;

// Use this for initialization
    void Start () {
        tmPro = GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        tmPro.text = Mathf.Ceil(fps).ToString();
    }
}
