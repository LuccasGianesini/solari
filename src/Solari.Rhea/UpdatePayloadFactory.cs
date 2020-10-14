namespace Solari.Rhea
{
    public delegate TPayload UpdatePayloadFactory<TPayload>(TPayload existing)
        where TPayload : class;
}
