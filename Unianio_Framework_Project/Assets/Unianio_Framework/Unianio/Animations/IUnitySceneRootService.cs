namespace Unianio.Animations
{
    public interface IUnitySceneRootService : IInitializable, IUpdatable { }
    public class VoidUnitySceneRootService : IUnitySceneRootService
    {
        void IInitializable.Initialize() { }
        void IUpdatable.Update() { }
    }
}