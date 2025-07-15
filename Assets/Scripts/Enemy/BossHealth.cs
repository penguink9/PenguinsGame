using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BossHealth : EnemyHealth 

{
    private Slider healthBar;
    [SerializeField] private GameObject HPContainer;

    private void Start()
    {
        healthBar = HPContainer.transform.GetChild(1).GetComponent<Slider>();
    }

    public override void TakeDamage(int damage, Transform damageSrc)
    {
        DisplayHPContainer();
        base.TakeDamage(damage, damageSrc);
        UpdateHPSlider();
    }
    public override void TakeSlow(int damage, Transform damageSrc)
    {
        DisplayHPContainer();
        base.TakeSlow(damage, damageSrc);
        UpdateHPSlider();
    }

    public override void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX("Enemy Death");
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            SpawnPickup();
            HPContainer.SetActive(false);
            MapStateManager.Instance.IsBossDefeated = true;
            UISingleton.Instance.ShowMessageText(transform, "You have defeated Polar Bear! Congratulations!");
            Destroy(gameObject);
        }
    }

    private void UpdateHPSlider()
    {
        if (healthBar == null) return;
        healthBar.maxValue = startingHealth;
        healthBar.value = currentHealth;
    }

    public void DisplayHPContainer()
    {
        HPContainer.SetActive(true);
    }

    public override void LoadState(EnemyState state)
    {
        currentHealth = state.currentHP;
        UpdateHPSlider();
    }
}
