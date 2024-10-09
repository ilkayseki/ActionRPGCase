using System;
using UnityEngine;
public class Pokemon : Entity
{
    public string Name { get; private set; }

    
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
    
    
    public void SetData(PokemonData data) => Name = data.Name;

}
