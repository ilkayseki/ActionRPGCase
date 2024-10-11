using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyAttributes : Entity
{
    public string Name { get; private set; }


    private NavMeshAgent Amesh;
    
    public static event Action<GameObject> OnEnemyEnabled;
    public static event Action<GameObject> OnEnemyDisabled;

    private void OnEnable()
    {
        OnEnemyEnabled?.Invoke(gameObject);
    }
    
    private void OnDisable()
    {
        OnEnemyDisabled?.Invoke(gameObject);
    }
    
    public void SetData(EnemyData data)
    {
        Name = data.Name;
        GetComponent<EnemyMovement>().speed = data.Speed;

        GetComponent<EnemyHealth>().health = data.Health;

        GetComponent<EnemyHealth>().rewardedGold = data.RewardedGold;

        GetComponent<EnemyHealth>().goldboxPrefab = data.GolBoxPrefab;
    }
    
    
    
    
    

}
