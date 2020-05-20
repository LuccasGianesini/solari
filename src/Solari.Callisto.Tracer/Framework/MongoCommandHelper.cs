namespace Solari.Callisto.Tracer.Framework
{
    internal class MongoCommandHelper
    {
        internal static string NormalizeCommandName(string name)
        {
            return name switch
                   {
                       "insert"      => "Insert",
                       "aggregate"   => "Aggregate",
                       "find"        => "Find",
                       "delete"      => "Aggregate",
                       "update"      => "Update",
                       "insertmany"  => "InsertMany",
                       "insertOne"   => "InsertOne",
                       "findmany"    => "FindMany",
                       "findone"     => "FindOne",
                       "deletemany"  => "DeleteMany",
                       "deleteone"   => "DeleteOne",
                       "replacemany" => "ReplaceMany",
                       "replaceone"  => "ReplaceOne",
                       "updateone"   => "UpdateOne",
                       "updatemany"  => "UpdateMany",
                       _             => ""
                   };
        }
    }
}