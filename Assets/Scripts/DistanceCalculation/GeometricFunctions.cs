using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses
{
    public class GeometricFunctions
    {
        public Vector3 cameraRotation;
        public float slope;
        public float[] lineCoefficients;
        public float distanceFromPortal;
        public float inequalitySign;

        public float[] CalculateLineCoefficientsFromAngleAndCenterCoordinates(float objectAngle, float[] objectCenterCoordinates)
        {
            float[] lineCoefficientsTemp = new float[] { 0, 0, 0 };
            slope = Mathf.Tan(objectAngle / 180 * Mathf.PI);
            lineCoefficientsTemp[2] = objectCenterCoordinates[1] - slope * objectCenterCoordinates[0]; // c
            lineCoefficientsTemp[1] = -lineCoefficientsTemp[2] / (-slope * objectCenterCoordinates[0] + objectCenterCoordinates[1]); // y
            lineCoefficientsTemp[0] = -lineCoefficientsTemp[1] * slope; // x
            //Debug.Log("slope: "+slope+" lineCoefficients: "+lineCoefficients[0]+" "+lineCoefficients[1]+ " " + lineCoefficients[2]);

            return lineCoefficientsTemp;
        }

        public float[] CalculateDistanceAndIneqSignFromPointToLine(float[] pointCoordinates, float[] lineCoefficients)
        {
            float numerator = Mathf.Abs(lineCoefficients[0] * pointCoordinates[0] + lineCoefficients[1] * pointCoordinates[1] + lineCoefficients[2]);
            float denumerator = Mathf.Sqrt(Mathf.Pow(lineCoefficients[0], 2) + Mathf.Pow(lineCoefficients[1], 2));
            inequalitySign = Mathf.Sign(lineCoefficients[0] * pointCoordinates[0] + lineCoefficients[1] * pointCoordinates[1] + lineCoefficients[2]);
            return new float[] { numerator / denumerator, inequalitySign };
        }

        public float[] CalculateDistanceAndIneqSign(float objectAngle, float[] objectCenterCoordinates, float[] pointCenterCoordinates)
        {
            lineCoefficients = CalculateLineCoefficientsFromAngleAndCenterCoordinates(objectAngle, objectCenterCoordinates);
            float[] distanceFromPortalAndIneqSign = CalculateDistanceAndIneqSignFromPointToLine(pointCenterCoordinates, lineCoefficients);
            //Debug.Log("distance: " + distanceFromPortal );
            return distanceFromPortalAndIneqSign;
        }

    }
}