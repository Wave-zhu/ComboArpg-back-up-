using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using GGG.Tool;
using UnityEngine;

namespace MyGame.Health
{
    public class EnemyHealthControl : CharacterHealthBase
    {
        protected override void CharacterHitAction(float damage, string hitName, string parryName)
        {
            //can parry when stamina>0
            //attack with high damage can cut a large size of stamina
            if (parryName == null)
            {
                _animator.Play(hitName, 0, 0f);
                GamePoolManager.MainInstance.TryGetPoolItem("SwordHitSound", transform.position, Quaternion.identity);
                TakeDamage(damage);
            }
            else if (!_characterHealthInformation.RunOutOfSP && !_animator.AnimationAtTag("Attack"))
            {
                //parry
                _animator.Play(parryName, 0, 0f);
                GamePoolManager.MainInstance.TryGetPoolItem("BlockSound", transform.position, Quaternion.identity);
                _parryEffect.Play();
                _characterHealthInformation.DamageCutSP(damage);
                if (_characterHealthInformation.RunOutOfSP)
                {
                    GameEventManager.MainInstance.CallEvent("EnableExecute", true);
                }
            }
            else
            {
                if (_characterHealthInformation.CurrentHP<0.2f*_characterHealthInformation.MaxHP)
                {
                    GameEventManager.MainInstance.CallEvent("EnableExecute", true);
                }
                //hit
                _animator.Play(hitName, 0, 0f);
                GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity);
                _hitEffect.Play();
                TakeDamage(damage);
            }
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            GameEventManager.MainInstance.CallEvent("UpdateDamageImage", _characterHealthInformation,transform);
            if (_characterHealthInformation.CurrentHP <= 0)
            {
                GameEventManager.MainInstance.CallEvent("WhenEnemyDead", transform);
                transform.gameObject.layer = 10;
                _animator.Play("Die", 0, 0f);
                GameEventManager.MainInstance.CallEvent("SetHealth", false, transform);
                EnemyManager.MainInstance.RemoveFromActiveEnemy(gameObject);
                TimerManager.MainInstance.TryGetOneTimer(20f, Respawn);
            }
        }
        private void Respawn()
        {
            _characterHealthInformation.RestoreHP(_characterHealthInformation.MaxHP);
            GameEventManager.MainInstance.CallEvent("UpdateDamageImage", _characterHealthInformation,transform);
            _characterHealthInformation.RestoreSP(_characterHealthInformation.MaxSP);
            GameEventManager.MainInstance.CallEvent("SetHealth",true, transform);
            transform.gameObject.layer = 8;
            EnemyManager.MainInstance.AddActiveEnemy(gameObject);

            _animator.Play("Lucy_Idle", 0, 0f);
        }

        protected override void Awake()
        {
            base.Awake();
            EnemyManager.MainInstance.AddEnemy(gameObject);
        }

        protected override void Start()
        {
            base.Start();
        }
    }

}
