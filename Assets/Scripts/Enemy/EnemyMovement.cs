using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player; // Oyuncu referansı
    [SerializeField] private float stoppingDistance = 1.0f; // Düşmanın duracağı mesafe
    public float speed = 0; // Düşmanın hızı
    private NavMeshAgent navMeshAgent; // NavMeshAgent bileşeni
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        player = GameObject.FindWithTag("Player");

        // Düşmanın yüzünü oyuncuya döndür
        LookAtPlayer();
    }

    void FixedUpdate()
    {
        // Oyuncu ile düşman arasındaki mesafeyi kontrol et
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Eğer düşman, oyuncudan daha yakınsa, hareket et
        if (distanceToPlayer > stoppingDistance)
        {
            // Oyuncunun konumuna git
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            // Eğer durma mesafesine ulaşıldıysa dur
            navMeshAgent.ResetPath();
        }
    }

    private void LookAtPlayer()
    {
        // Oyuncunun konumuna bak
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }
}