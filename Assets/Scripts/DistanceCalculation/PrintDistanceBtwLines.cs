using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintDistanceBtwLines : MonoBehaviour {

    public GameObject line;
    public GameObject line2;

    public TextMeshProUGUI tmPro;

    private CalculateDistance CD;

    // Use this for initialization
    void Start () {
        CD = GetComponent<CalculateDistance>();
    }
	
	// Update is called once per frame
	void Update () {

        PrintDistanceBtw2Lines();

    }

    public void PrintDistanceBtw2Lines()
    {
        float[] distanceAndIneqSign = CD.CalculateDistanceBtw2Lines(line.transform, line2.transform);

        tmPro.text = distanceAndIneqSign[0].ToString();
    }
}
