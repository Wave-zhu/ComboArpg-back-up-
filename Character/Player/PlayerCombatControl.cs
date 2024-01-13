using GGG.Tool;
using MyGame.ComboData;
using MyGame.Health;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using MyGame.Weapon;

namespace MyGame.Combat
{
    public class PlayerCombatControl : CharacterCombatBase
    {
        private Transform _cameraObject;
        [SerializeField, Header("Weapon Combo")]
        private CharacterComboSO _weaponCombo;
        private bool _weaponStyle;
        
        private bool _isEquiped;

        //player need to detect enemy
        private Vector3 _detectDirection;
        [SerializeField, Header("Detection")]
        private float _detectRange;
        [SerializeField]
        private float _detectMaxDistance;
        private Collider[] _detectTargets;
        private bool _activeEnemyDetection;
        private WeaponBase _weapon;

        #region basic attack
        protected override void CharacterBasicAttackInput()
        {
            if (!CanBasicAttackInput()) return;
            if (GameInputManager.MainInstance.UseKick)
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
            if (GameInputManager.MainInstance.LAttack)
            {
                if (_currentCombo == null || (_currentCombo != _weaponCombo && _currentCombo != _basicCombo))
                {
                    ChangeComboData(_basicCombo);
                }
                ExecuteComboAction();
            }
            else if (GameInputManager.MainInstance.RAttack && !_isEquiped)
            {
                if (_currentComboCount < 3) return;
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
                ExecuteComboAction();
                _currentComboCount = 0;

            }
        }
        #endregion

        #region execution
        protected override void EnableExecute(bool apply)
        {
            base.EnableExecute(apply);
            _activeEnemyDetection = false;
        }
        protected override void CharacterExecutionInput()
        {
            if (!CanExecute()) return;
            if (GameInputManager.MainInstance.Grab && !_isEquiped)
            {
                _currentComboIndex = Random.Range(0, _execution.TryGetComboMaxCount());

                _animator.Play(_execution.TryGetOneComboAction(_currentComboIndex));

                GameEventManager.MainInstance.CallEvent("OnCharacterExecuted",
                    _execution.TryGetOneHitAction(_currentComboIndex, 0),
                    transform, _currentTargetTransform);
                TimerManager.MainInstance.TryGetOneTimer(0.5f, UpdateComboInformation);
                EnemyManager.MainInstance.StopAllActiveEnemy();
                //move camera
                GameEventManager.MainInstance.CallEvent("SetExecutionTarget", _currentTargetTransform,
                    _animator.GetCurrentAnimatorStateInfo(0).length + 1f);
                //clear signal after execution
                _activeEnemyDetection = true;
                _canExecute = false;
            }

        }

        #endregion

        #region assassination
        protected override void CharacterAssassinationInput()
        {
            if (!CanAssassinate()) return;
            if (GameInputManager.MainInstance.TakeOut && !_isEquiped)
            {

                _currentComboIndex = Random.Range(0, _assassination.TryGetComboMaxCount());

                _animator.Play(_assassination.TryGetOneComboAction(_currentComboIndex));
                //method same as execution
                GameEventManager.MainInstance.CallEvent("OnCharacterExecuted",
                    _assassination.TryGetOneHitAction(_currentComboIndex, 0),
                    transform, _currentTargetTransform);
                TimerManager.MainInstance.TryGetOneTimer(0.5f, UpdateComboInformation);
                EnemyManager.MainInstance.StopAllActiveEnemy();
            }
        }
        #endregion

        #region detection
        private void DetectNearTarget()
        {
            if (!_activeEnemyDetection) return;
            if (_currentTargetTransform != null
                && DevelopmentToos.DistanceForTarget(transform, _currentTargetTransform) < 1.5f) return;
            _detectTargets = Physics.OverlapSphere(transform.position + transform.up * 0.7f,
                _detectRange, 1 << 8, QueryTriggerInteraction.Ignore);
        }
        private void SetCurrentTarget()
        {
            if (_animator.AnimationAtTag("Sword") || _animator.AnimationAtTag("Execution") || _animator.AnimationAtTag("Assassination")) return;
            if (_animator.AnimationAtTag("Attack")
            && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f) return;
            if (_detectTargets.Length == 0) return;
            if (_currentTargetTransform != null
                && DevelopmentToos.DistanceForTarget(transform, _currentTargetTransform) < 1.5f) return;
            if (!_activeEnemyDetection) return;

            Transform temp = null;
            var distance = Mathf.Infinity;
            foreach (var detectTarget in _detectTargets)
            {
                var dis = DevelopmentToos.DistanceForTarget(transform, detectTarget.transform);
                if (dis < distance)
                {
                    distance = dis;
                    temp = detectTarget.transform;
                }
            }
            _currentTargetTransform = (temp == null ? _currentTargetTransform : temp);
        }
        private void ResetTarget()
        {
            if (_currentTargetTransform == null) return;
            if (_animator.GetFloat(AnimationID.MovementID) > 0.7f
                && DevelopmentToos.DistanceForTarget(transform, _currentTargetTransform) > 3f)
            {
                _currentTargetTransform = null;
                _canExecute = false;
                _activeEnemyDetection = true;
            }
        }

        #endregion
        #region range damage
        private Collider[] DetectTargets()
        {
           return Physics.OverlapSphere(transform.position + transform.up * 0.7f,
               _detectRange*0.8f, 1 << 8, QueryTriggerInteraction.Ignore);
        }
        private void WhenRangeAttack()
        {
            _weapon._slashEffect.Play();
            GamePoolManager.MainInstance.TryGetPoolItem("SwordAttackSound", transform.position, Quaternion.identity);
            var temp = DetectTargets();
            if (temp.Length != 0)
            {
                foreach (var detectTarget in temp)
                {
                    if (Vector3.Dot(DevelopmentToos.DirectionForTarget(transform, detectTarget.transform), transform.forward) > 0.85f)
                    {
                        string str = null;
                        GameEventManager.MainInstance.CallEvent("OnCharacterHitEvent", _currentCombo.TryGetDamage(_currentComboIndex - 1),
                        _currentCombo.TryGetOneHitAction(_currentComboIndex - 1, _hitIndex),
                        str,
                        transform, detectTarget.transform);
                     
                    }
                }
            }
            UpdateHitIndex();

        }
        #endregion

        #region Health Listener

        private void RemoveTarget(Transform targetTransform)
        {
            if (_currentTargetTransform == targetTransform)
            {
                _currentTargetTransform = null;
                _canExecute = false;
                _activeEnemyDetection = true;
            }
        }

        #endregion

        #region switch to weapon
        private void EquipWeapon()
        {
            if (GameInputManager.MainInstance.EquipWeapon && _animator.AnimationAtTag("Motion"))
            {
                _isEquiped = !_isEquiped;
                if (_isEquiped)
                {
                    _enemyCanParry = false;
                    if (_weapon.WeaponIndex() == 1)
                    {
                        _animator.Play("EquipKatana", 0, 0f);
                    }
                    else
                    {
                        _animator.Play("EquipSword", 0, 0f);
                    }
                    _currentCombo = _weaponCombo;
                }
                else
                {
                    _enemyCanParry = true;
                    if (_weapon.WeaponIndex() == 1)
                    {
                        _animator.Play("UnEquipKatana", 0, 0f);
                    }
                    else
                    {
                        _animator.Play("UnEquipSword", 0, 0f);
                    }
                    _currentCombo = _basicCombo;
                }
                ResetComboInformation();
                _currentComboCount = 0;
            }
        }
        #endregion
        #region Dodge

        protected bool CanDodge()
        {
            if (_animator.AnimationAtTag("Dodge")||_animator.IsInTransition(0)) return false;
            return true;
        }
        protected void CharacterDodgeInput()
        {
            if (!CanDodge()) return;
            if (GameInputManager.MainInstance.Dodge)
            {
                if(_animator.GetFloat(AnimationID.MovementID)<0.3f)
                    _animator.Play("Dodge_B", 0, 0.1f);
                else
                {
                    _animator.Play("Slide_F", 0, 0.1f);
                }
                
            }
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _weapon = GetComponent<WeaponBase>();
        }
        private void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListener<bool>("EnableExecute", EnableExecute);
            GameEventManager.MainInstance.AddEventListener<Transform>("WhenEnemyDead", RemoveTarget);
        }
        private void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEvent<bool>("EnableExecute", EnableExecute);
            GameEventManager.MainInstance.RemoveEvent<Transform>("WhenEnemyDead", RemoveTarget);
        }

        protected override void Start()
        {
            base.Start();
            _activeEnemyDetection = true;
       
        }
        protected override void Update()
        {
            EquipWeapon();
            CharacterDodgeInput();
            base.Update();
            SetCurrentTarget();
            ResetTarget();
        }
        private void FixedUpdate()
        {
            DetectNearTarget();
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position + transform.up * 0.7f +
                _detectDirection * _detectMaxDistance, _detectRange);
        }
    }
}

