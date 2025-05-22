using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CandyFactoryApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CandyFactoryApp.ViewModels;

public partial class FactoryViewModel : ViewModelBase
{
    private readonly Factory _factory;
    private CancellationTokenSource? _cts;
    
    [ObservableProperty]
    private string _status = "Stopped";
    
    [ObservableProperty]
    private string _sugarStatus = "1000";
    
    [ObservableProperty]
    private string _productionStatus = "0";
    
    public string Name => _factory.Name;
    public ObservableCollection<string> EventLogs { get; } = new();
    public ObservableCollection<TechnicViewModel> Technics { get; } = new();
    
    public FactoryViewModel(Factory factory)
    {
        _factory = factory;
        _factory.OnSugarFinished += OnFactoryEvent;
        _factory.ProductionStatusChanged += OnProductionStatusChanged;
        _factory.SugarAmountChanged += OnSugarAmountChanged;
        
        foreach (var technic in factory.Technics)
        {
            var technicVm = new TechnicViewModel(technic);
            technicVm.OnEvent += OnTechnicEvent;
            Technics.Add(technicVm);
        }
    }
    
    private void OnSugarAmountChanged(object? sender, EventArgs e)
    {
        SugarStatus = _factory.SugarAmount.ToString();
    }
    
    private void OnProductionStatusChanged(object? sender, EventArgs e)
    {
        Status = _factory.IsRunning ? "Running" : "Stopped";
        ProductionStatus = _factory.CandiesProduced.ToString();
    }
    
    private void OnFactoryEvent(string message)
    {
        EventLogs.Add($"[{DateTime.Now:T}] {message}");
    }
    
    private void OnTechnicEvent(string message)
    {
        EventLogs.Add($"[{DateTime.Now:T}] {message}");
    }
    
    [RelayCommand]
    private void AddSugar()
    {
        _factory.AddSugar(500);
    }
    
    [RelayCommand]
    private async Task ToggleProduction()
    {
        if (_factory.IsRunning)
        {
            _cts?.Cancel();
            _factory.StopProduction();
        }
        else
        {
            _cts = new CancellationTokenSource();
            await _factory.StartProductionAsync(_cts.Token);
            
            foreach (var technicVm in Technics)
            {
                await technicVm.StartWorkingAsync(_cts.Token);
            }
        }
    }
}