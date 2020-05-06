using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Solari.Ganymede.Framework;

namespace Solari.Ganymede.Pipeline
{
    /// <summary>
    ///     Stage one is the uri stage.
    ///     In this stage you should build the uri of the request
    /// </summary>
    public sealed class UriStage : IPipelineStage
    {
        private readonly StringBuilder _stringBuilder;
        private bool _inited;

        public UriStage(PipelineDescriptor pipelineDescriptor)
        {
            PipelineDescriptor = pipelineDescriptor;
            _stringBuilder = new StringBuilder(pipelineDescriptor.TargetUri);
        }

        public PipelineDescriptor PipelineDescriptor { get; }

        public static implicit operator PipelineDescriptor(UriStage uriStage)
        {
            uriStage.PipelineDescriptor.RequestMessage.RequestUri = new Uri(uriStage._stringBuilder.ToString(), UriKind.RelativeOrAbsolute);

            return uriStage.PipelineDescriptor;
        }

        /// <summary>
        ///     Append a dictionary of parameters to the current <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public UriStage AppendParameter(IDictionary<string, string[]> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach ((string key, string[] value) in parameters) AppendParameter(key, value);

            return this;
        }

        /// <summary>
        ///     Append a parameter and its array of values to the current <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="parameterName">Parameter</param>
        /// <param name="parameterValues">ParameterValues</param>
        /// <returns></returns>
        public UriStage AppendParameter(string parameterName, string[] parameterValues)
        {
            foreach (string t in parameterValues)
                AppendParameter(parameterName, t);

            return this;
        }

        /// <summary>
        ///     Append a parameter to the current <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="parameterName">Parameter</param>
        /// <param name="parameterValue">Parameter value</param>
        public UriStage AppendParameter(string parameterName, string parameterValue)
        {
            if (string.IsNullOrEmpty(parameterName)) throw new ArgumentNullException(nameof(parameterName));
            if (string.IsNullOrEmpty(parameterValue)) throw new ArgumentNullException(nameof(parameterValue));

            if (!_inited) Init();

            _stringBuilder
                .Append("&")
                .Append(HttpUtility.UrlEncode(parameterName))
                .Append("=")
                .Append(HttpUtility.UrlEncode(parameterValue));

            return this;
        }


        public UriStage AppendQueryString(string queryString)
        {
            if (string.IsNullOrEmpty(queryString)) throw new ArgumentNullException(nameof(queryString));

            if (QueryStringHelper.Eval(queryString)) _stringBuilder.Append(queryString);

            return this;
        }

        public UriStage ReplaceToken(IDictionary<string, string> keyValues)
        {
            if (keyValues == null) throw new ArgumentNullException(nameof(keyValues));

            foreach ((string key, string value) in keyValues) ReplaceToken(key, value);

            return this;
        }

        public UriStage ReplaceToken(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value cannot be null or empty.", nameof(key));
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            _stringBuilder.Replace(_validateTokenKey(key), value);

            return this;
        }

        private static string _validateTokenKey(string tokenKey)
        {
            if (tokenKey.StartsWith("[") && tokenKey.EndsWith("]")) return tokenKey;

            var sb = new StringBuilder(tokenKey);
            if (!tokenKey.StartsWith("[")) sb.Insert(0, '[');
            if (!tokenKey.EndsWith("]")) sb.Insert(sb.Length, ']');

            return sb.ToString();
        }

        private void Init()
        {
            _stringBuilder.Append(_stringBuilder.ToString().Contains("?") ? "&" : "?");
            _inited = true;
        }
    }
}