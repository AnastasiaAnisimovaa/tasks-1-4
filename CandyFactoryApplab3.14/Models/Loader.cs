using System;
using System.Threading;
using System.Threading.Tasks;

namespace CandyFactoryApp.Models;

public class Loader : IFactoryTechnic
{
    private readonly Random _random = new();
    private bool _isWorking;
    
    public string Name { get; }
    public bool IsWorking
    {
        get => _isWorking;
        private set
        {
            if (_isWorking != value)
            {
                _isWorking = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public event Action<string>? OnMalfunction;
    public event EventHandler? StatusChanged;
    
    public Loader(string name)
    {
        Name = name;
    }
    
    public async Task StartWorkingAsync(CancellationToken cancellationToken)
    {
        IsWorking = true;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);
            
            // 5% chance of malfunction
            if (_random.Next(100) < 5)
            {
                OnMalfunction?.Invoke($"{Name} malfunction occurred!");
                IsWorking = false;
                return;
            }
        }
    }
    
    public void StopWorking()
    {
        IsWorking = false;
    }
}