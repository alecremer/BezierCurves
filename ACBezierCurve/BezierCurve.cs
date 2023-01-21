using System.Numerics;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vector3 = UnityEngine.Vector3;

namespace BezierCurves
{
    public class BakedPoint
    {

        public Vector3 point; 
        public Vector3 tangent;
        public Vector3 normal;

    }
    
    [ExecuteInEditMode]
    public class BezierCurve : MonoBehaviour
    {

        [System.Serializable]
        public class ControlPoint
        {
            public GameObject point;
            public Color color;
        
        }

        

        public List<ControlPoint> controlPoints;

        public int curveDebuggerResolution;
        public float pointsDebugRadius = 1f;
        public bool reloading = true;
        public Color lineColor = Color.red;
        public Color tangentColor = Color.yellow;
        public float tangentLenght = 1;
        public Color normalColor = Color.blue;
        public float normalLenght = 1;
        public Color viewPointColor = Color.green;
        public float viewPointT = 0;
        public float viewPointRadius = 0;

        // Called when scene visual debugger are activated
        private void OnDrawGizmos()
        {
            Gizmos.color = lineColor;



            if (reloading)
            {
                
                Vector3 lastPoint = Vector3.zero;
                Vector3 lastTangent = Vector3.zero;

                for (int i = 1; i <= curveDebuggerResolution; i++)
                {
                    
                    Gizmos.color = lineColor;
                
                    lastPoint = Curve((float)(i - 1) / curveDebuggerResolution);
                    Vector3 currentPoint = Curve((float)i / curveDebuggerResolution);

                    Gizmos.DrawLine(lastPoint, currentPoint);


                    // Draw tangent

                    Gizmos.color = tangentColor;

                    Vector3 tangentVector = (currentPoint - lastPoint).normalized;

                    Gizmos.DrawLine(currentPoint, currentPoint + tangentVector * tangentLenght);

                    
                    
                    // Draw normal 

                    Gizmos.color = normalColor;

                    Vector3 normalVector = (tangentVector - lastTangent).normalized;

                    Gizmos.DrawLine(currentPoint, currentPoint + normalVector.normalized * normalLenght);

                    lastTangent = tangentVector;
                
                }

                Gizmos.color = viewPointColor;
                Gizmos.DrawSphere(Curve(viewPointT), viewPointRadius);
            }
            
            for(int i=0; i<controlPoints.Count; i++)
            {

                try
                {
                    Gizmos.color = controlPoints[i].color;
                    Gizmos.DrawSphere(controlPoints[i].point.transform.position, pointsDebugRadius);
                }
                catch{}
            

            } 
        }


        float PolynomialCoefficient(int pointIndex, List<ControlPoint> controlPointsCurrent)
        {
            float a = Factorial(controlPointsCurrent.Count - 1) / (Factorial(pointIndex) * Factorial(controlPointsCurrent.Count - pointIndex - 1));
            return a;
        }


        float Factorial(float n)
        {
            try
            {
                if (n > 1)
                    return n * Factorial(n - 1);
                else return 1;
            }
            catch { Debug.LogError("n must be greater than -1"); return 1; }
        }


        public Vector3 Curve(float curveStep, int controlPointIndex = 0)
        {
            Vector3 a;



            if (controlPointIndex < controlPoints.Count)
            {

                // A = (Sum from 0 to points.count) Coefficient * (1 - curveStep) ** (points.count -i -1) * point
                a = PolynomialCoefficient(controlPointIndex, controlPoints) * Mathf.Pow((1f - curveStep), (controlPoints.Count - controlPointIndex - 1)) * Mathf.Pow(curveStep, controlPointIndex) * controlPoints[controlPointIndex].point.transform.position + Curve(curveStep, controlPointIndex + 1);

            }

            else a = Vector3.zero;
            return a;
        }


        public List<BakedPoint> BakeCurve(float curveStepLenght)
        {

            List<Vector3> curveBaked = new List<Vector3>();
            List<BakedPoint> bakedCurve = new List<BakedPoint>();

            Vector3 currentPoint;
            Vector3 lastPoint = Vector3.zero;
            Vector3 lastTangent = Vector3.zero;


            for(float curveStep = 0; curveStep < controlPoints.Count; curveStep += curveStepLenght)
            {

                BakedPoint bakedPoint = new BakedPoint();
                currentPoint = Curve(curveStep/controlPoints.Count);
                curveBaked.Add(currentPoint);

                bakedPoint.point = currentPoint;

                // tangent

                Vector3 tangentVector = (currentPoint - lastPoint).normalized;

                bakedPoint.tangent = tangentVector;

                // normal 

                Vector3 normalVector = (tangentVector - lastTangent).normalized;

                bakedPoint.normal = normalVector;

                bakedCurve.Add(bakedPoint);

                lastPoint = currentPoint;
                lastTangent = tangentVector;

            }


            return bakedCurve;

        }

    }

}