using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; // Merminin ömrü (saniye cinsinden)
    [SerializeField] private int damageAmount = 1;
    private void Start()
    {
        // Belirli bir süre sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Eğer bir objeye çarptıysa, mermiyi yok et
        gameObject.SetActive(false);
        collision.gameObject.GetComponent<EnemyHealth>()?.TakeDamage(damageAmount);
        
        Destroy(gameObject);

    }

    public void SetDamage(int d)
    {
        damageAmount = d;
    }
}