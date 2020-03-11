using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;

namespace Solari.Samples.Domain
{
    public class Person : IDocumentRoot
    {
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ObjectId Id { get; set; }

        public List<PersonAttribute> Attributes { get; set; }
    }
}