using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentValueText;
    [SerializeField] private TextMeshProUGUI newValueText;
    [SerializeField] private Button upgradeButton;

    private UpgradeData _upgradeData;
    private System.Action<UpgradeData> _onUpgradeClicked;
    private int _currentLevel;

    public void SetUpgrade(UpgradeData upgradeData, System.Action<UpgradeData> onUpgradeClicked)
    {
        _upgradeData = upgradeData;
        _onUpgradeClicked = onUpgradeClicked;

        // Kaydedilmiş seviye bilgilerini al
        _currentLevel = PlayerPrefs.GetInt(upgradeData.upgradeName + "_Level", 0);

        UpdateUI();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    public string GetUpgradeName()
    {
        return _upgradeData.upgradeName;
    }

    public void UpdateUI()
    {
        if (_currentLevel < _upgradeData.upgradeLevels.Count)
        {
            UpgradeLevel levelData = _upgradeData.upgradeLevels[_currentLevel];
            upgradeNameText.text = _upgradeData.upgradeName;
            costText.text = $"Cost: {levelData.cost}";
            currentValueText.text = $"Current: {levelData.currentValue}";
            newValueText.text = $"New: {levelData.currentValue + levelData.incrementValue}";
        }
        else
        {
            upgradeNameText.text = $"{_upgradeData.upgradeName} (Max Level)";
            costText.text = "N/A";
            currentValueText.text = "Max Level";
            newValueText.text = "Max Level";
            upgradeButton.interactable = false;
        }
    }

    private void OnUpgradeButtonClicked()
    {
        _onUpgradeClicked?.Invoke(_upgradeData);
        _currentLevel++;
    }
}