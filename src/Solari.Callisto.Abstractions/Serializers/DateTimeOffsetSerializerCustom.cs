﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Solari.Callisto.Abstractions.Serializers
{
    public class DateTimeOffsetSerializerCustom : IBsonSerializer
    {
        private readonly DateTimeOffsetSerializer _serializer;
        public DateTimeOffsetSerializerCustom(Type valueType, DateTimeOffsetSerializer serializer)
        {
            ValueType = valueType;
            _serializer = serializer;
        }
        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType != BsonType.Null) return DateTimeHelper.BuildDateTimeOffset(_serializer.Deserialize(context, args));

            context.Reader.ReadNull();

            return DateTimeHelper.DefaultDateTimeOffsetValue;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (!(value is DateTimeOffset)) return;

            DateTimeOffset offset = DateTimeHelper.BuildDateTimeOffset((DateTimeOffset) value);
            _serializer.Serialize(context, args, offset);
        }

        public Type ValueType { get; }
    }
}
