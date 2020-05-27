using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions
{
    public static class CallistoOperationHelper
    {
        public static void PreExecutionCheck<T>(ICallistoOperation<T> operation)
        where T: class, IDocumentRoot
        {
            if (operation is null)
                throw new CallistoException("Cannot call a null instance operation");
            operation.Validate();
        }
    }
}