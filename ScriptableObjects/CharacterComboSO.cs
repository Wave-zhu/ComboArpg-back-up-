using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.ComboData
{
    [CreateAssetMenu(fileName = "Combo", menuName = "Create/Character/Combo", order = 0)]
    public class CharacterComboSO : ScriptableObject
    {
        [SerializeField]private List<CharacterComboDataSO>_allComboData = new List<CharacterComboDataSO>();

        public string TryGetOneComboAction(int index)
        {
            if (_allComboData.Count <= index) return null;
            return _allComboData[index].ComboName;
        }
        public string TryGetOneHitAction(int index,int hitIndex)
        {
            if (_allComboData.Count <= index) return null;
            if (_allComboData[index].GetHitAndParryNameMaxCount() <= hitIndex) return null;
            return _allComboData[index].ComboHitName[hitIndex];
        }
        public string TryGetOneParryAction(int index, int parryIndex)
        {
            if (_allComboData.Count <= index) return null;
            if (_allComboData[index].GetHitAndParryNameMaxCount() <= parryIndex) return null;
            return _allComboData[index].ComboParryName[parryIndex];
        }
        public float TryGetDamage(int index)
        {
            if (_allComboData.Count <= index) return 0;
            return _allComboData[index].Damage;
        }
        public float TryGetColdTime(int index)
        {
            if (_allComboData.Count <= index) return 0;
            return _allComboData[index].ColdTime;
        }
        public float TryGetComboPositionOffset(int index)
        {
            if (_allComboData.Count <= index) return 0;
            return _allComboData[index].ComboPositionOffset;
        }
        public int TryGetHitAndParryNameMaxCount(int index)
        {
            if (_allComboData.Count <= index) return 0;
            return _allComboData[index].GetHitAndParryNameMaxCount();
        }
        public int TryGetComboMaxCount()=>_allComboData.Count;
    }
}
