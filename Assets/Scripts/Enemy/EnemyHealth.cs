using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int startingHealth = 3;
    [SerializeField] protected GameObject deathVFXPrefab;
    [SerializeField] private float healthDropChance = 0.5f; // 50% chance to drop health on death
    [SerializeField] private bool hasKey; // Enemy has a key to drop
    [SerializeField] private int maxCoins = 3; // Amount of max coins to drop
    [SerializeField] private float knockBackThrust = 15f; // Thrust amount for knockback


    public int currentHealth;
    private KnockBack knockback;
    private SlowEffect sloweffect;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
        sloweffect = GetComponent<SlowEffect>();
        currentHealth = startingHealth;

    }
    private void Start()
    {
        //currentHealth = startingHealth;
    }

    public virtual void TakeDamage(int damage, Transform damageSrc)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(damageSrc, knockBackThrust);
        UISingleton.Instance.ShowDmgDealEffect(transform, damage);
        StartCoroutine(flash.FlashRoutine());
        DetectDeath();
    }
    public virtual void TakeSlow(int damage, Transform damageSrc)
    {
        currentHealth -= damage;
        sloweffect.ApplySlow();
        UISingleton.Instance.ShowDmgDealEffect(transform, damage);
        DetectDeath();
    }
    public virtual void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX("Enemy Death");
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            SpawnPickup();
            Destroy(gameObject);
        }
    }
    public void SpawnPickup()
    {
        if(Random.value < healthDropChance)
        {
            PickupSpawner.Instance.SpawnHealthPotions(1, transform.position);
        }
        if (hasKey)
        {
            PickupSpawner.Instance.SpawnKeys(1, transform.position);
        }
        int coinsToDrop = Random.Range(1, maxCoins + 1);
        PickupSpawner.Instance.SpawnCoins(coinsToDrop, transform.position);
    }

    public EnemyState SaveState()
    {
        return new EnemyState { id = name, currentHP = currentHealth };
    }

    public void LoadState(EnemyState state)
    {
        currentHealth = state.currentHP;
    }
}
