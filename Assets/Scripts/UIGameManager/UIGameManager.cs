using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private Button BackGameButtonPrefab; // Back game butonu prefabı
    [SerializeField] private Transform BackGameButtonParent; // Back game butonu için parent

    [SerializeField] private GameObject endGamePanel;
    
    private void OnEnable()
    {
        // GameEvents'teki OnGameOver event'ine abone ol
        PlayerHealth.OnGameOver += CreateGameButtons;
    }

    private void OnDisable()
    {
        // Event'ten aboneliği kaldır
        PlayerHealth.OnGameOver -= CreateGameButtons;
    }

    private void CreateGameButtons()
    {
        
        endGamePanel.SetActive(true);
        
        Button backButton = Instantiate(BackGameButtonPrefab, BackGameButtonParent);
        backButton.onClick.AddListener(OnBackGameButtonClicked);
    }

    // Back butonuna tıklandığında çalışacak olan fonksiyon
    private void OnBackGameButtonClicked()
    {
        Debug.Log("Back butonuna tıklandı, ana menü sahnesine dönülüyor...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Bir önceki sahneyi yükle

    }
    
    
}
