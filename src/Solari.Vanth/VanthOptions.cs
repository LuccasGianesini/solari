namespace Solari.Vanth
{
    public class VanthOptions
    {
        public bool UseFluentValidation { get; set; }
        public bool UseExceptionHandlingMiddleware { get; set; }
        public bool ReturnFullExceptionInProduction { get; set; }
    }
}