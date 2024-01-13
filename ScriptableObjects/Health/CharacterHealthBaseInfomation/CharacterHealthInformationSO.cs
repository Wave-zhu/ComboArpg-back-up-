using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace MyGame.HealthData
{
    [CreateAssetMenu(fileName = "HealthInformation", menuName = "Create/Character/HealthInformation", order = 0)]
    public class CharacterHealthInformationSO : ScriptableObject
    {


        private float _currentHP;
        private float _currentSP;
        private float _maxHP;
        private float _maxSP;
        private bool _runOutOfSP;
        private bool _isDie => _currentHP <= 0;
        [SerializeField]
        private CharacterHealthBaseDataSO _characterHealthBaseData;

        public float CurrentHP => _currentHP;
        public float CurrentSP => _currentSP;
        public float MaxHP => _maxHP;
        public float MaxSP => _maxSP;
        public bool RunOutOfSP => _runOutOfSP;
        public bool IsDie => _isDie;


        public void InitCharacterHealthInfomation()
        {
            _maxHP = _characterHealthBaseData.MaxHP;
            _maxSP = _characterHealthBaseData.MaxSP;
            _currentHP = _maxHP;
            _currentSP = _maxSP;
            _runOutOfSP = false;
        }
        public void Damage(float damage)
        {
            //enough sp but be attacked->both SP and HP will be cut
            if (!_runOutOfSP)
            {
                _currentSP = Clamp(_currentSP, damage, 0, _maxSP, false);
                if(_currentSP <= 0)
                {
                    _runOutOfSP = true;
                }
            }
            _currentHP = Clamp(_currentHP, damage, 0, _maxHP, false);
        }
        public void DamageCutSP(float damage) 
        {
            if (!_runOutOfSP)
            {
                _currentSP = Clamp(_currentSP, damage, 0, _maxSP, false);
                if (_currentSP <= 0)
                {
                    _runOutOfSP = true;
                }
            }
        }

        public void RestoreHP(float hp)
        {
            _currentHP = Clamp(_currentHP, hp, 0, _maxHP, true);
        }
        public void RestoreSP(float sp)
        {
            _currentSP = Clamp(_currentSP, sp, 0, _maxSP, true);
            if(_currentSP >= 0.7*_maxSP)
                _runOutOfSP = false;
        }
        private float Clamp(float value,float offset, float min, float max,bool add)
        {
            return Mathf.Clamp(add?value+offset:value-offset, min, max);
        }

    }
}

