using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TP_CameraControl : MonoBehaviour
{
    [SerializeField, Header("Camera Settings")] private float _controlSpeed;
    [SerializeField] private Vector2 _cameraVerticalMaxAngle;//restrict angle when rotate up and down
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private float _positionOffset;
    [SerializeField] private float _positionSmoothTime;

    private Vector3 _smoothDampVelocity = Vector3.zero;
    private Vector2 _input;
    private Vector3 _cameraRotation;
    private GameObject _target;
    private Transform _lookTarget;
    private Transform _currentLookTarget;
    private bool _isExecution;
    private void Awake()
    {
        _target = GameObject.FindWithTag("CameraTarget");
        _lookTarget = _target.transform;
        _currentLookTarget = _lookTarget;
    }
    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListener<Transform,float>("SetExecutionTarget", SetExecutionTarget);
    }
    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<Transform, float>("SetExecutionTarget", SetExecutionTarget);
    }
    private void Update()
    {
        CameraInput();
    }
    private void LateUpdate()
    {
        UpdateCameraRotation();
        CameraPosition();
    }

    private void CameraInput()
    {
        _input.y += GameInputManager.MainInstance.CameraLook.x * _controlSpeed;
        _input.x -= GameInputManager.MainInstance.CameraLook.y * _controlSpeed;
        _input.x = Mathf.Clamp(_input.x, _cameraVerticalMaxAngle.x, _cameraVerticalMaxAngle.y);
        _positionOffset -= GameInputManager.MainInstance.Zoom / 240f*0.5f;
    }
    
    private void UpdateCameraRotation()
    {
        _cameraRotation = Vector3.SmoothDamp(_cameraRotation, new Vector3(_input.x, _input.y, 0f),
            ref _smoothDampVelocity,_smoothSpeed);
        transform.eulerAngles= _cameraRotation;
    }

    private void CameraPosition()
    {
        var newPosition = (_isExecution? (_currentLookTarget.position+_currentLookTarget.up*0.7f):_lookTarget.position)+ (-transform.forward*_positionOffset);
        //based by lookTarget and with a offset distance(distance can be modified)
        transform.position = Vector3.Lerp(transform.position, newPosition, DevelopmentToos.UnTetheredLerp(_positionSmoothTime));
    }

    private void SetExecutionTarget(Transform target,float time)
    {
        _isExecution = true;
        _currentLookTarget = target;
        _positionOffset = 0.01f;
        TimerManager.MainInstance.TryGetOneTimer(time, ResetTarget);
    }
    private void ResetTarget()
    {
        _isExecution = false;
        _positionOffset = 0.5f;
        _currentLookTarget = _lookTarget;
    }

}
