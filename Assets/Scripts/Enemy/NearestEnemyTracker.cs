using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NearestEnemyTracker : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    private List<GameObject> _enemies = new List<GameObject>();
    
    private void OnEnable()
    {
        EnemyAttributes.OnEnemyEnabled += AddEnemy;
        EnemyAttributes.OnEnemyDisabled += RemoveEnemy;
    }

    private void OnDisable()
    {
        EnemyAttributes.OnEnemyEnabled -= AddEnemy;
        EnemyAttributes.OnEnemyDisabled -= RemoveEnemy;
    }

    private void AddEnemy(GameObject enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
    }

   
    public Transform GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        if (_enemies.Count==0||_enemies==null)
        {
            return this.transform;
        }
        
        foreach (GameObject enemy in _enemies)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy.transform;
    }
}