using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PokemonSpawnController : EntitySpawnController
{
    [SerializeField] private PokemonData[] _data;
    [SerializeField] private float _interval = 1.0f; // 1 saniye aralıkla spawn edilecek

    private EntitySpawner<Pokemon> _spawner;
    private CountDownTimer _timer;

    protected override void Awake()
    {
        base.Awake();
        _spawner = new(new PokemonFactory<Pokemon>(_data), SpawnStrategy);
        _timer = SetTimer();
    }

    private void Start() => _timer.Start();
    private void Update() => _timer.Tick(Time.deltaTime);
    
    public override void Spawn() => _spawner.Spawn();

    private CountDownTimer SetTimer()
    {
        CountDownTimer timer = new(_interval);
        timer.OnStop += () =>
        {
            Spawn(); // Timer durduğunda spawn yapar
            timer.Reset(); // Timer yeniden başlatılır
            timer.Start(); // Timer tekrar başlatılır
        };
        return timer;
    }
}