using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Solari.Deimos.CorrelationId
{
    public class UpdateMatcher
    {
        public T Match<T>(IUpdateDescriptorCollection descriptorCollection) where T : class
        {
            
        


        private static void Validate(IUpdateDescriptorCollection descriptorCollection, Type typeToMatch)
        {
            if (descriptorCollection == null)
            {
                throw new DioneException($"{nameof(IUpdateDescriptorCollection)} cannot be null.");
            }

            if (!descriptorCollection.Descriptors.Any())
            {
                throw new DioneException($"{nameof(IUpdateDescriptorCollection.Descriptors)} is empty. Cannot match any properties");
            }

            if (typeToMatch == null)
            {
                throw new DioneException($"Type to match is null. Please provide a type.");
            }
        }

        private IEnumerable<PropertyInfo> GetTypeProperties(Type typeToMatch) { return typeToMatch.GetProperties(); }
    }
}