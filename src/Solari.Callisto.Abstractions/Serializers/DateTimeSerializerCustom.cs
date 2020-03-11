using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Solari.Callisto.Abstractions.Serializers
{
    public class DateTimeSerializerCustom : MongoDB.Bson.Serialization.Serializers.DateTimeSerializer
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