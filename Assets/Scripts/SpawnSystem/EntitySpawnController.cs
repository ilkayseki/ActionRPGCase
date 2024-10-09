using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySpawnController : MonoBehaviour
{
    [SerializeField] protected SpawnStrategyType StrategyType;
    [SerializeField] protected Transform[] SpawnPoints;
    protected ISpawnStrategy SpawnStrategy;
    protected enum SpawnStrategyType { Linear,Random}

    protected virtual void Awake()
    {
        SpawnStrategy = StrategyType switch
        {
            SpawnStrategyType.Random => new RandomSpawnStrategy(SpawnPoints),
            _ => new LinearSpawnStrategy(SpawnPoints)
        };
    }

    public abstract void Spawn();


}
