using System.Collections.Generic;

namespace Solari.Vanth
{
    public interface IError
    {
        string Code { get; set; }
        List<IErrorDetail> Details { get; set; }
        string ErrorType { get; set; }
        string Message { get; set; }
        string Target { get; set; }
    }
}
