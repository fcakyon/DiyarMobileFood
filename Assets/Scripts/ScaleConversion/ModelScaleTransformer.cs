using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelScaleTransformer : MonoBehaviour {

    public static float newModelScale;

    public static void ResetModelScaleAR(GameObject model, float desiredRealWorldHeight) 
    {
        // reset parent scale
        model.transform.localScale = new Vector3(1, 1, 1);

        // calculate model size in unity units
        Vector3 scaledUnitySize = model.transform.GetChild(0).GetComponent<BoxCollider>().bounds.size;
        //Debug.Log("scaledUnitySize: " + scaledUnitySize);

        // calculate scale factor
        float unityUnitToCm = 100;
        float desiredUnityHeight = desiredRealWorldHeight / unityUnitToCm;
        float scaleFactor = desiredUnityHeight / scaledUnitySize.y;
        //Debug.Log("desiredRealWorldHeight: " + desiredRealWorldHeight);
        //Debug.Log("desiredUnityHeight: " + desiredUnityHeight);
        //Debug.Log("scaleFactor: " + scaleFactor);

        // update parent scale
        Vector3 desiredScale = model.transform.localScale * scaleFactor;
        model.transform.localScale = desiredScale;
    }

    public static void ResetModelScale3D(GameObject model)
    {
        // reset parent scale
        model.transform.localScale = new Vector3(1, 1, 1);

        // calculate model size in unity units
        Vector3 scaledUnitySize = model.transform.GetChild(0).GetComponent<BoxCollider>().bounds.size;
        //Debug.Log("scaledUnitySize: " + scaledUnitySize);

        // calculate scale factor
        Vector3 maxUnitySize = new Vector3(0.22147614f, 0.22358226f, 0.332977946f);
        Vector3 unitySizeRatio = Vector3.Scale(new Vector3(1/ scaledUnitySize.x, 1/ scaledUnitySize.y, 1/ scaledUnitySize.z), maxUnitySize);
        float scaleFactor = Mathf.Min(unitySizeRatio.x, unitySizeRatio.y, unitySizeRatio.z);
        //Debug.Log("scaleFactor: "+ scaleFactor);

        // update parent scale
        Vector3 desiredScale = model.transform.localScale * scaleFactor;
        model.transform.localScale = desiredScale;
    }

    public static void CustomModelScale3D(GameObject model, float modelScale)
    {
        // reset parent scale
        model.transform.localScale = new Vector3(1, 1, 1);

        // calculate model size in unity units
        Vector3 scaledUnitySize = model.transform.GetChild(0).GetComponent<BoxCollider>().bounds.size;
        //Debug.Log("scaledUnitySize: " + scaledUnitySize);

        // check if desired scale is suitable
        if (modelScale > 0.7 && modelScale < 1.8)
        {
            newModelScale = modelScale;
        }
        else if (modelScale < 0.7)
        {
            // dont let model to be scaled less than 0.7 times
            newModelScale = 0.7f;
        }
        else if (modelScale > 1.8f)
        {
            // dont let model to be scaled greater than 1.8 times
            newModelScale = 1.8f;
        }

        // calculate scale factor
        Vector3 desiredUnitySize = newModelScale * new Vector3(0.22147614f, 0.22358226f, 0.332977946f);
        Vector3 unitySizeRatio = Vector3.Scale(new Vector3(1 / scaledUnitySize.x, 1 / scaledUnitySize.y, 1 / scaledUnitySize.z), desiredUnitySize);
        float scaleFactor = Mathf.Min(unitySizeRatio.x, unitySizeRatio.y, unitySizeRatio.z);
        //Debug.Log("scaleFactor: "+ scaleFactor);

        // update parent scale
        Vector3 desiredScale = model.transform.localScale * scaleFactor;
        model.transform.localScale = desiredScale;
    }

}
