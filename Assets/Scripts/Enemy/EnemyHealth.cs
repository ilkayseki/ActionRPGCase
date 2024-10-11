using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'ı
    [SerializeField] private float explosionDuration = 2f; // Patlama efektinin süresi
    public int rewardedGold;
    public void TakeDamage(int damageAmount)
    {
        health = health - damageAmount;

        if (health <= 0)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Belirli bir süre sonra patlama efektini yok et
            Destroy(explosion, explosionDuration);
            HandleEnemyDied(rewardedGold);
            Destroy(gameObject);
        }
    }

    public void DestroyYourself()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosionDuration);
        HandleEnemyDied(rewardedGold);
        Destroy(gameObject);
    }
    public void HandleEnemyDied(int gold)
    {
        // Oyuncunun mevcut parasını al
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        playerMoney += gold; // Gelen altını ekle
        PlayerPrefs.SetInt("PlayerMoney", playerMoney); // Güncellenmiş parayı kaydet
        PlayerPrefs.Save(); // Değişiklikleri kaydet
        Debug.Log($"Yeni Para Miktarı: {playerMoney}");
    }
   
}
