using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        // Çarpılan nesne EnemyAttributes bileşenine sahipse
        if (hit.gameObject.GetComponent<EnemyAttributes>())
        {
            hit.gameObject.GetComponent<EnemyHealth>().DestroyYourself();
            
            health--;
            if (health <= 0)
            {
                Time.timeScale = 0;
            }
        }
    }
}
