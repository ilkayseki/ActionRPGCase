using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health;
    
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti prefab'ı
    [SerializeField] private float explosionDuration = 2f; // Patlama efektinin süresi
    
    public void TakeDamage(int damageAmount)
    {
        health = health - damageAmount;

        if (health <= 0)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Belirli bir süre sonra patlama efektini yok et
            Destroy(explosion, explosionDuration);

            
            Destroy(gameObject);
        }
    }

    public void DestroyYourself()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosionDuration);

            
        Destroy(gameObject);
    }
   
}
