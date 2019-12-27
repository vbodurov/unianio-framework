namespace Unianio.Moves
{
    public interface IRangeOfNumbers
    {
        /// <summary>
        /// value must be between 0 and 1
        /// </summary>
        float From { get; }
        /// <summary>
        /// value must be between 0 and 1
        /// </summary>
        float To { get; }
    }
}