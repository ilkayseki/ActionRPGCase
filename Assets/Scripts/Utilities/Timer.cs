using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utilities
{
    public abstract class Timer
    {
        protected float _startTime;
        protected float _time;
        
        public bool Running { get; protected set; }

        public float Progress => _time / _startTime;

        public Action OnStart = delegate{};
        public Action OnStop = delegate{};

        protected Timer(float value)
        {
            _startTime = value;
            Running = false;
        }

        public void Start()
        {
            _time = _startTime;
            if (!Running)
            {
                Running = true;
                OnStart.Invoke();
            }
        }
        public void Stop()
        {
            if (Running)
            {
                Running = false;
                OnStop.Invoke();
            }
        }

        public abstract void Tick(float deltaTime);
    }


    public class CountDownTimer : Timer
    {
        public CountDownTimer(float value) : base(value) {}

        public override void Tick(float deltaTime)
        {
            if (Running)
            {
                _time -= deltaTime;
                if (_time <= 0)
                {
                    Stop();
                }
            }
        }

        public bool Finished() => _time <= 0;
        public void Reset() => _time = _startTime;

        public void Reset(float time)
        {
            _startTime = time;
            Reset();
        }
    }

    public class StopWatchTimer : Timer
    {
        public StopWatchTimer(float value) : base(0f) { }

        public override void Tick(float deltaTime)
        {
            if (Running) _time += deltaTime;
        }

        public void Reset() => _time = 0f;
    }
    
}