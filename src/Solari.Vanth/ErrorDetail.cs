﻿using System;
using System.Text;
using Newtonsoft.Json;
using Solari.Sol.Utils;

namespace Solari.Vanth
{
    [Serializable]
    public class ErrorDetail : IErrorDetail
    {
        public string Code { get; set; }
        public string StackTrace { get; set; }
        public string HelpUrl { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
        public string Source { get; set; }

    }
}
