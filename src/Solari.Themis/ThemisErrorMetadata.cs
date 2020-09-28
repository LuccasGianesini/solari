using App.Metrics.Logging;

namespace Solari.Themis
{
    public class ThemisErrorMetadata
    {
        public ThemisErrorMetadata(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
        }

        public ThemisErrorMetadata(string className, string methodName, object[] args)
        {
            ClassName = className;
            MethodName = methodName;
            Args = args;
        }

        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public object[] Args { get; set; }

        public static ThemisErrorMetadata New(string className, string methodName) => new ThemisErrorMetadata(className, methodName);

        public static ThemisErrorMetadata New(string className, string methodName, params object[] args) => new ThemisErrorMetadata(className, methodName, args);
    }
}
