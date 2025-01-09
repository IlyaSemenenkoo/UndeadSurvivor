using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private PlayerHealthManager playerHealthManager;
    private void OnEnable()
    {
        playerHealthManager.OnHpChangeEvent += SetHealth;
    }

    private void SetHealth(int health)
    {
        slider.value = health;
    }

    private void OnDisable()
    {
        playerHealthManager.OnHpChangeEvent -= SetHealth;
    }
}
