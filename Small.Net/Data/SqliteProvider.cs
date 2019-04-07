namespace Small.Net.Data
{
    public class SqliteProvider : IDbProvider
    {
        public bool CanInsertWithOutput { get; } = false;

        public bool CanInsertWithMultiOutput { get; } = false;
    }
}