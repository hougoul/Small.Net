namespace Small.Net.Data
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