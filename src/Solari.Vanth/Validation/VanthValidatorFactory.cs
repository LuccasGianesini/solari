using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Solari.Vanth.Validation
{
    public class VanthValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _provider;
        public VanthValidatorFactory(IServiceProvider provider) { _provider = provider; }


        public IValidator<T> GetValidator<T>() => (IValidator<T>) GetValidator(typeof(T));

        public IValidator GetValidator(Type type)
        {
            try
            {
                return _provider.GetRequiredService(typeof(IValidator<>).MakeGenericType(type)) as IValidator;
            }
            catch (Exception)
            {
                Type baseType = type.BaseType;
                if (baseType == null)
                    throw;
                return ActivatorUtilities.CreateInstance(_provider, typeof(IValidator<>).MakeGenericType(type)) as IValidator;
            }
        }
    }
}