using GGG.Tool;
using MyGame.Combat;
using MyGame.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualSwordControl : WeaponBase
{
    [SerializeField,Header("Hip Weapon")]
    private Transform LWeaponHip;
    [SerializeField]
    private Transform RWeaponHip;
    [SerializeField, Header("Equip Weapon")]
    private Transform LWeaponHand;
    [SerializeField]
    private Transform RWeaponHand;


    private void Awake()
    {
        LWeaponHand.gameObject.SetActive(false);
        RWeaponHand.gameObject.SetActive(false);
        _weaponIndex = 2;
    }
    protected override void WhenEquip()
    {
        LWeaponHand.gameObject.SetActive(true);
        RWeaponHand.gameObject.SetActive(true);
        LWeaponHip.gameObject.SetActive(false);
        RWeaponHip.gameObject.SetActive(false);
    }
    protected override  void WhenUnEquip()
    {
        LWeaponHand.gameObject.SetActive(false);
        RWeaponHand.gameObject.SetActive(false);
        LWeaponHip.gameObject.SetActive(true);
        RWeaponHip.gameObject.SetActive(true);
    }


}
