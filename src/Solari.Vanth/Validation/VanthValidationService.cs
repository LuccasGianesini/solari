﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Solari.Vanth.Validation
{
    public class VanthValidationService : IVanthValidationService
    {
        private readonly IValidatorFactory _validatorFactory;

        public VanthValidationService(IValidatorFactory validatorFactory) { _validatorFactory = validatorFactory; }

        public ValidationResult Validate<T>(T entity) where T : class 
            => GetValidationForEntity(entity).Validate(entity);

        public async Task<ValidationResult> ValidateAsync<T>(T entity, CancellationToken cancellationToken) where T : class 
            => await GetValidationForEntity(entity).ValidateAsync(entity, cancellationToken);

        private IValidator GetValidationForEntity<T>(T entity) where T : class 
            => _validatorFactory.GetValidator(entity.GetType());
    }
}