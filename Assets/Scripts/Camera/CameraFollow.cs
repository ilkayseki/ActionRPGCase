using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Takip edilecek karakterin Transform'u
    private Vector3 offset;    // Kameranın hedefe göre pozisyonu (otomatik hesaplanacak)
    
    public float smoothSpeed = 0.125f;  // Kameranın yumuşak hareket hızı
    private Quaternion initialRotation; // Kameranın başlangıç rotasyonu

    void Start()
    {
        // Kamera ve karakter arasındaki başlangıç mesafesini otomatik olarak hesapla
        offset = transform.position - target.position;

        // Kameranın başlangıçtaki rotasyonunu kaydet
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // İstenen pozisyon
        Vector3 desiredPosition = target.position + offset;

        // Kamera pozisyonunu yumuşak bir geçişle ayarla
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Kamerayı yeni pozisyona ayarla
        transform.position = smoothedPosition;

        // Kameranın başlangıç rotasyonunu koru
        transform.rotation = initialRotation;
    }
}