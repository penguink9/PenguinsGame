using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float healthDropChance = 0.5f; // 50% chance to drop health on death
    [SerializeField] private bool hasKey; // Enemy has a key to drop
    [SerializeField] private int maxCoins = 3; // Amount of max coins to drop

    private int currentHealth;
    private KnockBack knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }
    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage, Transform damageSrc)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(damageSrc, 15f);
        UISingleton.Instance.ShowDmgDealEffect(transform, damage);
        StartCoroutine(flash.FlashRoutine());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
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
