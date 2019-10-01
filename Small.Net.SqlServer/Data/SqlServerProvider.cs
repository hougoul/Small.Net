using Small.Net.Data;

namespace Small.Net.SqlServer.Data
{
    public class SqlServerProvider : DbProvider
    {
        public SqlServerProvider()
        {
            CanInsertWithOutput = true;
            CanInsertWithMultiOutput = true;
        }
    }
}