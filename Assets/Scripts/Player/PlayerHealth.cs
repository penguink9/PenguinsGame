using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockback;
    public bool isDead { get; private set; }
    private Flash flash;
    private Slider healthBar;

    public event Action OnPlayerDeath;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
        currentHealth = maxHealth;
        isDead = false;
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
            TakeDamage(1, other.transform);
            CheckIfPlayerDead();
        }
    }
    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHPSlider();
    }
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage || isDead) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        UISingleton.Instance.ShowDmgTakeEffect(transform, damageAmount);
        UpdateHPSlider();
        StartCoroutine(DamageRecoveryRoutine());
    }
    public bool Heal(int healAmount)
    {
        if (isDead) return false;
        if (currentHealth == maxHealth) return false; // No need to heal if already at max health

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UISingleton.Instance.ShowHealEffect(transform, healAmount);
        UpdateHPSlider();
        return true;
    }
    private void CheckIfPlayerDead()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger("Death");
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
    }
    private void UpdateHPSlider()
    {
        if (healthBar == null) return;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }


}
