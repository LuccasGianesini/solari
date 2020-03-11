using MongoDB.Driver.Core.Clusters;

namespace Solari.Callisto.Connector
{
    public class CallistoConnectionCheck
    {
        public bool IsConnected { get; }
        public string PingResult { get; }
        public ClusterState ClusterState { get; }

        public CallistoConnectionCheck(ClusterState clusterState, string pingResult)
        {
            ClusterState = clusterState;
            PingResult = pingResult;
            IsConnected = clusterState == ClusterState.Connected;
        }
    }
}