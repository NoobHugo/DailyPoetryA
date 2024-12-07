using System;
using Avalonia;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPoetryA;

public class ServiceLocator
{
    private readonly IServiceProvider _serviceProvider;
    
    public ResultViewModel ResultViewModel  => _serviceProvider.GetRequiredService<ResultViewModel>();

    private static ServiceLocator? _current;

    public static ServiceLocator Current
    {
        get
        {
            if (_current is not null)
            {
                return _current;
            }

            if (Application.Current!.TryGetResource(nameof(ServiceLocator), null, out var locator)
                && locator is ServiceLocator serviceLocator)
            {
                return _current = serviceLocator;
            }

            throw new Exception("Could not find service locator");
        }
    }

    public ServiceLocator()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        services.AddSingleton<IPoetryStorage, PoetryStorage>();
        services.AddSingleton<ResultViewModel>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
}