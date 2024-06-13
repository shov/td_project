namespace ECS.Infrastructure
{
    public interface IComponentPool
    {
        void AllocateComponent();
        void FreeComponent(int id);
    }
}