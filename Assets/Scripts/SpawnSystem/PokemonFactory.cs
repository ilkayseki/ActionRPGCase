using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonFactory<T> : IEntityFactory<T> where T : Pokemon
{
    private PokemonData[] _data;

    public PokemonFactory(PokemonData[] data) => _data=data;


    public T Create(Transform spawnPoint)
    {
        var data = _data[Random.Range(0, _data.Length)];
        var instance = Object.Instantiate(data.Prefab, spawnPoint);

        T pokemon = instance.GetComponent<T>();
        
        pokemon.SetData(data);
        return pokemon;
    }
}
