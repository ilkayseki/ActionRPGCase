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
        
        SetPlayerAttributes();
    }

    private void GetPlayerAttributes()
    {
        healthLevel = (int)PlayerPrefs.GetFloat("Health_Value");
        powerLevel = (int)PlayerPrefs.GetFloat("Power_Value");
        speedLevel = (int)PlayerPrefs.GetFloat("Speed_Value");
        attackSpeedLevel =(int) PlayerPrefs.GetFloat("AttackSpeed_Value");
    }

    private void SetPlayerAttributes()
    {
        GetComponent<PlayerHealth>().SetHealth(healthLevel);
        
        GetComponent<PlayerMovement>().SetSpeed(speedLevel);

        GetComponentInChildren<ShootingController>().SetBulletDamage(powerLevel);
        
        GetComponentInChildren<ShootingController>().SetAttackSpeed(attackSpeedLevel);
        
    }
    
    
}
