using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Movement
{
    public class PlayerMovementControl : CharacterMovementControlBase
    {
        private float _rotationAngle;
        private float _angleVelocity = 0f;
        [SerializeField] private float _rotationSmoothTime;
        private Transform _mainCamera;
        //turn run
        private Vector3 _characterTargetDirection;

        private void CharacterRotationControl()
        {
            if (!_characterIsOnGround) return;
            if (_animator.GetBool(AnimationID.HasInputID))
            {
                _rotationAngle = Mathf.Atan2(GameInputManager.MainInstance.Movement.x, GameInputManager.MainInstance.Movement.y)
               * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;

            }
            if (_animator.GetBool(AnimationID.HasInputID) && _animator.AnimationAtTag("Motion"))
            {             
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, _rotationAngle,
                    ref _angleVelocity, _rotationSmoothTime);
                _characterTargetDirection = Quaternion.Euler(0, _rotationAngle, 0) * Vector3.forward;

            }
            _animator.SetFloat(AnimationID.DeltaAngleID, DevelopmentToos.GetDeltaAngle(transform, _characterTargetDirection.normalized));


        }

        private void UpdateAnimation()
        {
            if (!_characterIsOnGround) return;
            _animator.SetBool(AnimationID.HasInputID, GameInputManager.MainInstance.Movement != Vector2.zero);
            if (_animator.GetBool(AnimationID.HasInputID))
            {
                if (GameInputManager.MainInstance.Run)
                {
                    _animator.SetBool(AnimationID.RunID, true);
                }
                _animator.SetFloat(AnimationID.MovementID, _animator.GetBool(AnimationID.RunID)? 
                    2f : GameInputManager.MainInstance.Movement.sqrMagnitude,0.25f,Time.deltaTime);
                SetCharacterFootSound();
            }
            else
            {
                _animator.SetFloat(AnimationID.MovementID, 0f, 0.25f, Time.deltaTime);
                //set false when almost stop running
                if (_animator.GetFloat(AnimationID.MovementID) < 0.2f)
                {
                    _animator.SetBool(AnimationID.RunID, false);
                }
            }

        }
        // foot step
        private float _nextFootTime;
        [SerializeField] private float _runFootTime;
        [SerializeField] private float _sprintFootTime;

        private void SetCharacterFootSound()
        {
            if (_characterIsOnGround && _animator.GetFloat(AnimationID.MovementID) > 0.5f && _animator.AnimationAtTag("Motion"))
            {
                _nextFootTime-=Time.deltaTime;
                if (_nextFootTime < 0f)
                {
                    PlayFootSound();
                }
            }
            else
            {
                _nextFootTime = 0f;
            }
        }
        private void PlayFootSound()
        {
            GamePoolManager.MainInstance.TryGetPoolItem("FootSound", transform.position, Quaternion.identity);
            _nextFootTime = (_animator.GetFloat(AnimationID.MovementID) > 1.1f) ? _sprintFootTime : _runFootTime;
        }

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main.transform;
        }
        private void LateUpdate()
        {
            UpdateAnimation();
            CharacterRotationControl();
        }
    }
}

