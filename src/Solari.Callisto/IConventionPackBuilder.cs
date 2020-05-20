using System;
using MongoDB.Bson.Serialization.Conventions;

namespace Solari.Callisto
{
    public interface IConventionPackBuilder
    {
        /// <summary>
        ///     Set the name of the convention pack.
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        IConventionPackBuilder WithName(string name);

        /// <summary>
        ///     Set the filter used to apply the convention pack.
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns></returns>
        IConventionPackBuilder WithFilter(Func<Type, bool> filter);

        /// <summary>
        ///     Use the default conventions.
        /// </summary>
        /// <returns></returns>
        IConventionPackBuilder WithDefaultConventions();

        /// <summary>
        ///     Add a convention into the convention pack. <see cref="IConvention" />
        /// </summary>
        /// <param name="convention">The convention</param>
        /// <returns></returns>
        IConventionPackBuilder WithConvention(IConvention convention);

        /// <summary>
        ///     Build the convention pack. If no conventions were provided, the library will use its default conventions.
        /// </summary>
        /// <returns></returns>
        IConventionPackBuilder BuildConventionPack();

        /// <summary>
        ///     Register the convention pack into the <see cref="MongoDB.Bson.Serialization.Conventions.ConventionRegistry" />
        /// </summary>
        /// <returns>
        ///     <see cref="ConventionPack" />
        /// </returns>
        ConventionPack RegisterConventionPack();
    }
}