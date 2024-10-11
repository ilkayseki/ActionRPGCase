using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Upgrade System/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName; // Yükseltmenin adı
    public List<UpgradeLevel> upgradeLevels = new List<UpgradeLevel>(); // Yükseltme seviyeleri

    [HideInInspector] public int currentLevel; // Mevcut seviye
    [HideInInspector] public float currentValue; // Mevcut seviyenin değeri
}

[System.Serializable]
public class UpgradeLevel
{
    public float value; // Bu seviyedeki değer
}