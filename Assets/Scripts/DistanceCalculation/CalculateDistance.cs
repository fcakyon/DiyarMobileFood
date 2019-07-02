using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomClasses;
using TMPro;

public class CalculateDistance : MonoBehaviour
{

    public GameObject line;

    private TextMeshProUGUI tmPro;
    private Camera cam;

    public LayerMask planeLayerMask;
    public GameObject CanvasToHide;

    // Use this for initialization
    void Start()
    {
        //tmPro = GetComponent<TextMeshProUGUI>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CalculateDistanceForAllScreenPoints()
    {
        CanvasToHide.SetActive(false);

        int originalHeight = Camera.main.pixelHeight;
        int originalWidth = Camera.main.pixelWidth;
        int newHeight = Camera.main.pixelHeight;
        int newWidth = Camera.main.pixelWidth;
        Vector2[,] resizedIndexes = NearestNeightborResize(originalHeight, originalWidth, newHeight, newWidth);

        float[,] pixelDistances = new float[newHeight, newWidth];
        float[,] pixelPositionsX = new float[newHeight, newWidth];
        float[,] pixelPositionsY = new float[newHeight, newWidth];

        // calculate pixel positions or distances
        for (int widthIdx = 0; widthIdx < newWidth; widthIdx++)
        {
            for (int heightIdx = 0; heightIdx < newHeight; heightIdx++)
            {
                Vector3 screenPoint = new Vector3(resizedIndexes[heightIdx, widthIdx][1], resizedIndexes[heightIdx, widthIdx][0], 0f);
                //Debug.Log(resizedIndexes[heightIdx, widthIdx][0]+", "+ resizedIndexes[heightIdx, widthIdx][1]);
                //pixelDistances[heightIdx, widthIdx] = CalculateDistanceFromScreenPoint(screenPoint);
                Vector3 pixelPosition = CalculatePositionFromScreenPoint(screenPoint);
                pixelPositionsX[heightIdx, widthIdx] = pixelPosition.x;
                pixelPositionsY[heightIdx, widthIdx] = pixelPosition.z;
            }
        }

        // calculate line parameters
        Vector3 lineRotation = line.transform.rotation.eulerAngles;
        float lineAngle = 90 + lineRotation.y;
        Vector3 linePosition = line.transform.position;

        // send photo as jpg
        cam.GetComponent<ScreenshotHandler>().RecordAndSaveFrameTexture();

        // send pixel distances as json
        //Debug.Log("pixelDistancesSize: " + pixelDistances.GetLength(0) + ", " + pixelDistances.GetLength(1));
        //cam.GetComponent<CreateJSON>().SendDistancedataAsJSON(pixelDistances);

        // send pixel positions as jsons
        cam.GetComponent<CreateJSON>().SendPositiondataAsJSON(pixelPositionsX, pixelPositionsY, linePosition, lineAngle);

        CanvasToHide.SetActive(true);
    }

    Vector3 CalculatePositionFromScreenPoint(Vector3 screenPoint)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        if (Physics.Raycast(ray, out hit, 500.0f, planeLayerMask))
        {
            return hit.point;
        }
        else
        {
            return new Vector3(0,0,0);
        }

    }

    float CalculateDistanceFromScreenPoint(Vector3 screenPoint)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        if (Physics.Raycast(ray, out hit, 500.0f, planeLayerMask))
        {
            // calculate distance from reference line to hitpoint 
            float[] distanceAndIneqSign = CalculatePositionDistance(hit.point);
            return distanceAndIneqSign[0];
        }
        else
        {
            return -99f;
        }

    }

    public float[] CalculatePositionDistance(Vector3 pointPos)
    {
        Vector3 lineRotation = line.transform.rotation.eulerAngles;
        float lineAngle = 90 + lineRotation.y;

        Vector3 linePosition = line.transform.position;
        Vector3 pointPosition = pointPos;
        float[] lineCenterCoordinates = { linePosition.z, linePosition.x };
        float[] pointCenterCoordinates = { pointPosition.z, pointPosition.x };

        GeometricFunctions geometricFunctions = new GeometricFunctions();
        float[] distanceAndIneqSign = geometricFunctions.CalculateDistanceAndIneqSign(lineAngle, lineCenterCoordinates, pointCenterCoordinates);

        return distanceAndIneqSign;
    }


    public static Vector2[,] NearestNeightborResize(int originalHeight, int originalWidth, int newHeight, int newWidth)
    {
        Vector2[,] resizedIndexes = new Vector2[newHeight, newWidth];

        int hRatio = (int)((originalHeight << 16) / newHeight) + 1;
        int wRatio = (int)((originalWidth << 16) / newWidth) + 1;

        int hIndex, wIndex;

        for (int i = 0; i < newHeight; i++)
        {
            for (int j = 0; j < newWidth; j++)
            {
                hIndex = (i * hRatio) >> 16;
                wIndex = (j * wRatio) >> 16;
                resizedIndexes[i, j] = new Vector2(hIndex, wIndex);

            }
        }

        return resizedIndexes;
        }

    public float[] CalculateDistanceBtw2Lines(Transform line1, Transform line2)
    {
        Vector3 line1Rotation = line1.transform.rotation.eulerAngles;
        float line1Angle = 90 + line1Rotation.y;

        Vector3 line1Position = line1.transform.position;
        Vector3 line2Position = line2.transform.position;
        float[] line1CenterCoordinates = { line1Position.z, line1Position.x };
        float[] line2CenterCoordinates = { line2Position.z, line2Position.x };

        GeometricFunctions geometricFunctions = new GeometricFunctions();
        float[] distanceAndIneqSign = geometricFunctions.CalculateDistanceAndIneqSign(line1Angle, line1CenterCoordinates, line2CenterCoordinates);


        return distanceAndIneqSign;
    }

}
