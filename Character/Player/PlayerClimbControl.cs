using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbControl : MonoBehaviour
{
    private Animator _animator;
    [SerializeField, Header("Detection")] private float _detectionDistance;
    [SerializeField] private LayerMask _detectionLayer;

    private RaycastHit _hit;

    private bool CanClimb()
    {
        return Physics.Raycast((transform.position + transform.up * 0.5f),
            transform.forward, out _hit, _detectionDistance, _detectionLayer, QueryTriggerInteraction.Ignore);
    }

    private void CharacterClimbInput()
    {
        if (!CanClimb()) return;
        if (GameInputManager.MainInstance.Climb&&!_animator.AnimationAtTag("Climb"))
        {
            var position = Vector3.zero;
            var rotation = Quaternion.LookRotation(-_hit.normal);
            position.Set(_hit.point.x, 
                _hit.collider.bounds.extents.y + _hit.collider.transform.position.y-transform.position.y, _hit.point.z);
            
            switch (_hit.collider.tag)
            {
                case "Middle wall":
                    ToCallEvent(position, rotation);
                    _animator.CrossFade("climb_middle_wall",0f,0,0f);
                    break;
                case "High wall":
                    ToCallEvent(position, rotation);
                    _animator.CrossFade("climb_high_wall", 0f, 0, 0f);
                    break;
            }
        }
    }
    private void ToCallEvent(Vector3 position, Quaternion rotation)
    {
        GameEventManager.MainInstance.CallEvent("SetAnimationMatchInformation", position, rotation);
        GameEventManager.MainInstance.CallEvent("EnableCharActerGravity", false);
    }
    private void Awake()
    {
        _animator=GetComponent<Animator>();
    }
    private void Update()
    {
        CharacterClimbInput();
    }
}
