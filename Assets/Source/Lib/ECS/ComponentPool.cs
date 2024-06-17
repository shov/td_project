using System;
using UnityEngine.Assertions;

namespace Lib.ECS
{
    public class ComponentPool<T> : IComponentPool where T : struct
    {
        private struct ComponentData
        {
            public T value;
            public bool isTaken;
        }

        private const int ALLOC_BATCH_SIZE = 256;

        private ComponentData[] _componentArray = new ComponentData[ALLOC_BATCH_SIZE];

        public void AllocateComponent(int id)
        {
            if (id > World.MAX_ENTITY_ID)
            {
                throw new Exception($"The entity limit has been reached {World.MAX_ENTITY_ID}");
            }
            
            while (true)
            {
                // If the row with id already exists, no need to allocate anything
                if (this._componentArray.Length > id)
                {
                    // Reset memory to its default
                    this._componentArray[id] = new ComponentData { isTaken = false, value = default };
                    return;
                }

                // Else -- double the size and try once more
                Array.Resize(ref this._componentArray, this._componentArray.Length * 2);
            }
        }

        public void FreeComponent(int id)
        {
            if (id >= this._componentArray.Length)
            {
                throw new Exception("Cannot free a component, out of range");
            }

            this._componentArray[id].isTaken = false;
            this._componentArray[id].value = default;
        }

        public ref T GetComponent(int id)
        {
            ref var component = ref this._componentArray[id];
            return ref component.value;
        }

        public void SetComponent(int id, ref T value)
        {
            ref var component = ref this._componentArray[id];
            component.isTaken = true;
            component.value = value;
        }

        public bool HasComponent(int id)
        {
            return this._componentArray[id].isTaken;
        }
    }
}