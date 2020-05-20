using System;
using System.Collections;
using System.Collections.Generic;
using Solari.Deimos.CorrelationId;
using Xunit;

namespace Solari.Dione.Tests
{
    public class TestUpdate : IUpdateDescriptorCollection
    {
        public ICollection<UpdateDescriptor> Descriptors { get; set; } 
        
    }

    public class BaseClass
    {
        private DateTimeOffset CreatedAt { get; set; }
    }
    public class TestClass : BaseClass
    {
        public string Name { get; set; }
        public InnerTestClass InnetClass { get; set; }
        public ICollection<InnerInnerTestClass>  InnerInnerTestClasses { get; set; }
        public IDictionary<string, InnerTestClass> Dictionary { get; set; }
    }

    public class InnerTestClass : BaseClass
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        
    }

    public class InnerInnerTestClass
    {
        private List<TestList> List { get; set; }
        public decimal Id { get; set; }
        public string MMX { get; set; }
        
    }

    public class TestList
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class UpdateMatcherTests
    {
        [Fact]
        public void CheckFirstLevelProperty()
        {
            var update = new TestUpdate()
            {
                Descriptors = new List<UpdateDescriptor>
                {
                    new UpdateDescriptor
                    {
                        Path = "Name",
                        NewValue = "Luccas"
                    }
                }
            };
            var matecher = new UpdateMatcher();
            var val = matecher.Match<TestClass>(update);

        }
    }
}