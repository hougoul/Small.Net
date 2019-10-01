using Small.Net.Data;

namespace Small.Net.Oracle.Data
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