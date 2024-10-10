using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName; // Yükseltme adı
    public List<UpgradeLevel> upgradeLevels; // Yükseltme seviyeleri
}


[System.Serializable]
public class UpgradeLevel
{
    public int value; // Yükseltme değeri
}