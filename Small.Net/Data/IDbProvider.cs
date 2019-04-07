namespace Small.Net.Data
{
    public interface IDbProvider
    {
        /// <summary>
        /// To Know if the system support insert with output
        /// </summary>
        bool CanInsertWithOutput { get; }

        /// <summary>
        /// To know if we can have multi output
        /// </summary>
        bool CanInsertWithMultiOutput { get; }
    }
}