using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace CoreLogicTesting;

public class OnServiceInjectionExtensionTesting
{
    private interface ITestingScoped
    {
    }
    
    [Registration(Type = RegistrationKind.Scoped)]
    private class TestingScoped : ITestingScoped
    {
    }
    
    private interface ITestingSingleton
    {
    }
    
    [Registration(Type = RegistrationKind.Singleton)]
    private class TestingSingleton : ITestingSingleton
    {
    }

    private interface ITestingTransient
    {
    }
    
    [Registration(Type = RegistrationKind.Transient)]
    private class TestingTransient : ITestingTransient
    {
    }
    
    [Fact]
    public void InjectServices_RegistersServices_Based_On_The_Specified_Life_Time_Ok()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.InjectServices("CoreLogicTesting");

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var scopedService = serviceProvider.GetService<ITestingScoped>();
        var singletonService = serviceProvider.GetService<ITestingSingleton>();
        var transientService = serviceProvider.GetService<ITestingTransient>();
        
        Assert.NotNull(scopedService);
        Assert.NotNull(singletonService);
        Assert.NotNull(transientService);
    }
    
    [Fact]
    public void InjectServices_Services_Have_Correct_Type_After_Registration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.InjectServices("CoreLogicTesting");

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var scopedService = serviceProvider.GetService<ITestingScoped>();
        var singletonService = serviceProvider.GetService<ITestingSingleton>();
        var transientService = serviceProvider.GetService<ITestingTransient>();

        Assert.IsType<TestingScoped>(scopedService);
        Assert.IsType<TestingSingleton>(singletonService);
        Assert.IsType<TestingTransient>(transientService);
    }
    
    [Fact]
    public void InjectServices_Services_Have_Correct_LifeTime_After_Registration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.InjectServices("CoreLogicTesting");

        var scopedServiceDescriptor =  
            serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITestingScoped));
        
        var singletonServiceDescriptor =
            serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITestingSingleton));
        
        var transientServiceDescriptor =  
            serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITestingTransient));
        
        Assert.Equal(ServiceLifetime.Scoped, scopedServiceDescriptor!.Lifetime);
        Assert.Equal(ServiceLifetime.Singleton, singletonServiceDescriptor!.Lifetime);
        Assert.Equal(ServiceLifetime.Transient, transientServiceDescriptor!.Lifetime);

    }
}