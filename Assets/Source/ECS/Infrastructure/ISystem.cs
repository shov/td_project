using Unity.VisualScripting;

namespace ECS.Infrastructure
{
    public interface ISystem
    {
    }

    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(int id);
    }

    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(int id);
    }

    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate(int id);
    }
}