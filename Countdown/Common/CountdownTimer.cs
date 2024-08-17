﻿using System.Timers;
using Timer = System.Timers.Timer;


public class CountdownTimer
{
    private int _timeLeft;
    private int _seconds;
    private readonly Timer _timer;

    public event Action<int>? OnTimeChanged;
    public event Action? OnTimerFinished;

    public CountdownTimer(int seconds)
    {
        _seconds = seconds;
        _timeLeft = seconds;
        _timer = new Timer(1000);
        _timer.Elapsed += HandleTimerElapsed;
    }

    ~CountdownTimer()
    {
        _timer.Elapsed -= HandleTimerElapsed; 
    }

    public void Start()
    {
        Task.Run(() => _timer.Start());
    }

    public void Stop()
    {
        _timer.Stop();
        _timeLeft = _seconds;
    }

    private void HandleTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _timeLeft--;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            OnTimeChanged?.Invoke(_timeLeft);
        });

        if (_timeLeft <= 0)
        {
            Stop();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                OnTimerFinished?.Invoke();
            });
        }
    }
}

