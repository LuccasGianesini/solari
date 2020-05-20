using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Solari.Vanth.Validation
{
    public interface IVanthValidationService
    {
        ValidationResult Validate<T>(T entity) where T : class;
        Task<ValidationResult> ValidateAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    }
}