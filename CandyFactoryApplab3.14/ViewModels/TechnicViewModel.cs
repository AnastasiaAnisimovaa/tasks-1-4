using System;
using System.Threading;
using System.Threading.Tasks;
using CandyFactoryApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CandyFactoryApp.ViewModels;

public partial class TechnicViewModel : ViewModelBase
{
    private readonly IFactoryTechnic _technic;
    
    [ObservableProperty]
    private string _status = "Stopped";
    
    public event Action<string>? OnEvent;
    
    public string Name => _technic.Name;
    
    public TechnicViewModel(IFactoryTechnic technic)
    {
        _technic = technic;
        _technic.OnMalfunction += OnTechnicMalfunction;
        
        if (technic is Loader loader)
        {
            loader.StatusChanged += OnTechnicStatusChanged;
        }
    }
    
    private void OnTechnicStatusChanged(object? sender, EventArgs e)
    {
        Status = _technic.IsWorking ? "Working" : "Stopped";
    }
    
    private void OnTechnicMalfunction(string message)
    {
        OnEvent?.Invoke(message);
    }
    
    public async Task StartWorkingAsync(CancellationToken cancellationToken)
    {
        if (_technic is Loader loader)
        {
            await loader.StartWorkingAsync(cancellationToken);
        }
    }
    
    [RelayCommand]
    private void StopWorking()
    {
        if (_technic is Loader loader)
        {
            loader.StopWorking();
        }
    }
}