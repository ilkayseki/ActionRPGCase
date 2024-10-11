using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> upgrades; // Yükseltme verileri
    [SerializeField] private GameObject upgradeUIPrefab; // Yükseltme UI prefabı
    [SerializeField] private Transform upgradeUIParent; // UI objeleri için parent
    [SerializeField] private GameObject playerMoneyPrefab; // Player money texti
    [SerializeField] private Transform playerMoneyParent; // Player money texti için parent
    [SerializeField] private GameObject randomUpgradePrefab; // Player money texti
    [SerializeField] private Transform randomUpgradeParent; // Player money texti için parent
    [SerializeField] private CostData costData; // CostData ScriptableObject'i
    private TextMeshProUGUI playerMoneyText; // Player money text bileşeni
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Yükseltme seviyeleri
    private List<UpgradeUI> upgradeUIs = new List<UpgradeUI>(); // Yükseltme UI bileşenleri

    
    #region Animation

    
    [HideInInspector]public List<GameObject> upgradeButtons; // Upgrade butonlarının Image bileşenleri
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color finalColor = Color.red;
    [SerializeField] private float flashCount  = 3f;
    [SerializeField] private float highlightDuration = 0.1f;
    [SerializeField] private float finalColorDuration = 3f;
    private int originalIndex;
    private UpgradeData selectedUpgrade;
    
    
    #endregion

    
    
    private void Start()
    {
        costData.LoadCostIndex(); // Maliyet indeksini yükle
        CreatePlayerMoneyUI();
        CreateRandomUpgradeUI();
        UpdatePlayerMoneyUI(); // Başlangıçta PlayerMoney UI'sını güncelle

        foreach (var upgrade in upgrades)
        {
            LoadUpgrade(upgrade); // Yükseltme seviyesini yükle
            CreateUpgradeUI(upgrade); // UI objelerini oluştur
        }
    }

    private void CreateRandomUpgradeUI()
    {
        GameObject uiObject = Instantiate(randomUpgradePrefab, randomUpgradeParent);
        uiObject.GetComponent<Button>().onClick.AddListener(RandomUpgrade);

    }
    
    private void CreatePlayerMoneyUI()
    {
        GameObject uiObject = Instantiate(playerMoneyPrefab, playerMoneyParent);
        playerMoneyText = uiObject.GetComponent<TextMeshProUGUI>();
    }

    private void LoadUpgrade(UpgradeData upgrade)
    {
        string levelKey = upgrade.upgradeName + "_Level";
        string valueKey = upgrade.upgradeName + "_Value";
    
        // Eğer PlayerPrefs'te seviye kaydı yoksa yeni kayıt oluştur
        if (!PlayerPrefs.HasKey(levelKey))
        {
            int initialLevel = 0;
            float initialValue = upgrade.upgradeLevels[initialLevel].value;

            PlayerPrefs.SetInt(levelKey, initialLevel);
            PlayerPrefs.SetFloat(valueKey, initialValue);
            PlayerPrefs.Save();

            upgrade.currentLevel = initialLevel;
            upgrade.currentValue = initialValue;
        }
        else
        {
            // Kayıtlı olan seviyeyi ve değeri yükle
            int savedLevel = PlayerPrefs.GetInt(levelKey);
            float savedValue = PlayerPrefs.GetFloat(valueKey);

            upgrade.currentLevel = savedLevel;
            upgrade.currentValue = savedValue;
        }

        // Dictionary güncellemesi
        upgradeLevels[upgrade.upgradeName] = upgrade.currentLevel;
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
        
        upgradeButtons.Add(uiObject);
        
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

                // Maliyeti artır
                costData.IncreaseCostIndex(); // Maliyet indeksini artır
                costData.SaveCostIndex(); // Maliyet indeksini kaydet
                
                
                StartCoroutine(UpgradeAnimationSequence());
                
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
        // Mevcut seviye
        int currentLevel = upgrade.currentLevel;

        // Eğer son seviyeye ulaşılmamışsa yükseltme yapılabilir
        if (currentLevel < upgrade.upgradeLevels.Count - 1)
        {
            int nextLevel = currentLevel + 1;
            float nextValue = upgrade.upgradeLevels[nextLevel].value;

            // Mevcut seviyeyi ve değeri güncelle
            upgrade.currentLevel = nextLevel;
            upgrade.currentValue = nextValue;

            // PlayerPrefs'e kaydet
            string levelKey = upgrade.upgradeName + "_Level";
            string valueKey = upgrade.upgradeName + "_Value";

            PlayerPrefs.SetInt(levelKey, nextLevel);
            PlayerPrefs.SetFloat(valueKey, nextValue);
            PlayerPrefs.Save();

            
            int playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
            int cost = costData.GetCost(costData.currentLevelCostIndex); // Geçerli maliyeti al

            playerMoney -= cost; // Oyuncunun parasını azalt
            PlayerPrefs.SetInt("PlayerMoney", playerMoney);
            PlayerPrefs.Save();
            
            Debug.Log($"{upgrade.upgradeName} yükseltildi. Yeni Seviye: {nextLevel}, Yeni Değer: {nextValue}");
        }
        else
        {
            Debug.Log($"{upgrade.upgradeName} maksimum seviyeye ulaştı!");
        }
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
        // Eğer "PlayerMoney" PlayerPrefs'te yoksa, 0 değeri ile oluştur
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            PlayerPrefs.SetInt("PlayerMoney", 0);
        }
    
        // PlayerMoney'yi al ve UI'ya ata
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney");
        playerMoneyText.text = $"Money: {playerMoney}"; // Parayı TextMeshPro bileşenine ata
    }

    public void RandomUpgrade()
    {
        // Maksimum seviyeye ulaşmayan yükseltmeleri filtrele
        List<UpgradeData> availableUpgrades = new List<UpgradeData>();

        foreach (var upgrade in upgrades)
        {
            if (upgrade.currentLevel < upgrade.upgradeLevels.Count - 1)
            {
                availableUpgrades.Add(upgrade);
            }
        }

        // Eğer hiç yükseltilebilir upgrade yoksa çık
        if (availableUpgrades.Count == 0)
        {
            Debug.Log("Yükseltilebilecek başka upgrade kalmadı!");
            return;
        }
        /*
         buraya, upgradeButtons listesini kontrol edecek kodu yaz
         upgradeButtons içinde upradeUI classının içindeki upgrade data ile  availableUpgrades listesindeki dataları karşılaştır.
         eğer upgradeButtons içinde availableUpgrades listesinde olmayan bir obje varsa sil
        */
        for (int i = upgradeButtons.Count - 1; i >= 0; i--) // Ters döngü, listeyi güvenli bir şekilde silmek için
        {
            UpgradeData buttonUpgradeData = upgradeButtons[i].GetComponent<UpgradeUI>().GetUpgradeData(); // UpgradeUI sınıfındaki upgradeData'yı al
            if (!availableUpgrades.Contains(buttonUpgradeData)) // Eğer availableUpgrades içinde yoksa
            {
                Debug.Log($"Siliniyor: {buttonUpgradeData.upgradeName}");
                upgradeButtons.RemoveAt(i); // Listeyi güncelle
            }
        }
        
        
        // Rastgele bir yükseltme seç
        int randomIndex = Random.Range(0, availableUpgrades.Count);
        selectedUpgrade = availableUpgrades[randomIndex];

        
       originalIndex = upgradeButtons.FindIndex(button => 
            button.GetComponent<UpgradeUI>().GetUpgradeData() == selectedUpgrade); 
        
        
        // Seçilen yükseltmeyi uygula
        Upgrade(selectedUpgrade);
        
    }



    private IEnumerator UpgradeAnimationSequence()
    {
        // Rastgele 6 buton seçimi için bir liste oluştur
        List<Image> randomButtons = new List<Image>();

        for (int i = 0; i < flashCount; i++)
        {
            Image randomButton = upgradeButtons[Random.Range(0, upgradeButtons.Count)].GetComponent<Image>();
            randomButtons.Add(randomButton);
            
            // Highlight yap (renk değişimi)
            randomButton.DOColor(highlightColor, highlightDuration).SetLoops(2, LoopType.Yoyo);
            
            // Bekleme
            yield return new WaitForSeconds(highlightDuration * 2);
        }

        // Son butonu al ve kırmızıya çevir
        Image finalButton = upgradeButtons[originalIndex].GetComponent<Image>();
        Color originalColor = finalButton.color;

        finalButton.DOColor(finalColor, 0.5f);

        // 3 saniye bekle
        yield return new WaitForSeconds(finalColorDuration);

        // Eski rengine geri dön
        finalButton.DOColor(originalColor, 0.5f);
        
        // UI'yi güncelle
        UpdateUI(selectedUpgrade);
        
    }
    
  
    [Button()]
    public void SetMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", 100); // Örnek para değeri
        PlayerPrefs.Save();
        UpdatePlayerMoneyUI();
    }

    [Button()]
    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        foreach (var var in upgrades)
        {
            UpdateUI(var);
        }
    }
    
    
}
