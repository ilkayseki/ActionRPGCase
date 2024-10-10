using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyAttributes : Entity
{
    public string Name { get; private set; }


    private NavMeshAgent Amesh;
    
    public static event Action<GameObject> OnPokemonEnabled;
    public static event Action<GameObject> OnPokemonDisabled;

    private void OnEnable()
    {
        OnPokemonEnabled?.Invoke(gameObject);
    }
    
    private void OnDisable()
    {
        OnPokemonDisabled?.Invoke(gameObject);
    }
    
    public void SetData(EnemyData data)
    {
        Name = data.Name;
        GetComponent<EnemyMovement>().speed = data.Speed;
    }
    
    
    
    
    

}
