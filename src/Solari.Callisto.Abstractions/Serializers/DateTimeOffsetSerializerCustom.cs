using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Solari.Callisto.Abstractions.Serializers
{
    public class DateTimeOffsetSerializerCustom : DateTimeOffsetSerializer
    {
        public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType != BsonType.Null)
                return DateTimeHelper.BuildDateTimeOffset(base.Deserialize(context, args));
            context.Reader.ReadNull();

            return DateTimeHelper.DefaultDateTimeOffsetValue;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
        {
            base.Serialize(context, args, DateTimeHelper.BuildDateTimeOffset(value));
        }
    }
}