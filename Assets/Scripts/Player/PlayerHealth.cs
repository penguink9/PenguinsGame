﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 0.5f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockback;
    private Flash flash;
    private Slider healthBar;
    private PlayerState playerState;

    public event Action OnPlayerDeath;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
        playerState = GetComponent<PlayerState>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateHPSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();  
        
        if (enemy && canTakeDamage)
        {
            int damage = enemy.GetDamage();
            TakeDamage(damage, other.transform);
        }
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHPSlider();
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage || playerState.CurrentState == PlayerState.State.Dead) return;
        playerState.CurrentState = PlayerState.State.TakingDamage;
        AudioManager.Instance.PlaySFX("Penguin Take Damage");
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;

        UISingleton.Instance.ShowDmgTakeEffect(transform, damageAmount);
        UpdateHPSlider();
        CheckIfPlayerDead();
        StartCoroutine(DamageRecoveryRoutine());
    }

    public bool Heal(int healAmount)
    {
        if (playerState.CurrentState == PlayerState.State.Dead) return false;
        if (currentHealth == maxHealth) return false;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UISingleton.Instance.ShowHealEffect(transform, healAmount);
        AudioManager.Instance.PlaySFX("Healing");
        UpdateHPSlider();
        return true;
    }

    private void CheckIfPlayerDead()
    {
        if (currentHealth <= 0 && playerState.CurrentState != PlayerState.State.Dead)
        {
            currentHealth = 0;
            playerState.CurrentState = PlayerState.State.Dead;
            GetComponent<Animator>().SetTrigger("Death");
            //GetComponent<CapsuleCollider2D>().enabled = false;
            knockback.enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            EnemyTargetProvider.Instance.SetTarget(null);
            AudioManager.Instance.PlaySFX("Penguin Death");
            InventoryManager.Instance.gameObject.SetActive(false);
            StartCoroutine(DeadLoadSceneRoutine());
        }
    }

    private IEnumerator DeadLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        OnPlayerDeath?.Invoke();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;

        if (playerState.CurrentState == PlayerState.State.TakingDamage)
            playerState.CurrentState = PlayerState.State.Idle;
    }

    private void UpdateHPSlider()
    {
        if (healthBar == null) return;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHPSlider();
    }
}

