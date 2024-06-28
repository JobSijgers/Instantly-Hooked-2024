using System;
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
        [SerializeField] private int checksInDistance = 20;
        [SerializeField] private float onlyRenderLastDistance = 10;
        private Vector3 originPosition;
        private Vector3 hookPosition;
        private PathCreator path;
        private LineRenderer lineRenderer;
        private float pointInterval;

        private void Start()
        {
            path = gameObject.AddComponent<PathCreator>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            BezierPath newPath = new()
            {
                ControlPointMode = BezierPath.ControlMode.Free, Space = PathSpace.xy, IsClosed = false
            };

            path.bezierPath = newPath;
            pointInterval = onlyRenderLastDistance / checksInDistance;
            lineRenderer.positionCount = checksInDistance + 1;
        }

        private void Update()
        {
            CreateBezier();
            RenderLine();
        }

        private void CreateBezier()
        {
            originPosition = origin.position;
            hookPosition = hook.position;

            path.bezierPath.SetPoint(0, hookPosition - originPosition);
            path.bezierPath.SetPoint(1, hookPosition - originPosition + Vector3.up * (Vector2.Distance(originPosition, hookPosition) * 0.3f));
            path.bezierPath.SetPoint(2, new Vector3(0, -1));
            path.bezierPath.SetPoint(3, Vector3.zero);
        }


        private void RenderLine()
        {
            for (int i = 0; i < checksInDistance; i++)
            {
                lineRenderer.SetPosition(i, path.path.GetPointAtDistance(i * pointInterval, EndOfPathInstruction.Stop));
            }
            lineRenderer.SetPosition(checksInDistance, (Vector2)originPosition);

        }
    }
}