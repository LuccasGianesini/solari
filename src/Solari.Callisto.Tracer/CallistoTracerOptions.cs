using System.Collections.Generic;

namespace Solari.Callisto.Tracer
{
    public class CallistoTracerOptions
    {
        public List<string> TracedDbOperations { get; set; } = new List<string>
        {
            "insert",
            "find",
            "update",
            "delete",
            "aggregate",
            "insertone",
            "insertmany",
            "deleteone",
            "deletemany",
            "replaceone",
            "replacemany",
            "updateone",
            "updatemany",
            "findone",
            "findmany"
        };

        public bool Enabled { get; set; } = true;
        public int MaxPacketSize { get; set; } = 65000;
    }
}