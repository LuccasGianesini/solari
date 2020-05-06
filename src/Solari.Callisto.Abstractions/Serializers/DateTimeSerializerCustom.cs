using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Solari.Callisto.Abstractions.Serializers
{
    public class DateTimeSerializerCustom : DateTimeSerializer
    {
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType != BsonType.Null) return DateTimeHelper.BuildDateTimeValue(base.Deserialize(context, args));

            context.Reader.ReadNull();

            return DateTimeHelper.DefaultDateTimeValue;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            DateTime dateTime = DateTimeHelper.BuildDateTimeValue(value);
            base.Serialize(context, args, dateTime);
        }
    }
}