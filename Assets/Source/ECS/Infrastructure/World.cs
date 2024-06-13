using System;
using System.Collections.Generic;

namespace ECS.Infrastructure
{
    public class World
    {
        // Entity ID is the index, is that taken is the value
        private readonly List<bool> _idTakenList = new();

        // Systems
        private readonly List<ISystem> _systemList = new();
        private readonly List<IUpdateSystem> _updateSystemList = new();
        private readonly List<IFixedUpdateSystem> _fixedUpdateSystemList = new();
        private readonly List<ILateUpdateSystem> _lateUpdateSystem = new();

        // Component pools
        private readonly Dictionary<Type, IComponentPool> _componentPoolDict = new();

        public int CreateEntity()
        {
            // TODO 
            return 0;
        }

        public void DestroyEntity(int id)
        {
            // TODO
        }
    }
}