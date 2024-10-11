using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    public static event Action OnGameOver;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Çarpılan nesne EnemyAttributes bileşenine sahipse
        if (hit.gameObject.GetComponent<EnemyAttributes>())
        {
            hit.gameObject.GetComponent<EnemyHealth>().DestroyYourself();
            
            health--;
            if (health <= 0)
            {
                OnGameOver.Invoke();
                Time.timeScale = 0;
            }
        }
    }

    public void SetHealth(int h)
    {
        health = h;
    }
    
}
