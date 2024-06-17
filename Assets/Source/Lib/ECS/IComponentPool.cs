namespace Lib.ECS
{
    public interface IComponentPool
    {
        void AllocateComponent(int id);
        void FreeComponent(int id);
    }
}