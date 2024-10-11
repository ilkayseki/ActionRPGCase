using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> upgrades; // Yükseltme verileri
    [SerializeField] private GameObject upgradeUIPrefab; // Yükseltme UI prefabı
    [SerializeField] private Transform upgradeUIParent; // UI objeleri için parent
    [SerializeField] private GameObject playerMoneyPrefab; // Player money texti
    [SerializeField] private Transform playerMoneyParent; // Player money texti için parent
    [SerializeField] private CostData costData; // CostData ScriptableObject'i
    private TextMeshProUGUI playerMoneyText; // Player money text bileşeni
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Yükseltme seviyeleri
    private List<UpgradeUI> upgradeUIs = new List<UpgradeUI>(); // Yükseltme UI bileşenleri

    private void Start()
    {
        costData.LoadCostIndex(); // Maliyet indeksini yükle
        CreatePlayerMoneyUI();
        UpdatePlayerMoneyUI(); // Başlangıçta PlayerMoney UI'sını güncelle

        foreach (var upgrade in upgrades)
        {
            LoadUpgrade(upgrade); // Yükseltme seviyesini yükle
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
        string key = upgrade.upgradeName + "_Level";

        // Eğer PlayerPrefs'te bu anahtar yoksa, ilk seviyeyi ayarlıyoruz
        if (!PlayerPrefs.HasKey(key))
        {
            Debug.LogError("Yok");

            int initialLevel = 0; // Listenin ilk seviyesi
            PlayerPrefs.SetInt(key, initialLevel);
            PlayerPrefs.Save();
            upgradeLevels[upgrade.upgradeName] = initialLevel;
            
            
            
            
        }
        else
        {
            int savedLevel = PlayerPrefs.GetInt(key);
            upgradeLevels[upgrade.upgradeName] = savedLevel;
        }
    }

    private void SaveUpgrade(UpgradeData upgrade)
    {
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

        if (currentLevel < upgrade.upgradeLevels.Count)
        {
            int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al

            if (CanAffordUpgrade(cost)) // Maliyet kontrolü
            {
                ApplyUpgrade(upgrade);

                // Seviye artışı
                upgradeLevels[upgrade.upgradeName]++;
                SaveUpgrade(upgrade);

                // UI'yi güncelle
                UpdateUI(upgrade);

                // Maliyeti artır
                costData.IncreaseCostIndex(); // Maliyet indeksini artır
                costData.SaveCostIndex(); // Maliyet indeksini kaydet
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

    private bool CanAffordUpgrade(int cost)
    {
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        return playerMoney >= cost;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al

        playerMoney -= cost; // Oyuncunun parasını azalt
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
        PlayerPrefs.Save();

        // Diğer işlemler: Oyuncunun sağlığı ya da hızı gibi değerleri güncelle
        Debug.Log($"{upgrade.upgradeName} yükseltildi.");
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
        PlayerPrefs.SetInt("PlayerMoney", 100); // Örnek para değeri
    }

    [Button()]
    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    [Button()]
    private void RandomUpgrade()
    {
        int randomIndex = Random.Range(0, upgrades.Count);
        UpgradeData upgrade = upgrades[randomIndex];

        Upgrade(upgrade); // Rastgele yükseltmeyi uygula
    }
    
    
}
