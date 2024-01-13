using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.HealthData
{
    [CreateAssetMenu(fileName ="HealthData", menuName ="Create/Character/HealthData", order =0)]
    public class CharacterHealthBaseDataSO : ScriptableObject
    {
        [SerializeField] private float _maxHP;
        [SerializeField] private float _maxSP;
        public float MaxHP => _maxHP;
        public float MaxSP => _maxSP;
    }
}

