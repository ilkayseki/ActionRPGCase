using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner <T> where T : Entity
{
   private IEntityFactory<T> _factory;
   private ISpawnStrategy _strategy;


   public EntitySpawner(IEntityFactory<T> factory, ISpawnStrategy strategy)
   {
      _factory = factory;
      _strategy = strategy;
      
   }

   public T Spawn() => _factory.Create(_strategy.GetSpawnPoint());
   
}
