using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CandyFactoryApp.Models;

public class Factory
{
    private readonly Random _random = new();
    private int _sugarAmount = 1000;
    private bool _isRunning;
    
    public string Name { get; }
    public List<IFactoryTechnic> Technics { get; } = new();
    public int CandiesProduced { get; private set; }
    public int SugarAmount
    {
        get => _sugarAmount;
        private set
        {
            if (_sugarAmount != value)
            {
                _sugarAmount = value;
                SugarAmountChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public event Action<string>? OnSugarFinished;
    public event EventHandler? SugarAmountChanged;
    public event EventHandler? ProductionStatusChanged;
    
    public Factory(string name)
    {
        Name = name;
    }
    
    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            if (_isRunning != value)
            {
                _isRunning = value;
                ProductionStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public async Task StartProductionAsync(CancellationToken cancellationToken)
    {
        IsRunning = true;
        
        while (!cancellationToken.IsCancellationRequested && IsRunning)
        {
            await Task.Delay(500, cancellationToken);
            
            if (SugarAmount <= 0)
            {
                OnSugarFinished?.Invoke($"{Name}: Sugar has finished!");
                IsRunning = false;
                continue;
            }
            
            SugarAmount -= 10;
            CandiesProduced += 5;
            
            // 2% chance of random event
            if (_random.Next(100) < 2)
            {
                var randomEvent = _random.Next(2);
                if (randomEvent == 0 && SugarAmount > 0)
                {
                    OnSugarFinished?.Invoke($"{Name}: False alarm! Sugar is still available.");
                }
            }
        }
    }
    
    public void StopProduction()
    {
        IsRunning = false;
    }
    
    public void AddSugar(int amount)
    {
        SugarAmount += amount;
    }
}