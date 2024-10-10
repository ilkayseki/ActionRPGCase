using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory<T> : IEntityFactory<T> where T : EnemyAttributes
{
    private EnemyData[] _data;

    public EnemyFactory(EnemyData[] data) => _data=data;


    public T Create(Transform spawnPoint)
    {
        var data = _data[Random.Range(0, _data.Length)];
        var instance = Object.Instantiate(data.Prefab, spawnPoint);

        T enemy = instance.GetComponent<T>();
        
        enemy.SetData(data);
        return enemy;
    }
}
