using System.Collections.Generic;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;

namespace FishingRod
{
    [RequireComponent(typeof(LineRenderer))]
    public class RodRenderer : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform hook;
        [SerializeField] private float pointInterval;
        private PathCreator path;
        private LineRenderer lineRenderer;
        private const float MaxLineRenderDistance = 10000;
        private void Start()
        {
            path = gameObject.AddComponent<PathCreator>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            BezierPath newPath = new()
            {
                ControlPointMode = BezierPath.ControlMode.Free,
                Space = PathSpace.xy,
                IsClosed = false
            };
            
            path.bezierPath = newPath;
        }
        
        private void Update()
        {
            CreateBezier();
            RenderLine(GetLinePoints());
        }

        private void CreateBezier()
        {
            Vector3 originPosition = origin.position;
            Vector3 hookPosition = hook.position;

            path.bezierPath.SetPoint(0, Vector3.zero);
            path.bezierPath.SetPoint(1, Vector3.zero + new Vector3(0, -1));
            path.bezierPath.SetPoint(2, hookPosition - originPosition + Vector3.up * (Vector2.Distance(originPosition, hookPosition) * 0.3f));
            path.bezierPath.SetPoint(3, hookPosition - originPosition);
        }

        private List<Vector3> GetLinePoints()
        {
            List<Vector3> linePositions = new();
            Vector2 lastLocation = Vector3.positiveInfinity;
            float checks = MaxLineRenderDistance / pointInterval;
            
            for (int i = 0; i < checks; i++)
            {   
                Vector2 newLocation = path.path.GetPointAtDistance(i * pointInterval, EndOfPathInstruction.Stop);
                if (newLocation == lastLocation)
                {
                    return linePositions;
                }

                linePositions.Add(newLocation);
                lastLocation = newLocation;
            }     
            return linePositions;
        }

        private void RenderLine(List<Vector3> points)
        {
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
    }
}