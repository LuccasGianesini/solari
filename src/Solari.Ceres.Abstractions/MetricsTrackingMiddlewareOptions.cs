using System.Collections.Generic;

namespace Solari.Ceres.Abstractions
{
    public class MetricsTrackingMiddlewareOptions
    {
        public bool ActiveRequests { get; set; } = true;
        public bool ErrorTracking { get; set; } = true;
        public bool PostAndPutSizeTracking { get; set; } = true;
        public bool RequestTracking { get; set; } = true;
        public bool OAuth2Tracking { get; set; }
        public bool ApdexTracking { get; set; } = true;
        public int ApdexSeconds { get; set; } = 1;

        public List<int> IgnoredHttpStatusCodes { get; set; } = new List<int>
        {
            404
        };
    }
}