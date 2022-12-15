using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRenderController : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField] private Transform anchor;
    [SerializeField] private Transform target;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lineRenderer.SetPositions(new Vector3[] {
            anchor.position,
            target.position,
        });
    }
}
