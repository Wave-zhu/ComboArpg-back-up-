using GGG.Tool;
using MyGame.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Weapon;
public class KatanaControl : WeaponBase
{
    [SerializeField, Header("Hip Weapon")]
    private Transform WeaponHip;
    [SerializeField, Header("Equip Weapon")]
    private Transform WeaponHand;

    private void Awake()
    {
        _slashEffect = WeaponHand.GetComponentInChildren<IFX>();
        WeaponHand.gameObject.SetActive(false);
        _weaponIndex = 1;
    }

    protected override void WhenEquip()
    {
        WeaponHand.gameObject.SetActive(true);
        WeaponHip.gameObject.SetActive(false);
    }
    protected override void WhenUnEquip()
    {
        WeaponHand.gameObject.SetActive(false);
        WeaponHip.gameObject.SetActive(true);
    }
}


