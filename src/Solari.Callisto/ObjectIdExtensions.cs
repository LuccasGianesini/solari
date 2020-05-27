using System;
using System.Linq;
using MongoDB.Bson;

namespace Solari.Callisto
{
    public static class ObjectIdExtensions
    {
        public static Guid AsGuid(this ObjectId oid)
        {
            var bytes = oid.ToByteArray().Concat(new byte[] { 5, 5, 5, 5 }).ToArray();
            Guid gid = new Guid(bytes);
            return gid;
        }
        /// </summary>
        public static ObjectId AsObjectId(this Guid gid)
        {
            var bytes = gid.ToByteArray().Take(12).ToArray();
            var oid = new ObjectId(bytes);
            return oid;
        }

    }
}