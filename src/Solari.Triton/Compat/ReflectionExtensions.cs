using System.Reflection;


namespace Solari.Triton.Compat
{
    internal static class ReflectionExtensions
    {
        public static MemberTypes GetMemberType(this MemberInfo mi)
        {
            return mi.MemberType;
        }
    }
}
