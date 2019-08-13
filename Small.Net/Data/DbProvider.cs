namespace Small.Net.Data
{
    public abstract class DbProvider : IDbProvider
    {
        public bool CanInsertWithOutput { get; protected set; } = false;
        public bool CanInsertWithMultiOutput { get; protected set; } = false;

        protected string Prefix { get; set; } = "@P_";

        public virtual string ComputeParameterName(string baseOnName)
        {
            return Prefix + baseOnName.ToUpperInvariant();
        }
    }
}