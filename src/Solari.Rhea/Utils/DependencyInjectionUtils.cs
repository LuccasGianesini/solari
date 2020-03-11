using System;
using Microsoft.Extensions.DependencyInjection;

namespace Solari.Rhea.Utils
{
  public static class DependencyInjectionUtils
  {

    /// <summary>
    /// Create a <see cref="ServiceDescriptor"/>
    /// </summary>
    /// <param name="serviceLifetime">The life time of the service.<see cref="ServiceLifetime"/></param>
    /// <typeparam name="TService">Type of the service i.e: Interface</typeparam>
    /// <typeparam name="TConcrete">Type of the service concrete implementation.</typeparam>
    /// <returns><see cref="ServiceDescriptor"/></returns>
    public static ServiceDescriptor BuildServiceDescriptor<TService, TConcrete>(ServiceLifetime serviceLifetime)
    {
      return BuildServiceDescriptorFromServiceAndConcreteTypes(typeof(TService), typeof(TConcrete), serviceLifetime);
    }

    /// <summary>
    /// Create a <see cref="ServiceDescriptor"/> 
    /// </summary>
    /// <param name="serviceType">Type of the service i.e: Interface</param>
    /// <param name="concreteType">Type of the service concrete implementation.</param>
    /// <param name="serviceLifetime">The life time of the service.<see cref="ServiceLifetime"/></param>
    /// <returns><see cref="ServiceDescriptor"/></returns>
    public static ServiceDescriptor BuildServiceDescriptorFromServiceAndConcreteTypes(Type serviceType, Type concreteType, ServiceLifetime serviceLifetime)
    {
      return new ServiceDescriptor(serviceType, concreteType, serviceLifetime);
    }

    /// <summary>
    /// Build a <see cref="ServiceDescriptor"/> using open generic types. 
    /// </summary>
    /// <param name="serviceGenericType">The open generic service type. i.e: IInterface{T}</param>
    /// <param name="concreteGenericType">The concrete implementation of the generic type. i.e: Class{T} : IInterface{T}</param>
    /// <param name="targetGenericType">Target generic type. i.e: Class{int}</param>
    /// <param name="serviceLifetime">The life time if the service <see cref="ServiceLifetime"/></param>
    /// <returns><see cref="ServiceDescriptor"/></returns>
    public static ServiceDescriptor BuildServiceDescriptorWithGeneric(Type serviceGenericType, Type concreteGenericType, Type targetGenericType, ServiceLifetime serviceLifetime)
    {
      return new ServiceDescriptor(serviceGenericType.MakeGenericType(targetGenericType), concreteGenericType.MakeGenericType(targetGenericType), serviceLifetime);
    }
  }
}