using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSpawnStrategy : ISpawnStrategy
{
   private List<Transform> _unusedSpawnPoints;
   private Transform[] _spawnPoints;

   public RandomSpawnStrategy(Transform[] spawnPoints)
   {
      _spawnPoints = spawnPoints;
      _unusedSpawnPoints = new(spawnPoints);
   }

   public Transform GetSpawnPoint()
   {
      if (!_unusedSpawnPoints.Any()) _unusedSpawnPoints = new(_spawnPoints);
      int index = Random.Range(0, _unusedSpawnPoints.Count);
      Transform spawnPoint = _unusedSpawnPoints[index];
      _unusedSpawnPoints.RemoveAt(index);
      return spawnPoint;
   }


}
