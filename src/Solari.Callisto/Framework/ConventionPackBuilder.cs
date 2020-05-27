using System;
using System.Linq;
using MongoDB.Bson.Serialization.Conventions;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Framework
{
    public class ConventionPackBuilder : IConventionPackBuilder
    {
        private readonly CallistoConventionRegistry _registry;
        private ConventionPack _conventionPack;
        private Func<Type, bool> _filter;
        private string _name;
        public ConventionPackBuilder() { _registry = CallistoConventionRegistry.Instance; }

        /// <summary>
        ///     Set the name of the convention pack.
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public IConventionPackBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        ///     Set the filter used to apply the convention pack.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns></returns>
        public IConventionPackBuilder WithFilter(Func<Type, bool> filter)
        {
            _filter = filter;
            return this;
        }

        /// <summary>
        ///     Use the default conventions.
        /// </summary>
        /// <returns></returns>
        public IConventionPackBuilder WithDefaultConventions()
        {
            CallistoLogger.ConventionPackLogger.UsingDefaultConventions();
            _registry.AddDefaultConventions();
            return this;
        }

        /// <summary>
        ///     Add a convention into the convention pack. <see cref="IConvention" />
        /// </summary>
        /// <param name="convention">The convention</param>
        /// <returns></returns>
        public IConventionPackBuilder WithConvention(IConvention convention)
        {
            if (convention == null) return this;
            _registry.AddConvention(convention);
            return this;
        }

        /// <summary>
        ///     Build the convention pack. If no conventions were provided, the library will use its default conventions.
        /// </summary>
        /// <returns></returns>
        public IConventionPackBuilder BuildConventionPack()
        {
            if (!_registry.RegisteredConventions.Any()) WithDefaultConventions();
            _conventionPack = new ConventionPack();

            foreach (IConvention convention in _registry.RegisteredConventions)
            {
                CallistoLogger.ConventionPackLogger.RegisteringConvention(convention.Name);
                _conventionPack.Add(convention);
            }

            return this;
        }

        /// <summary>
        ///     Register the convention pack into the <see cref="MongoDB.Bson.Serialization.Conventions.ConventionRegistry" />
        /// </summary>
        /// <returns>
        ///     <see cref="ConventionPack" />
        /// </returns>
        public ConventionPack RegisterConventionPack()
        {
            if (_conventionPack == null) BuildConventionPack();
            if (string.IsNullOrEmpty(_name)) _name = "Solari.Callisto.Conventions";
            _filter ??= CallistoTypeHelper.IsCallistoClass;
            ConventionRegistry.Register(_name, _conventionPack, _filter);
            CallistoLogger.ConventionPackLogger.RegisterConventionPack(_name);
            return _conventionPack;
        }
    }
}