using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Movement
{
    public enum SuperMoveState
    {
        FORWARD,
        RIGHTFORWARD,
        LEFTFORWARD
    }
    public class EnemyMovementControl : CharacterMovementControlBase
    {
        private bool _applyMovement;



        private void LookTargetDirection()
        {
            if (_animator.AnimationAtTag("Motion"))
            {
                transform.Look(EnemyManager.MainInstance.GetMainPlayer().position, 50f);
            }
        }
        public void SetAnimatorMovementValue(float horizontal,float vertical)
        {
            if(_applyMovement) 
            {
                _animator.SetBool(AnimationID.HasInputID, true);
                _animator.SetFloat(AnimationID.LockID, 1f);
                _animator.SetFloat(AnimationID.HorizontalID, horizontal, 0.2f, Time.deltaTime);
                _animator.SetFloat(AnimationID.VerticalID, vertical, 0.2f, Time.deltaTime);
            }
            else
            {
                _animator.SetFloat(AnimationID.LockID, 0f);
                _animator.SetFloat(AnimationID.HorizontalID, 0f, 0.2f, Time.deltaTime);
                _animator.SetFloat(AnimationID.VerticalID, 0f, 0.2f, Time.deltaTime);
            }
            
        }

        public void AnimationPlay(string clip)
        {
            _animator.Play(clip, 0, 0f);
        }

        //gizmo
        private void DrawDirection()
        {
            Debug.DrawRay(transform.position + transform.up * 0.7f,
                EnemyManager.MainInstance.GetMainPlayer().position - transform.position,
                Color.yellow);
        }

        #region movement
        public void SetApplyMovement(bool apply)
        {
            _applyMovement = apply;
        }
        public void EnableCharacterController(bool enable)
        {
            _characterController.enabled= enable;
        }
        #endregion

        #region supermove

        private SuperMoveState _superMoveDirection;
        private float _superMoveDistance;

        public SuperMoveState SuperMoveDirection
        {
            get
            {
                return _superMoveDirection;
            }
            set
            {
                _superMoveDirection = value;
            }
        }
        public float SuperMoveDistance
        {
            get
            {
                return _superMoveDistance;
            }
            set
            {
                _superMoveDistance = value;
            }

        }

        public void WhenSpecialMove()
        {
            print(SuperMoveDistance);
            Vector3 moveDirection;
            if(_superMoveDirection== SuperMoveState.FORWARD)
            {
                moveDirection = transform.forward.normalized;
            }
            else if(_superMoveDirection == SuperMoveState.LEFTFORWARD)
            {
                moveDirection = (transform.forward-transform.right).normalized;
            }
            else
            {
                moveDirection = (transform.forward + transform.right).normalized;
            }
            _characterController.Move(moveDirection * _superMoveDistance);
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            SetApplyMovement(true);
        }
        protected override void Update()
        {
            base.Update();
            LookTargetDirection();
            DrawDirection();
        }

    }

}

