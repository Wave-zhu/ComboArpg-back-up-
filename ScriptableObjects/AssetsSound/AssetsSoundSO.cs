using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace MyGame.Assets 
{
    [CreateAssetMenu(fileName ="Sound",menuName ="Create/Assets/Sound",order = 0)]
    public class AssetsSoundSO : ScriptableObject
    {
        [System.Serializable]
        private class Sounds
        { 
            public SoundType soundType;
            public AudioClip[] audioClips;
        }
        [SerializeField] private List<Sounds> _configSound = new List<Sounds>();    

        public AudioClip GetAudioClip(SoundType soundType)
        {
            if(_configSound.Count == 0) return null;
            switch (soundType)
            {
                case SoundType.ATTACK:
                    return _configSound[0].audioClips[Random.Range(0, _configSound[0].audioClips.Length)];
                case SoundType.HIT:
                    return _configSound[1].audioClips[Random.Range(0, _configSound[1].audioClips.Length)];
                case SoundType.FOOT:
                    return _configSound[2].audioClips[Random.Range(0, _configSound[2].audioClips.Length)];
                case SoundType.BLOCK:
                    return _configSound[3].audioClips[Random.Range(0, _configSound[3].audioClips.Length)];
                case SoundType.SWORDATTACK:
                    return _configSound[4].audioClips[Random.Range(0, _configSound[3].audioClips.Length)];
                case SoundType.SWORDHIT:
                    return _configSound[5].audioClips[Random.Range(0, _configSound[3].audioClips.Length)];
        }
            return null;
        }

    }
}


