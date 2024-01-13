using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Health;
using MyGame.HealthData;
using GGG.Tool;
using System.Buffers;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private GameObject _healthUI;
    private Camera _camera;
    private void Start()
    {
        _healthImage.fillAmount = 1;
        _camera = Camera.main;
    }
    private void Update()
    {
        _healthUI.transform.rotation=Quaternion.LookRotation(_healthUI.transform.position - _camera.transform.position);
    }
    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListener<CharacterHealthInformationSO, Transform>("UpdateDamageImage", UpdateDamageImage);
        GameEventManager.MainInstance.AddEventListener<bool,Transform>("SetHealth", SetHealth);
    }
    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<CharacterHealthInformationSO,Transform>("UpdateDamageImage", UpdateDamageImage);
        GameEventManager.MainInstance.RemoveEvent<bool,Transform>("SetHealth", SetHealth);
    }
    public void UpdateDamageImage(CharacterHealthInformationSO information,Transform trans)
    {
        if (transform != trans) return;
        _healthImage.fillAmount = information.CurrentHP / information.MaxHP;
    }
    public void SetHealth(bool active,Transform trans)
    {
        if (transform != trans) return;
        _healthUI.SetActive(active);
    }
}
