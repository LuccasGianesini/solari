namespace Solari.Rhea
{
    public delegate TPayload PayloadFactory<out TPayload>()
        where TPayload : class;
}
