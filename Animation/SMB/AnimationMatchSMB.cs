using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationMatchSMB : StateMachineBehaviour
{
    [SerializeField,Header("Match information")] private float _startTime;
    [SerializeField] private float _endTime;
    [SerializeField] private AvatarTarget _avatarTarget;

    [SerializeField,Header("Enable gravity")] private bool _isEnableGravity;
    [SerializeField] private float _enableTime;

    private Vector3 _matchPosition;
    private Quaternion _matchRotation;

    private void GetMatchInformation(Vector3 position, Quaternion rotation)
    {
        _matchPosition = position;
        _matchRotation = rotation;
    }
    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListener<Vector3, Quaternion>("SetAnimationMatchInformation", GetMatchInformation);
    }
    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<Vector3, Quaternion>("SetAnimationMatchInformation", GetMatchInformation);
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.isMatchingTarget)
        {
            animator.MatchTarget(_matchPosition, _matchRotation, _avatarTarget,
                new MatchTargetWeightMask(Vector3.one,0f), _startTime, _endTime);
        }
        if (_isEnableGravity)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _enableTime)
            {
                //activate gravity
                GameEventManager.MainInstance.CallEvent("EnableCharActerGravity", true);
            }
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}


}
