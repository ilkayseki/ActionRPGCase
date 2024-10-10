using System.Collections.Generic;
using NaughtyAttributes;
using TMPro; // TextMeshPro kütüphanesini ekle
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> upgrades; // Inspector'dan ayarlanacak UpgradeData listesi
    [SerializeField] private GameObject upgradeUIPrefab; // Yükseltme UI prefabı
    [SerializeField] private Transform upgradeUIParent;  // UI objelerinin atanacağı parent
    [SerializeField] private GameObject playerMoneyPrefab; // Player money texti
    [SerializeField] private Transform playerMoneyParent; // Player money texti
    private TextMeshProUGUI playerMoneyText; // Player money texti
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Her yükseltmenin seviyesi
    private List<UpgradeUI> upgradeUIs = new List<UpgradeUI>(); // UI objelerinin listesi

    private void Start()
    {
        CreatePlayerMoneyUI();
        UpdatePlayerMoneyUI(); // Başlangıçta PlayerMoney UI'sını güncelle
        foreach (var upgrade in upgrades)
        {
            LoadUpgrade(upgrade); // Kaydedilmiş verileri yükle
            CreateUpgradeUI(upgrade); // UI objelerini oluştur
        }
    }

    private void CreatePlayerMoneyUI()
    {
        GameObject uiObject = Instantiate(playerMoneyPrefab, playerMoneyParent);
        playerMoneyText = uiObject.GetComponent<TextMeshProUGUI>();
    }

    private void LoadUpgrade(UpgradeData upgrade)
    {
        // PlayerPrefs'den yükseltme seviyesi yüklenir, yoksa 0 olarak ayarlanır
        int savedLevel = PlayerPrefs.GetInt(upgrade.upgradeName + "_Level", 0);
        upgradeLevels[upgrade.upgradeName] = savedLevel;
    }

    private void SaveUpgrade(UpgradeData upgrade)
    {
        // Yükseltme seviyesini kaydet
        int currentLevel = upgradeLevels[upgrade.upgradeName];
        PlayerPrefs.SetInt(upgrade.upgradeName + "_Level", currentLevel);
        PlayerPrefs.Save();
    }

    private void CreateUpgradeUI(UpgradeData upgrade)
    {
        GameObject uiObject = Instantiate(upgradeUIPrefab, upgradeUIParent);
        UpgradeUI ui = uiObject.GetComponent<UpgradeUI>();
        ui.SetUpgrade(upgrade, Upgrade); // UpgradeUI'ı ayarla
        upgradeUIs.Add(ui); // UI objesini listeye ekle
    }

    private void Upgrade(UpgradeData upgrade)
    {
        int currentLevel = upgradeLevels[upgrade.upgradeName];

        // Eğer son seviyeye ulaşılmamışsa yükseltme yapılabilir
        if (currentLevel < upgrade.upgradeLevels.Count)
        {
            UpgradeLevel levelData = upgrade.upgradeLevels[currentLevel];
            if (CanAffordUpgrade(levelData))
            {
                // Yükseltme işlemi
                ApplyUpgrade(upgrade, levelData);

                // Seviye artışı
                upgradeLevels[upgrade.upgradeName]++;
                SaveUpgrade(upgrade);

                // UI'yi güncelle
                UpdateUI(upgrade); // UI güncellemesini burada kontrol edin
            }
            else
            {
                Debug.Log("Yetersiz bakiye!");
            }
        }
        else
        {
            Debug.Log("Maksimum seviyeye ulaşıldı!");
        }

        UpdatePlayerMoneyUI(); // Yükseltmeden sonra PlayerMoney UI'sını güncelle
    }

    private bool CanAffordUpgrade(UpgradeLevel levelData)
    {
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        return playerMoney >= levelData.cost;
    }

    private void ApplyUpgrade(UpgradeData upgrade, UpgradeLevel levelData)
    {
        // Oyuncunun parasını azalt
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        playerMoney -= levelData.cost;
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
        PlayerPrefs.Save();

        // Diğer işlemler: Oyuncunun sağlığı ya da hızı gibi değerleri güncelle
        Debug.Log($"{upgrade.upgradeName} yükseltildi. Yeni Değer: {levelData.currentValue + levelData.incrementValue}");
    }

    private void UpdateUI(UpgradeData upgrade)
    {
        foreach (var ui in upgradeUIs)
        {
            if (ui.GetUpgradeName() == upgrade.upgradeName)
            {
                ui.UpdateUI();
                break;
            }
        }
    }

    private void UpdatePlayerMoneyUI()
    {
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        playerMoneyText.text = $"Money: {playerMoney}"; // Parayı TextMeshPro bileşenine ata
    }

    [Button()]
    public void SetMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", 10);
    }
    
    
}
