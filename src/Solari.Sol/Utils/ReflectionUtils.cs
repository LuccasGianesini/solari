using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Solari.Sol.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        ///     Gets the assemblies all the assemblies from the app domain.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}" />. T being <see cref="Assembly" /></returns>
        public static IEnumerable<Assembly> GetAssembliesFromAppDomain()
        {
            //TODO Find a more efficient way
            return Assembly.GetEntryAssembly()
                           ?.GetReferencedAssemblies().AsEnumerable()
                           .Select(assemblyName => AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName)).ToList();
        }

        /// <summary>
        ///     Get the implementation of an open generic type.
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="Type" /></returns>
        public static IEnumerable<Type> GetOpenGenericServiceImplementations<TService>() { return GetOpenGenericServiceImplementations(typeof(TService)); }

        /// <summary>
        ///     Get the implementation of an open generic type.
        /// </summary>
        /// <param name="openGenericType">Type of the service</param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="Type" /></returns>
        public static IEnumerable<Type> GetOpenGenericServiceImplementations(Type openGenericType)
        {
            var list = new List<Type>();

            foreach (Assembly assembly in GetAssembliesFromAppDomain())
                list.AddRange(from type in assembly.GetTypes()
                              from @interface in type.GetInterfaces()
                              let @base = type.BaseType
                              where
                                  @base?.IsGenericType == true
                               && openGenericType.IsAssignableFrom(@base.GetGenericTypeDefinition())
                               || @interface.IsGenericType
                               && openGenericType.IsAssignableFrom(@interface.GetGenericTypeDefinition())
                              select type);

            return list;
        }

        /// <summary>
        ///     Get the implementations of a service.
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="Type" /></returns>
        public static IEnumerable<Type> GetServiceImplementations<TService>() { return GetServiceImplementations(typeof(TService)); }


        /// <summary>
        ///     Get the implementations of a service.
        /// </summary>
        /// <param name="type">Type of the service</param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="Type" /></returns>
        public static IEnumerable<Type> GetServiceImplementations(Type type)
        {
            return GetAssembliesFromAppDomain().SelectMany(x => x.GetTypes())
                                               .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                               .Select(x => x);
        }
    }
}