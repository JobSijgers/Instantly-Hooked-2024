using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace FishingRod
{
    [RequireComponent(typeof(LineRenderer))]
    public class RodRenderer : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform hook;
        [SerializeField] private float pointInterval;
        private PathCreator _path;
        private LineRenderer _lineRenderer;
        private const float MaxLineRenderDistance = 200;
        private void Start()
        {
            _path = gameObject.AddComponent<PathCreator>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            CreateBezier();
            RenderLine(GetLinePoints());
        }

        private void CreateBezier()
        {
            BezierPath path = new BezierPath
            {
                ControlPointMode = BezierPath.ControlMode.Free,
                Space = PathSpace.xy,
                IsClosed = false
            };

            var originPosition = origin.position;
            var hookPosition = hook.position;

            path.SetPoint(0, Vector3.zero);
            path.SetPoint(1, Vector3.zero + new Vector3(0, -1));
            path.SetPoint(2, hookPosition - originPosition + Vector3.up * (Vector2.Distance(originPosition, hookPosition) * 0.3f));
            path.SetPoint(3, hookPosition - originPosition);
            _path.bezierPath = path;
        }

        private Vector3[] GetLinePoints()
        {
            List<Vector3> linePositions = new();
            Vector3 lastLocation = Vector3.positiveInfinity;
            float checks = MaxLineRenderDistance / pointInterval;
            
            for (int i = 0; i < checks; i++)
            {   
                Vector3 newLocation = _path.path.GetPointAtDistance(i * pointInterval, EndOfPathInstruction.Stop);
                if (newLocation == lastLocation)
                {
                    return linePositions.ToArray();
                }

                linePositions.Add(newLocation);
                lastLocation = newLocation;
            }     
            return linePositions.ToArray();
        }

        private void RenderLine(Vector3[] points)
        {
            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
        }
    }
}