namespace Small.Net.Data
{
    public class OracleProvider : DbProvider
    {
        public OracleProvider()
        {
            Prefix = ":P_";
            CanInsertWithOutput = true;
            CanInsertWithMultiOutput = true;
        }
    }
}