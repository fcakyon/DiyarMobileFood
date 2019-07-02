using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;

public class ScreenshotHandler : MonoBehaviour
{

    private static ScreenshotHandler instance;

    private Camera myCamera;
    private bool takeSSOnNextFrame;

    private XRController xr;
    public static string URL = "http://192.168.0.16:5000/upload_file";

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();

    }

    public void RecordAndSaveFrameTexture()
    {
        xr = GameObject.FindWithTag("XRController").GetComponent<XRController>();
        Material xrMat = gameObject.GetComponent<CustomXRVideoController>().GetMaterial();

        Texture2D src = xr.ShouldUseRealityRGBATexture()
            ? xr.GetRealityRGBATexture()
            : xr.GetRealityYTexture();
            
        RenderTexture renderTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(null, renderTexture, xrMat);

        var dirPath = "Assets/Resources/Snapshots/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        var timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");

        SendTextureAsPNG(renderTexture);
        //SaveTextureAsPNG(renderTexture, dirPath + timeStamp + ".jpg");
        //SaveTextureAsPNG(renderTexture, Application.persistentDataPath + "/texture.png");
    }

    public void SendTextureAsPNG(RenderTexture renderTexture)
    {
        Texture2D aTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        //Debug.Log("aTexture.width:" + aTexture.width + ", " + "aTexture.height:" + aTexture.height);
        RenderTexture.active = renderTexture;
        aTexture.ReadPixels(new Rect(0, 0, aTexture.width, aTexture.height), 0, 0);
        RenderTexture.active = null;


        byte[] bytes = aTexture.EncodeToJPG();
        var timeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        StartCoroutine(SendImage(timeStamp, bytes));
    }

    public static void SaveTextureAsPNG(RenderTexture renderTexture, string aFullPath)
    {
        Texture2D aTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        //Debug.Log("aTexture.width:" + aTexture.width + ", " + "aTexture.height:" + aTexture.height);
        RenderTexture.active = renderTexture;
        aTexture.ReadPixels(new Rect(0, 0, aTexture.width, aTexture.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes = aTexture.EncodeToJPG();
        System.IO.File.WriteAllBytes(aFullPath, bytes);
    }

    private IEnumerator SendImage(string fileName, byte[] data)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName + ".jpg", "multipart/form-data"));

        UnityWebRequest www = UnityWebRequest.Post(URL, formData);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Foto upload complete!");
        }
    }
}