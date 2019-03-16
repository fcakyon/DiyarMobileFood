using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build IOS AssetBundles")]
    static void BuildAllIOSAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles/IOS_AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
                BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
                //BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
    }

    [MenuItem("Assets/Build Android AssetBundles")]
    static void BuildAllAndroidAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Android_AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        //BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);
    }

    [MenuItem("Assets/Build IOS&Android AssetBundles")]
    static void BuildAllIOSAndAndroidAssetBundles()
    {
        BuildAllIOSAssetBundles();
        BuildAllAndroidAssetBundles();
    }

    static void ClearCashe()
    {
    	Caching.ClearCache();
    }
}
