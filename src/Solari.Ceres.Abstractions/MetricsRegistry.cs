using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;

namespace Solari.Ceres.Abstractions
{
    public static class MetricsRegistry
    {
        public static class ErrorMetrics
        {
            private static readonly string Context = "Exceptions";

            public static CounterOptions CatchExceptionSinceLastReport = new CounterOptions
            {
                Context = Context,
                Name = "Number of exceptions raised and catch since last reported",
                MeasurementUnit = Unit.Errors,
                ReportItemPercentages = true,
                ReportSetItems = true
            };

            public static CounterOptions CatchExceptionTotal = new CounterOptions
            {
                Context = Context,
                Name = "Number of exceptions raised and catch",
                MeasurementUnit = Unit.Errors,
                ReportItemPercentages = true
            };
        }

        public static class ProcessMetrics
        {
            private static readonly string Context = "Process";

            public static GaugeOptions CpuUsageTotal = new GaugeOptions
            {
                Context = Context,
                Name = "Process Total CPU Usage",
                MeasurementUnit = Unit.Percent
            };

            public static GaugeOptions ProcessPagedMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "PagedProcess Memory Size",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessPeekPagedMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Peek Paged Memory Size",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessPeekVirtualMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Peek Paged Memory Size",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessPeekWorkingSetSizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Working Set",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessPrivateMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Private Memory Size",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessVirtualMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Virtual Memory Size",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions SystemNonPagedMemoryGauge = new GaugeOptions
            {
                Context = Context,
                Name = "System Non-Paged Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions SystemPagedMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "PagedSystem Memory Size",
                MeasurementUnit = Unit.Bytes
            };
        }
    }
}