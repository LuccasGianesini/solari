namespace Solari.Eris
{
    public interface IQuery : Convey.CQRS.Queries.IQuery
    {
    }

    public interface IQuery<T> : Convey.CQRS.Queries.IQuery<T>, IQuery
    {
    }
}