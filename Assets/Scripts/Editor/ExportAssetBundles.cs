using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build IOS AssetBundles")]
    static void BuildAllIOSAssetBundles()
    {
        string assetBundleDirectory = "Assets/IOS_AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    [MenuItem("Assets/Build Android AssetBundles")]
    static void BuildAllAndroidAssetBundles()
    {
        string assetBundleDirectory = "Assets/Android_AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    static void ClearCashe()
    {
    	Caching.ClearCache();
    }
}
