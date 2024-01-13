using GGG.Tool;
using MyGame.HealthData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Health
{
    public abstract class CharacterHealthBase : MonoBehaviour, IHealth
    {
        //hitted
        //executed
        //parry
        //hp
        protected Animator _animator;

        protected HitEffect _hitEffect;
        protected ParryEffect _parryEffect;
        protected Transform _currentAttacker;

        [SerializeField,Header("Health Information")]
        protected CharacterHealthInformationSO _characterHealthInformation;
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _hitEffect = GetComponentInChildren<HitEffect>();
            _parryEffect = GetComponentInChildren<ParryEffect>();
        }
        protected virtual void Start()
        {
            _characterHealthInformation.InitCharacterHealthInfomation();
        }
        protected virtual void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListener<float,string,string,Transform,Transform>
                ("OnCharacterHitEvent", OnCharacterHitEvent);
            GameEventManager.MainInstance.AddEventListener<string, Transform, Transform>
                ("OnCharacterExecuted", OnCharacterExecuted);
            GameEventManager.MainInstance.AddEventListener<float,Transform>
                ("ApplyExecutionDamage", ApplyExecutionDamage);
        }
        protected virtual void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEvent<float, string, string, Transform, Transform>
                ("OnCharacterHitEvent", OnCharacterHitEvent);
            GameEventManager.MainInstance.RemoveEvent<string, Transform, Transform>
                ("OnCharacterExecuted", OnCharacterExecuted);
            GameEventManager.MainInstance.RemoveEvent<float,Transform>
                ("ApplyExecutionDamage", ApplyExecutionDamage);
        }
        protected virtual void Update()
        {
            OnHitLookAtAttacker();
        }
        protected virtual void CharacterHitAction(float damage, string hitName,string parryName)
        {

        }
        protected virtual void TakeDamage(float damage)
        {
            _characterHealthInformation.Damage(damage);
        }
        private void SetAttacker(Transform attacker)
        {
            if(_currentAttacker == null || _currentAttacker != attacker)
            {
                _currentAttacker = attacker;
            }
        }
        private void OnHitLookAtAttacker()
        {
            if (_currentAttacker == null) return;
            if ((_animator.AnimationAtTag("Hit") || _animator.AnimationAtTag("Parry"))&&_animator.GetCurrentAnimatorStateInfo(0).normalizedTime<0.5f)
            {
                transform.Look(_currentAttacker.position,50f);
            }

        }
        private void OnCharacterHitEvent(float damage,string hitName,string parryName,Transform attacker,Transform self)
        {
            if (self != transform) return;

            SetAttacker(attacker);
            CharacterHitAction(damage, hitName, parryName);
        }
        private void OnCharacterExecuted(string hitName,Transform attacker,Transform self)
        {
            if (self != transform) return;
            SetAttacker(attacker);
            _animator.Play(hitName);
        }
        private void ApplyExecutionDamage(float damage,Transform self)
        {
            if(self != transform) return;
            TakeDamage(damage);
            GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity);
        }

        public bool IsDie()=>_characterHealthInformation.IsDie;

    }
}

