using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACBezier
{
    [ExecuteInEditMode]
    public class BezierCurve : MonoBehaviour
    {

        public int numberOfPointsInGizmos;
        public float giszmosRadius;
        public bool reloading = true;
        public Color color;
        public List<GameObject> points = new List<GameObject>();

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            if (reloading)
            {
                for (int i = 1; i <= numberOfPointsInGizmos; i++)
                {
                    Gizmos.DrawLine(Curve(0, (float)(i - 1) / numberOfPointsInGizmos), Curve(0, (float)i / numberOfPointsInGizmos));
                }
            }
        }

        void Start()
        {

        }

        float CoefficientsObj(int pointIndex)
        {
            float a = Factorial(points.Count - 1) / (Factorial(pointIndex) * Factorial(points.Count - pointIndex - 1));
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

        Vector3 Curve(int i, float t)
        {
            Vector3 a;
            if (i < points.Count)
            {
                a = CoefficientsObj(i) * Mathf.Pow((1f - t), (points.Count - i - 1)) * Mathf.Pow(t, i) * points[i].transform.position + Curve(i + 1, t);

            }
            else a = Vector3.zero;
            return a;
        }

        void Update()
        {

        }
    }

}