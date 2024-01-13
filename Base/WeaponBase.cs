using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.Weapon 
{
    public abstract class WeaponBase : MonoBehaviour
    {
        protected int _weaponIndex;
        public IFX _slashEffect;
        // Start is called before the first frame update
        public int WeaponIndex() => _weaponIndex;
        protected virtual void WhenEquip()
        {
            return;
        }
        protected virtual void WhenUnEquip()
        {
            return;
        }
    }
}


