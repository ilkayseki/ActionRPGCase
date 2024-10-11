using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] private Button StartGameButtonPrefab; // Start game butonu prefabı
    [SerializeField] private Transform StartGameButtonParent; // Start game butonu için parent

    private void Start()
    {
        CreateStartGameButton();
    }

    private void CreateStartGameButton()
    {
        // Butonu instantiate et ve parent'ına yerleştir
        Button uiObject = Instantiate(StartGameButtonPrefab, StartGameButtonParent);
        
        // Butona tıklandığında çağrılacak olan fonksiyonu ekle
        uiObject.onClick.AddListener(OnStartGameButtonClicked);
    }

    // Butona tıklandığında çalışacak olan fonksiyon
    private void OnStartGameButtonClicked()
    {
        Debug.Log("Butona tıklandı, sahne değiştiriliyor...");
        // Sahneyi değiştirmek için coroutine başlatıyoruz
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Bir sonraki sahneyi yükle

    }

}