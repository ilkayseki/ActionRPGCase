using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText; // Yükseltme adı metni
    [SerializeField] private TextMeshProUGUI costText; // Maliyet metni
    [SerializeField] private TextMeshProUGUI currentValueText; // Geçerli değer metni
    [SerializeField] private TextMeshProUGUI nextValueText; // Sonraki değer metni
    [SerializeField] private Button upgradeButton; // Yükseltme butonu

    private UpgradeData _upgradeData; // UpgradeData referansı
    private System.Action<UpgradeData> _onUpgradeClicked; // Yükseltme butonuna tıklama olayını temsil eder

    public void SetUpgrade(UpgradeData upgradeData, System.Action<UpgradeData> onUpgradeClicked)
    {
        _upgradeData = upgradeData;
        _onUpgradeClicked = onUpgradeClicked;

        UpdateUI(); // UI'yi güncelle
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        upgradeNameText.text = _upgradeData.upgradeName; // Yükseltme adını ayarla
    }

    public string GetUpgradeName()
    {
        return _upgradeData.upgradeName; // Yükseltme adını döndür
    }

    public void UpdateUI()
    {
        int currentLevel = PlayerPrefs.GetInt(_upgradeData.upgradeName + "_Level", 0);
    
        // _upgradeData'nın null olup olmadığını kontrol et
        if (_upgradeData == null)
        {
            Debug.LogError("UpgradeData is null!");
            return;
        }
    
        if (currentLevel < _upgradeData.upgradeLevels.Count)
        {
            UpgradeLevel levelData = _upgradeData.upgradeLevels[currentLevel];
            currentValueText.text = $"Current: {levelData.value}"; // Geçerli değeri göster
        
            // Sonraki seviyenin değerini kontrol et
            if (currentLevel + 1 < _upgradeData.upgradeLevels.Count)
            {
                nextValueText.text = $"Next: {_upgradeData.upgradeLevels[currentLevel + 1].value}"; // Sonraki değer
                upgradeButton.interactable = true; // Butonu aktif et
            }
            else
            {
                costText.text = "Cost: N/A"; // Maksimum seviyeye ulaşıldığında maliyeti gizle
                currentValueText.text = $"Current: {levelData.value}";
                nextValueText.text = $"Next: -";
                upgradeButton.interactable = false; // Butonu devre dışı bırak
            }

           
        }
        
    }

    private void OnUpgradeButtonClicked()
    {
        _onUpgradeClicked?.Invoke(_upgradeData); // Yükseltmeyi uygula
    }
}
