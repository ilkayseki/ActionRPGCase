using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class ShootingController : MonoBehaviour
{
    public LookAtIK lookAtIK;
    public GameObject bulletPrefab; // Mermi prefab'ı
    [SerializeField] private float fireRate = 2f; // Ateş etme aralığı (saniye cinsinden)
    [SerializeField] private float bulletSpeed = 10f; // Mermi hızı
    
    private void Start()
    {
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }


    private void Fire()
    {
        if (lookAtIK != null && lookAtIK.solver != null)
        {
            // LookAt IK'nin baktığı yönü al
            Vector3 lookDirection = lookAtIK.solver.GetIKPosition() - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            
            SpawmBullet();
            
        }
    }
    
    

    private void SpawmBullet()
    {
        // Mermiyi spawn point pozisyonunda ve rotasyonunda instantiate et
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }


/*

private Transform target; // Ateş edilecek hedef
[SerializeField] private GameObject BulletPrefab; // Mermi prefab'ı
[SerializeField] private float fireRate = 2f; // Ateş etme aralığı (saniye cinsinden)
[SerializeField] private float bulletSpeed = 10f; // Mermi hızı


[SerializeField] private Transform SpawnPoint;


private void Start()
{
    // Belirli bir aralıkla sürekli ateş et
    StartCoroutine(FireRoutine());
}

private IEnumerator FireRoutine()
{
    while (true)
    {
        Fire();
        yield return new WaitForSeconds(fireRate);
    }
}


private void Fire()
{
    target = nearestEnemyTracker.GetNearestEnemy();
    if (target == null) return;

    // Mermiyi instantiate et
    GameObject projectile = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

    // Merminin hedefe doğru yönlenmesi
    Vector3 direction = (target.position - transform.position).normalized;
    projectile.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

    // Opsiyonel: Mermi yönünü hedefe bakacak şekilde ayarlama
    projectile.transform.LookAt(target);
}

*/
}
