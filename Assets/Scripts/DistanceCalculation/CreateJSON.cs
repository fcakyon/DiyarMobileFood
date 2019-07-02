using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class CreateJSON : MonoBehaviour {

    public static float[] data;
    public float[][] dataMulti;
    public static string URL = "http://192.168.0.16:5000/upload_file";

    // Use this for initialization
    void Start () {

    }
	

	// Update is called once per frame
	void Update () {
		
	}

    public void SendPositiondataAsJSON(float[,] pixelPositionsX2D, float[,] pixelPositionsY2D, Vector3 linePositionVector, float lineAngle)
    {
        // convert vector to 1d float array
        float[] linePosition = new float[3]; 
        linePosition[0] = linePositionVector.x;
        linePosition[1] = linePositionVector.y;
        linePosition[2] = linePositionVector.z;

        // convert 2d float arrays to 1d float arrays
        float[] pixelPositionsX1D = Convert1Dfrom2DFloat(pixelPositionsX2D);
        float[] pixelPositionsY1D = Convert1Dfrom2DFloat(pixelPositionsY2D);

        SendPixelPositionsAsJSON(pixelPositionsX1D, pixelPositionsY1D, linePosition, lineAngle);
    }

    private float[] Convert1Dfrom2DFloat(float[,] inputData)
    {
        float[] rearrangedInputData = new float[inputData.GetLength(0) * inputData.GetLength(1)];
        for (int i = 0; i < inputData.GetLength(0); i++)
        {
            for (int j = 0; j < inputData.GetLength(1); j++)
            {
                rearrangedInputData[i * inputData.GetLength(1) + j] = inputData[i, j];
            }
        }
        return rearrangedInputData;
    }

    public void SendDistancedataAsJSON(float[,] inputData)
    {
        data = new float[inputData.GetLength(0) * inputData.GetLength(1)];
        for (int i = 0; i < inputData.GetLength(0); i++)
        {
            for (int j = 0; j < inputData.GetLength(1); j++)
            {
                data[i * inputData.GetLength(1) + j] = inputData[i,j];
            }
        }
        SendPixelDistancesAsJSON();
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

    public class PixelDistancesObject
    {
      public float[] data;
    }

    public class PixelPositionsObject
    {
        public float[] pixelPositionsX;
        public float[] pixelPositionsY;
        public float[] linePosition;
        public float lineAngle;
    }

    static void ToJSON()
    {
        string path = "Assets/Resources/data.json";

        PixelDistancesObject jsonObject = new PixelDistancesObject();
        jsonObject.data = data;

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(JsonUtility.ToJson(jsonObject));
        writer.Close();
    }

    public void SendPixelPositionsAsJSON(float[] pixelPositionsX, float[] pixelPositionsY, float[] linePosition, float lineAngle)
    {

        PixelPositionsObject jsonObject = new PixelPositionsObject();
        jsonObject.pixelPositionsX = pixelPositionsX;
        jsonObject.pixelPositionsY = pixelPositionsY;
        jsonObject.linePosition = linePosition;
        jsonObject.lineAngle = lineAngle;

        var timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonObject));

        StartCoroutine(SendJSON(timeStamp, bytes));
    }

    public void SendPixelDistancesAsJSON()
    {

        PixelDistancesObject jsonObject = new PixelDistancesObject();
        jsonObject.data = data;

        var timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonObject));

        StartCoroutine(SendJSON(timeStamp, bytes));
    }

    public IEnumerator SendJSON(string fileName, byte[] data)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName + ".json", "multipart/form-data"));

        UnityWebRequest www = UnityWebRequest.Post(URL, formData);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Distancedata upload complete!");
        }
    }
}
