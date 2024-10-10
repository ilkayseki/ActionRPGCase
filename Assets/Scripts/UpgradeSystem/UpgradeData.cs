using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName; // Yükseltme adı (örn: "Health")
    public List<UpgradeLevel> upgradeLevels; // Her seviyedeki değerler
}

[System.Serializable]
public class UpgradeLevel
{
    public int cost;          // Bu seviyedeki yükseltme maliyeti
    public int currentValue;  // Bu seviyedeki mevcut değer
    public int incrementValue; // Bu seviyedeki artış değeri
}