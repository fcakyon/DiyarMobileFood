using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomClasses;
using TMPro;

public class CalculateDistance : MonoBehaviour
{

    public GameObject line;

    private Vector3 lineRotation;
    private Vector3 linePosition;
    private Vector3 pointPosition;

    private TextMeshProUGUI tmPro;

    public LayerMask planeLayerMask;
    public GameObject CanvasToHide;

    // Use this for initialization
    void Start()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CalculateDistanceForAllScreenPoints()
    {
        CanvasToHide.SetActive(false);
        Debug.Log("burdayım");

        int originalHeight = Camera.main.pixelHeight;
        int originalWidth = Camera.main.pixelWidth;
        int newHeight = 400;
        int newWidth = 200;
        Vector2[,] resizedIndexes = NearestNeightborResize(originalHeight, originalWidth, newHeight, newWidth);

        float[,] pixelDistances = new float[newHeight, newWidth];
        for (int widthIdx = 0; widthIdx < newWidth; widthIdx++)
        {
            for (int heightIdx = 0; heightIdx < newHeight; heightIdx++)
            {
                Vector3 screenPoint = new Vector3(resizedIndexes[heightIdx, widthIdx][1], resizedIndexes[heightIdx, widthIdx][0], 0f);
                //Debug.Log(resizedIndexes[heightIdx, widthIdx][0]+", "+ resizedIndexes[heightIdx, widthIdx][1]);
                pixelDistances[heightIdx, widthIdx] = CalculateDistanceFromScreenPoint(screenPoint);
            }
        }
        CreateJSON.SaveData(pixelDistances);

        CanvasToHide.SetActive(true);
    }

    float CalculateDistanceFromScreenPoint(Vector3 screenPoint)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        if (Physics.Raycast(ray, out hit, 500.0f, planeLayerMask))
        {
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
        lineRotation = line.transform.rotation.eulerAngles;
        float lineAngle = 90 + lineRotation.y;

        linePosition = line.transform.position;
        pointPosition = pointPos;
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

}
