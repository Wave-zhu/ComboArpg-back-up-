using GGG.Tool;
using MyGame.ComboData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Combat
{
    public class CharacterCombatBase : MonoBehaviour
    {
         /* we need a set of combo
         * and current combo
         * and a serial of index to pinpoint which action should be played 
         */
        protected Animator _animator;
        protected Transform _currentTargetTransform;

        [SerializeField, Header("Basic Combo")]
        protected CharacterComboSO _fistCombo;
        [SerializeField]
        protected CharacterComboSO _kickCombo;
        protected CharacterComboSO _basicCombo;

        [SerializeField, Header("Change Combo")]
        protected CharacterComboSO _fistChangeCombo;
        [SerializeField]
        protected CharacterComboSO _kickChangeCombo;
        protected CharacterComboSO _changeCombo;

        [SerializeField, Header("Execution")]
        protected CharacterComboSO _execution;

        [SerializeField, Header("Assassination")]
        protected CharacterComboSO _assassination;


        protected CharacterComboSO _currentCombo;
        protected int _currentComboIndex;
        protected int _hitIndex;
        protected float _maxColdTime;
        protected bool _canAttackInput;
        protected bool _canExecute;
        // able to change combo
        protected int _currentComboCount;
        protected bool _enemyCanParry;

        #region modify location

        protected void MatchPosition()
        {
            if (_currentTargetTransform == null) return;
            if (_animator == null) return;

            if (_animator.AnimationAtTag("Attack"))
            {
                var timer = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (timer > 0.35f) return;
                if (DevelopmentToos.DistanceForTarget(_currentTargetTransform, transform) > 1.5f) return;
                if(!_animator.isMatchingTarget && !_animator.IsInTransition(0))
                {
                    MatchingProcess(_execution,0,0.2f);
                }
            }
            else if (_animator.AnimationAtTag("Execution") && !_animator.IsInTransition(0))
            {
                transform.rotation = Quaternion.LookRotation(-_currentTargetTransform.forward);
                MatchingProcess(_execution);
            }
            else if (_animator.AnimationAtTag("Assassination") && !_animator.IsInTransition(0))
            {
                transform.rotation = Quaternion.LookRotation(_currentTargetTransform.forward);
                MatchingProcess(_assassination);
            }
            else if (_animator.AnimationAtTag("Sword") && !_animator.IsInTransition(0))
            {
                if (DevelopmentToos.DistanceForTarget(_currentTargetTransform, transform) > 3f) return;
                transform.rotation = Quaternion.LookRotation(-_currentTargetTransform.forward);
                MatchingProcess(_assassination,0,0.02f);
            }
        }
        protected void MatchingProcess(CharacterComboSO currentCombo, float startTime = 0f, float endTime = 0.01f)
        {
            if (!_animator.isMatchingTarget)
            {
                _animator.MatchTarget(_currentTargetTransform.position +
                    (-transform.forward) * currentCombo.TryGetComboPositionOffset(_currentComboIndex),
                    Quaternion.identity, AvatarTarget.Body, new MatchTargetWeightMask(Vector3.one, 0f),
                   startTime, endTime);
            }
        }

        #endregion


        #region basicAttack
        protected bool CanBasicAttackInput()
        {
            /*
             * can't attack when _canAttackInput==false
             * player is being hit
             * player is parrying
             * player is executing
             */
            if (!_canAttackInput ||
                _animator.AnimationAtTag("Hit") ||
                _animator.AnimationAtTag("Parry") ||
                _animator.AnimationAtTag("Execution") ||
                _animator.AnimationAtTag("Assassination"))
                return false;
            return true;
        }
        protected virtual void CharacterBasicAttackInput()
        {

        }
        protected virtual void ExecuteComboAction()
        {
            _currentComboCount += (_currentCombo == _basicCombo) ? 1 : 0;
            //update hitIndex
            _hitIndex = 0;
            if (_currentComboIndex == _currentCombo.TryGetComboMaxCount())
            {
                // up to index bound, reset to 0
                _currentComboIndex = 0;
                _currentComboCount = 0;
            }
            _maxColdTime = _currentCombo.TryGetColdTime(_currentComboIndex);
            _animator.CrossFadeInFixedTime(_currentCombo.TryGetOneComboAction(_currentComboIndex), 0.1555f, 0, 0f);

            TimerManager.MainInstance.TryGetOneTimer(_maxColdTime, UpdateComboInformation);
            _currentComboIndex++;
            _canAttackInput = false;
        }
        protected void UpdateComboInformation()
        {
            _maxColdTime = 0f;
            _canAttackInput = true;
        }
        protected void UpdateHitIndex()
        {
            _hitIndex++;
            if (_hitIndex == _currentCombo.TryGetHitAndParryNameMaxCount(_currentComboIndex))
            {
                _hitIndex = 0;
            }
        }
        protected void ResetComboInformation()
        {
            _currentComboIndex = 0;
            _maxColdTime = 0;
            _hitIndex = 0;
        }

        protected void OnEndAttack()
        {
            if (_animator.AnimationAtTag("Motion") && _canAttackInput)
            {
                ResetComboInformation();
                _currentComboCount = 0;
            }
        }

        protected void ChangeComboData(CharacterComboSO comboData)
        {
            if (_currentCombo != comboData)
            {
                _currentCombo = comboData;
                ResetComboInformation();
            }
        }
        #endregion

        #region execution
        protected bool CanExecute()
        {
            if (_animator.AnimationAtTag("Execution")) return false;
            if (_currentTargetTransform == null) return false;
            if (!_canExecute) return false;

            return true;
        }

        protected virtual void EnableExecute(bool apply)
        {
            if (_canExecute) return;
            _canExecute = apply;

        }
        protected virtual void CharacterExecutionInput()
        {

        }

        #endregion

        #region Assassination

        protected bool CanAssassinate()
        {
            if (_currentTargetTransform == null) return false;
            if (_animator.AnimationAtTag("Assassination")) return false;
            if (DevelopmentToos.DistanceForTarget(transform, _currentTargetTransform) > 2f) return false;
            if (Vector3.Angle(transform.forward - _currentTargetTransform.forward, -_currentTargetTransform.forward) > 80f ||
            Vector3.Angle(transform.forward, _currentTargetTransform.forward) > 90f) return false;
            return true;
        }
        protected virtual void CharacterAssassinationInput()
        {

        }
        #endregion

        #region sendMessage
        private void WhenAttack()
        {            
            TriggerDamage();
            UpdateHitIndex();
            //sword audio or normal audio
            if (_enemyCanParry)
            {
                GamePoolManager.MainInstance.TryGetPoolItem("AttackSound", transform.position, Quaternion.identity);
            }         
            else
            {
                GetComponent<KatanaControl>()._slashEffect.Play();
                GamePoolManager.MainInstance.TryGetPoolItem("SwordAttackSound", transform.position, Quaternion.identity);
            }
        }
        protected void TriggerDamage()
        {
            // trigger damage when target exists and target's position under the range with right angle
            if (_currentTargetTransform == null) return;
            if (Vector3.Dot(transform.forward, DevelopmentToos.DirectionForTarget(transform, _currentTargetTransform)) < 0.85f) return;
            if (DevelopmentToos.DistanceForTarget(transform, _currentTargetTransform) > 1.3f) return;

            //damage,hitname,parryname,attacker,target
            if (_animator.AnimationAtTag("Attack"))
            {
                GameEventManager.MainInstance.CallEvent("OnCharacterHitEvent", _currentCombo.TryGetDamage(_currentComboIndex - 1),
                _currentCombo.TryGetOneHitAction(_currentComboIndex - 1, _hitIndex),
                _currentCombo.TryGetOneParryAction(_currentComboIndex - 1, _hitIndex),
                transform, _currentTargetTransform);
            }
            else if (_animator.AnimationAtTag("Sword"))
            {
                string temp = null;
                GameEventManager.MainInstance.CallEvent("OnCharacterHitEvent", _currentCombo.TryGetDamage(_currentComboIndex - 1),
                _currentCombo.TryGetOneHitAction(_currentComboIndex - 1, _hitIndex),
                temp,
                transform, _currentTargetTransform);
            }
            else
            {
                GameEventManager.MainInstance.CallEvent("ApplyExecutionDamage", _execution.TryGetDamage(_currentComboIndex), _currentTargetTransform);
            }
        }

        #endregion

        #region lookAtTarget

        protected void LookAtTargetOnAttack()
        {
            if (_currentTargetTransform == null) return;
            if (DevelopmentToos.DistanceForTarget(transform, _currentTargetTransform) > 3f) return;
            if (_animator.AnimationAtTag("Attack") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            {
                transform.Look(_currentTargetTransform.position, 50f);
            }
        }

        #endregion


        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _basicCombo = _fistCombo;
            _changeCombo = _fistChangeCombo;
        }
        protected virtual void Start()
        {
            _enemyCanParry = true;
            _canAttackInput = true;
            _canExecute = false;
            _currentCombo = _basicCombo;
        }
        protected virtual void Update()
        {
            MatchPosition();
            LookAtTargetOnAttack();
            CharacterBasicAttackInput();
            CharacterExecutionInput();
            CharacterAssassinationInput();
            OnEndAttack();
        }
    }

}

