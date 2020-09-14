using System;
using System.Collections.Generic;

namespace Solari.Vanth
{
    [Serializable]
    public class Error : IError
    {
        public string Code { get; set; }
        public List<IErrorDetail> Details { get; set;} = new List<IErrorDetail>(2);
        public string ErrorType { get; set;}
        public string Message { get; set;}
        public string Target { get; set;}


    }
}
