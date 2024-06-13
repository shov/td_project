using System;
using Unity.VisualScripting;

namespace ECS.Infrastructure
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
        private int _size = 0;

        public void AllocateComponent()
        {
            if (this._size + 1 >= this._componentArray.Length)
            {
                Array.Resize(ref this._componentArray, this._componentArray.Length * 2);
            }

            this._componentArray[this._size] = new ComponentData
            {
                isTaken = false,
                value = default
            };

            this._size++;
        }

        public void FreeComponent(int id)
        {
            // the id must always be in the range...
            this._componentArray[id].isTaken = false;
            this._componentArray[id].value = default;
        }

        public T GetComponent(int id)
        {
            ref var component = ref this._componentArray[id];
            return component.value;
        }

        public void SetComponent(int id, T value)
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