namespace Small.Net.Data
{
    public class SqlServerProvider : IDbProvider
    {
        public bool CanInsertWithOutput { get; } = true;
        public bool CanInsertWithMultiOutput { get; } = false;
    }
}