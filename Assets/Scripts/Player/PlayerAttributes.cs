using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    private int healthLevel;
    private int powerLevel;
    private int speedLevel;
    private int attackSpeedLevel;
    
    
    private void Start()
    {
        GetPlayerAttributes();
        
        //SetPlayerAttributes();
    }

    private void GetPlayerAttributes()
    {
        if (!PlayerPrefs.HasKey("Health_Level"))
        {
            Debug.LogError("Yok");
        }
        healthLevel = PlayerPrefs.GetInt("Health_Level");
        powerLevel = PlayerPrefs.GetInt("Power_Level");
        speedLevel = PlayerPrefs.GetInt("Speed_Level");
        attackSpeedLevel = PlayerPrefs.GetInt("AttackSpeed_Level");
    }

    private void SetPlayerAttributes()
    {
        GetComponent<PlayerHealth>().SetHealth(healthLevel);
        
        GetComponent<PlayerMovement>().SetSpeed(speedLevel);

        GetComponentInChildren<ShootingController>().SetBulletDamage(powerLevel);
        
        GetComponentInChildren<ShootingController>().SetAttackSpeed(attackSpeedLevel);
        
    }
    
    
}
