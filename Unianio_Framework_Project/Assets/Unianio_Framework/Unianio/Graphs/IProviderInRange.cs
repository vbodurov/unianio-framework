namespace Unianio.Graphs
{
    public interface IProviderInRange
    {
         /// <summary>
        /// value must be between 0 and 1
        /// </summary>
        float From { get; }
        /// <summary>
        /// value must be between 0 and 1
        /// </summary>
        float To { get; }
        bool IsIn(double value);
    }
}