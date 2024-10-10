using UnityEngine;

[CreateAssetMenu(fileName = "CostData", menuName = "ScriptableObjects/CostData")]
public class CostData : ScriptableObject
{
    public int[] costs; // Maliyetler dizisi
    public int currentLevelCostIndex; // Mevcut maliyet indeksi

    public int GetCost(int level)
    {
        if (level < 0 || level >= costs.Length)
        {
            Debug.LogError($"Invalid level: {level}. Valid range: 0 to {costs.Length - 1}.");
            return costs[costs.Length - 1]; // Maksimum seviyeye ulaşıldığında son değeri döndür
        }
        return costs[level];
    }

    public void IncreaseCostIndex()
    {
        currentLevelCostIndex++;
        if (currentLevelCostIndex >= costs.Length)
        {
            Debug.LogWarning("Maksimum maliyet seviyesine ulaşıldı.");
            currentLevelCostIndex = costs.Length - 1; // Sınırda kalmak için en yüksek değeri kullan
        }
    }

    public void SaveCostIndex()
    {
        PlayerPrefs.SetInt("CurrentLevelCostIndex", currentLevelCostIndex);
        PlayerPrefs.Save();
    }

    public void LoadCostIndex()
    {
        currentLevelCostIndex = PlayerPrefs.GetInt("CurrentLevelCostIndex", 0);
    }
}