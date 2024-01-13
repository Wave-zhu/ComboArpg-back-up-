using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    //min,max,offset
    //layer
    [SerializeField, Header("Max and min offset")] private Vector2 _maxDistanceOffset;
    [SerializeField, Header("Detection layer"), Space(height: 10)]
    private LayerMask _whatIsWall;
    [SerializeField, Header("Ray length"), Space(height: 10)]
    private float _detectionDistance;
    [SerializeField, Header("Collider move smooth time"), Space(height: 10)]
    private float _colliderSmoothTime;

    private Vector3 _originPosition;
    private float _originOffsetDistance;
    private Transform _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main.transform;
    }
    private void Start()
    {
        _originPosition = transform.localPosition.normalized;
        _originOffsetDistance = _maxDistanceOffset.y;
    }
    private void LateUpdate()
    {
        UpdateCameraCollider();
    }

    private void UpdateCameraCollider()
    {
        //move the camera when close to wall
        var detectionDirection = transform.TransformPoint(_originPosition * _detectionDistance);
        if (Physics.Linecast(transform.position, detectionDirection, 
            out var hit, _whatIsWall, QueryTriggerInteraction.Ignore))
        {
            _originOffsetDistance=Mathf.Clamp((hit.distance * 0.8f),_maxDistanceOffset.x,_maxDistanceOffset.y);
        }
        else
        {
            _originOffsetDistance = _maxDistanceOffset.y;
        }
        _mainCamera.localPosition = Vector3.Lerp(_mainCamera.localPosition, _originPosition * (_originOffsetDistance-0.1f),
            DevelopmentToos.UnTetheredLerp(_colliderSmoothTime));
    }
}
