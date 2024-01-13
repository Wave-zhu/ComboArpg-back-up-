using MyGame.HealthData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private TextMeshProUGUI _maxHP;
    [SerializeField] private TextMeshProUGUI _currentHP;
    private void Start()
    {
        _healthImage.fillAmount = 1;
        _maxHP.text = "/1000";
        _currentHP.text = "1000";
    }
    private void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListener<CharacterHealthInformationSO>("UpdateHealthImage",UpdateHealthImage);
    }
    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<CharacterHealthInformationSO>("UpdateHealthImage", UpdateHealthImage);
    }
    public void UpdateHealthImage(CharacterHealthInformationSO information)
    {
        _healthImage.fillAmount = information.CurrentHP/information.MaxHP;
        _currentHP.text = information.CurrentHP+"";
    }
}
