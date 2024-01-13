using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Health
{
    public class PlayerHealthControl : CharacterHealthBase
    {
        protected override void CharacterHitAction(float damage, string hitName, string parryName)
        {
            // don't apply damage when in special attack
            if (_animator.AnimationAtTag("Execution") || _animator.AnimationAtTag("Assassination")) return;

            if (_animator.GetBool(AnimationID.ParryID)) 
            {
                _animator.Play(parryName, 0, 0f);
                GamePoolManager.MainInstance.TryGetPoolItem("BlockSound", transform.position, Quaternion.identity);
                _parryEffect.Play();
                _characterHealthInformation.DamageCutSP(damage);
                //to do counterattack
            }
            else
            {
                _animator.Play(hitName, 0, 0f);
                GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity);
                _hitEffect.Play();
                TakeDamage(damage);
            }
            
        }
        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            GameEventManager.MainInstance.CallEvent("UpdateHealthImage", _characterHealthInformation);
        }



        private void PlayerParryInput()
        {
            if (_animator.AnimationAtTag("Hit") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f) return;
            if (_animator.AnimationAtTag("Executed") || _animator.AnimationAtTag("Assassinated")) return;
            _animator.SetBool(AnimationID.ParryID, GameInputManager.MainInstance.Parry);

        }

        protected override void Update()
        {
            base.Update();
            PlayerParryInput();
        }
    }
}

