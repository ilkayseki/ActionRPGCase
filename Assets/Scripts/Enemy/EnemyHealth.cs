using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'ı
    [SerializeField] public GameObject goldboxPrefab; 

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
            CreateGoldBox();
            Destroy(gameObject);
        }
    }

    public void DestroyYourself()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosionDuration);
        CreateGoldBox();
        Destroy(gameObject);
    }
    
    private void CreateGoldBox()
    {
        // Belirli bir prefab'ı belirli bir pozisyonda oluştur
        GameObject goldbox = Instantiate(goldboxPrefab, transform.position, Quaternion.identity);
        
        goldbox.GetComponent<EnemyGoldBox>().AddRewardedGoldValue(rewardedGold);
    }
    
    
    
   
}
