using System.Reflection;

namespace Solari.Triton.Compat
{
    static class TypeExtraInfo
    {
        public static bool MembersAreTheSame(MemberInfo a, MemberInfo b)
        {
            return a.MetadataToken == b.MetadataToken
                && a.DeclaringType == b.DeclaringType;
        }
    }
}
