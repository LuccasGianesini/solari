namespace Solari.Vanth
{
    public interface IErrorDetail
    {
        string Code { get; set; }
        string StackTrace { get; set; }
        string HelpUrl { get; set; }
        string Message { get; set; }
        string Target { get; set; }
        string Source { get; set; }
    }
}
