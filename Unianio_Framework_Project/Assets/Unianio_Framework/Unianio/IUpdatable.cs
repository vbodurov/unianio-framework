namespace Unianio
{
    public interface IUpdatable
    {
        void Update();
    }
    public sealed class VoidUpdatable : IUpdatable
    {
        internal static readonly VoidUpdatable Instance = new VoidUpdatable();
        void IUpdatable.Update()
        {

        }
    }
}
