using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearSpawnStrategy : ISpawnStrategy
{
    private int _index = 0;
    private Transform[] _spawnPoints;

    public LinearSpawnStrategy(Transform[] spawnPoints) => _spawnPoints = spawnPoints;

    public Transform GetSpawnPoint()
    {
        Transform spawnPoint = _spawnPoints[_index];
        _index = (_index + 1) % _spawnPoints.Length;
        return spawnPoint;
    }
}
