using System.Collections.Generic;

namespace Solari.Ceres.Abstractions
{
    public class MetricsTrackingMiddlewareOptions
    {
        public bool UseActiveRequests { get; set; } = true;
        public bool UseErrorTracking { get; set; } = true;
        public bool PostAndPutSizeTracking { get; set; } = true;
        public bool RequestTracking { get; set; } = true;
        public bool OAuth2Tracking { get; set; }
        public bool ApdexTracking { get; set; }
        public int ApdexSeconds { get; set; } = 2;
        public List<int> IgnoredHttpStatusCodes { get; set; } = new List<int>
        {
            404
        };
        
    }
}