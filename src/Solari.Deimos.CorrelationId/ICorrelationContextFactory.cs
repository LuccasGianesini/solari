﻿namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContextFactory
    {
        ICorrelationContext Create(IEnvoyCorrelationContext envoyCorrelationContext);
        ICorrelationContext Create(string messageId, IEnvoyCorrelationContext envoyCorrelationContext);
        ICorrelationContext Create_Root_From_System_Diagnostics_Activity_And_Tracers(string messageId = "");
    }
}