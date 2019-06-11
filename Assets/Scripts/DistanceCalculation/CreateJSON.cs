using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;

public class CreateJSON : MonoBehaviour {

    public static float[] data;
    public float[][] dataMulti;

    // Use this for initialization
    void Start () {

    }
	

	// Update is called once per frame
	void Update () {
		
	}


    public static void SaveData(float[,] inputData)
    {
        data = new float[inputData.GetLength(0) * inputData.GetLength(1)];
        for (int i = 0; i < inputData.GetLength(0); i++)
        {
            for (int j = 0; j < inputData.GetLength(1); j++)
            {
                data[i * inputData.GetLength(1) + j] = inputData[i,j];
            }
        }
        ToJSON();
    }

    static void WriteString(float[][] data)
    {
        string path = "Assets/Resources/data.json";
        string result = "{\"data\": [";

        for (int i = 0; i < data.Length; i++)
        {
            result += "[";
            for (int j = 0; j < data[i].Length; j++)
            {
                result += data[i][j] + ", ";
            }
            result = result.Substring(0, result.Length - 2);
            result += "], ";
        }
        result = result.Substring(0, result.Length - 2);
        result += "]}";

        //Write some text to the file at path
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(result);
        writer.Close();

        ////Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load<TextAsset>("data");
    }

    static void WriteString(float[,] data)
    {
        string path = "Assets/Resources/data.json";
        string result = "{\"data\": [";

        for (int i = 0; i < data.GetLength(0); i++)
        {
            result += "[";
            for (int j = 0; j < data.GetLength(1); j++)
            {
                result += data[i, j] + ", ";
            }
            result = result.Substring(0, result.Length - 2);
            result += "], ";
        }
        result = result.Substring(0, result.Length - 2);
        result += "]}";

        //Write some text to the file at path
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(result);
        writer.Close();

        ////Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load<TextAsset>("data");
    }

    public class ToJsonObject
    {
      public float[] data;
    }


    static void ToJSON()
    {
        string path = "Assets/Resources/data.json";

        ToJsonObject toJsonObject = new ToJsonObject();
        toJsonObject.data = data;

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(JsonUtility.ToJson(toJsonObject));
        writer.Close();
    }
}
