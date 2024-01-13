using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Combat
{
    public class EnemyCombatControl : CharacterCombatBase
    {
        //AI accept the attack command only in avaliable
        [SerializeField]
        private bool _attackCommand;
        public bool AttackCommand() =>_attackCommand;

        public void AIBasicAttackInput()
        {
            if(!_canAttackInput) return;            
            if (_currentCombo == null || _currentCombo != _basicCombo)
            {
                if (_currentCombo == _changeCombo){
                    ChangeComboData(_basicCombo);
                    ResetComboInformation();
                    ResetAttackCommand();
                    _currentComboCount = 0;                   
                    return;
                }
                ChangeComboData(_basicCombo);
            }
            ExecuteComboAction();
        }
        public void SwitchAttackStyle()
        {
            if (_basicCombo == _fistCombo)
            {
                _basicCombo = _kickCombo;
                _changeCombo = _kickChangeCombo;
            }
            else
            {
                _basicCombo = _fistCombo;
                _changeCombo = _fistChangeCombo;
            }
            ResetComboInformation();
            _currentComboCount = 0;
        }
        private void UseChangeCombo()
        {
            if (_currentComboCount < 3) return;
            if (Random.Range(0, 3) == 1)
            {
                _currentCombo = _changeCombo;
                ChangeComboData(_changeCombo);
                switch (_currentComboCount)
                {
                    case 3:
                        _currentComboIndex = 0;
                        break;
                    case 4:
                        _currentComboIndex = 1;
                        break;
                    case 5:
                        _currentComboIndex = 2;
                        break;
                }
                _currentComboCount = 0;
            }
        }
        protected override void ExecuteComboAction()
        {
            UseChangeCombo();
            _currentComboCount += (_currentCombo == _basicCombo) ? 1 : 0;
            //update hitIndex
            _hitIndex = 0;
            if (_currentComboIndex == _currentCombo.TryGetComboMaxCount())
            {
                // up to index bound, reset to 0
                _currentComboIndex = 0;
                _currentComboCount = 0;
                //reset command after attack
                ResetAttackCommand();
                return;
            }
            _maxColdTime = _currentCombo.TryGetColdTime(_currentComboIndex);
            _animator.CrossFadeInFixedTime(_currentCombo.TryGetOneComboAction(_currentComboIndex), 0.1555f, 0, 0f);

            TimerManager.MainInstance.TryGetOneTimer(_maxColdTime, UpdateComboInformation);
            _currentComboIndex++;
            _canAttackInput = false;
        }
        


        private bool CheckAIState()
        {
            if(_animator.AnimationAtTag("Attack")||
                _animator.AnimationAtTag("Hit") ||
                _animator.AnimationAtTag("Parry") ||
                _animator.AnimationAtTag("Executed") ||
                _animator.AnimationAtTag("Assassinated")||
                _attackCommand)
            return false;
            
            return true;
        }
        private void ResetAttackCommand()
        {
            _attackCommand = false;
        }
        public void SetAttackCommand()
        {
            if (!CheckAIState()){
                ResetAttackCommand();
                return;
            }
            _attackCommand= true;
        }
        private void SetDead(Transform targetTransform)
        {
            if (transform == targetTransform)
            {
                ResetAttackCommand();
                ResetComboInformation();
                _canAttackInput = true;
            }
        }
        public void StopAllAction()
        {
            if(_attackCommand)
            {
                ResetAttackCommand();
            }
            if (_animator.AnimationAtTag("Attack"))
            {
                _animator.Play("Lucy_Idle",0,0f);
                ResetAttackCommand();
            }
        }
        protected override void Start()
        {
            base.Start();
            _currentTargetTransform = EnemyManager.MainInstance.GetMainPlayer();
        }
        private void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListener<Transform>("WhenEnemyDead", SetDead);
        }
        private void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEvent<Transform>("WhenEnemyDead", SetDead);
        }

    }
}


