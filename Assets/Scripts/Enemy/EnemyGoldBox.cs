using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoldBox : MonoBehaviour
{
    public int rewardedGold;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAttributes>())
        {
            HandleEnemyDied();
            Destroy(gameObject);
        }
    }

    private void HandleEnemyDied()
    {
        // Oyuncunun mevcut parasını al
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        playerMoney += rewardedGold; // Gelen altını ekle
        PlayerPrefs.SetInt("PlayerMoney", playerMoney); // Güncellenmiş parayı kaydet
        PlayerPrefs.Save(); // Değişiklikleri kaydet
        Debug.Log($"Yeni Para Miktarı: {playerMoney}");
    }

    public void AddRewardedGoldValue(int a)
    {
        rewardedGold = a;
    }
}
