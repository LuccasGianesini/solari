using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Solari.Callisto.Serializers
{
    public class DateTimeSerializerCustom : DateTimeSerializer
    {

        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType != BsonType.Null) return Helper.BuildDateTimeValue(base.Deserialize(context, args));

            context.Reader.ReadNull();

            return Helper.DefaultDateTimeValue;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            DateTime dateTime = Helper.BuildDateTimeValue(value);
            base.Serialize(context, args, dateTime);
        }
    }
}
