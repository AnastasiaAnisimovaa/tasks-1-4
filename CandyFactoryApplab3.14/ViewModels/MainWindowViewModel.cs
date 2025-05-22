using System.Collections.ObjectModel;
using CandyFactoryApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CandyFactoryApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<FactoryViewModel> _factories = new();
    
    [RelayCommand]
    private void AddFactory()
    {
        var factory = new Factory($"Factory {Factories.Count + 1}");
        
        var loader1 = new Loader($"Loader {Factories.Count + 1}-1");
        var loader2 = new Loader($"Loader {Factories.Count + 1}-2");
        
        factory.Technics.Add(loader1);
        factory.Technics.Add(loader2);
        
        Factories.Add(new FactoryViewModel(factory));
    }
    
    [RelayCommand]
    private void RemoveFactory(FactoryViewModel factory)
    {
        Factories.Remove(factory);
    }
}