using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    [Serializable]
    public class Error
    {

        public string Code { get; set; }
        public List<ErrorDetail> Details { get; set;} = new List<ErrorDetail>(2);
        public string ErrorType { get; set;}
        public string Message { get; set;}
        public string Target { get; set;}


    }
}
