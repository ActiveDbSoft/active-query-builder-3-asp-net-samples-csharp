using System.Data;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace CustomStorage.QueryBuilderProvider
{
    public class SqLiteQueryBuilder : QueryBuilder
    {
        public SqLiteQueryBuilder(IDbConnection connection, string instanceId) : base(instanceId)
        {
            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            BehaviorOptions.AllowSleepMode = false;

            // Assign an instance of the syntax provider which defines SQL syntax and metadata retrieval rules.
            SyntaxProvider = new SQLiteSyntaxProvider();

            // Bind Active Query Builder to a live database connection.
            MetadataProvider = new SQLiteMetadataProvider
            {
                // Assign an instance of DBConnection object to the Connection property.
                Connection = connection
            };
        }
    }
}