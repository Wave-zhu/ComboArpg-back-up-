using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Event
{
    public class AnimationEvent : MonoBehaviour
    {
        private void PlaySound(string name)
        {
            GamePoolManager.MainInstance.TryGetPoolItem(name,transform.position,Quaternion.identity);
        }
    }
}
