using System;

namespace CandyFactoryApp.Models;

public interface IFactoryTechnic
{
    string Name { get; }
    bool IsWorking { get; }
    event Action<string>? OnMalfunction;
}