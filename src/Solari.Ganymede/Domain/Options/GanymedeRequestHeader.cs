using System;
using Solari.Ganymede.Builders;

namespace Solari.Ganymede.Domain.Options
{
	/// <summary>
	///     <see cref="HttpRequestMessageHeaderBuilder" />
	/// </summary>
	public class GanymedeRequestHeader
    {
	    /// <summary>
	    ///     For key value headers like Authorization, set this property as the key.
	    ///     For value quality header like Accept, set this as the quality.
	    /// </summary>
	    public string KeyOrQuality { get; set; }

	    /// <summary>
	    ///     Header name i.e.: Accept
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    ///     Value.
	    ///     For single parameter method like IfModifiedSince, set this as the value for the parameter.
	    /// </summary>
	    public string Value { get; set; }

	
    }
}