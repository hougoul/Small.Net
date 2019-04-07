namespace Small.Net.Data
{
    public class OracleProvider : IDbProvider
    {
        public bool CanInsertWithOutput { get; } = true;

        public bool CanInsertWithMultiOutput { get; } = true;
    }
}