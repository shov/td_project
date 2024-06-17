using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lib.ECS
{
    public class World
    {
        // Public ECS constants
        public const uint MAX_ENTITY_ID = 4096 - 1; // Just a limit
        
        // Entity ID is the index, is that taken is the value
        private readonly List<bool> _idTakenList = new();

        // Systems
        private readonly List<ISystem> _systemList = new();
        private readonly List<IUpdateSystem> _updateSystemList = new();
        private readonly List<IFixedUpdateSystem> _fixedUpdateSystemList = new();
        private readonly List<ILateUpdateSystem> _lateUpdateSystem = new();

        // Component pools, keep "columns"
        private readonly Dictionary<Type, IComponentPool> _componentPoolDict = new();
        
        #region Systems & Components

        public void RegisterComponent<T>() where T : struct
        {
            this._componentPoolDict[typeof(T)] = new ComponentPool<T>();
        }

        public void RegisterSystem<T>() where T : ISystem, new()
        {
            var system = new T();
            this._systemList.Add(system);

            if (system is IUpdateSystem updateSystem)
            {
                this._updateSystemList.Add(updateSystem);
            }

            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                this._fixedUpdateSystemList.Add(fixedUpdateSystem);
            }

            if (system is ILateUpdateSystem lateUpdateSystem)
            {
                this._lateUpdateSystem.Add(lateUpdateSystem);
            }
        }

        public void Install()
        {
            foreach (var system in this._systemList)
            {
                Type systemType = system.GetType();
                var fieldArray = systemType.GetFields(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                foreach (var field in fieldArray)
                {
                    var genericArgArray = field.FieldType.GetGenericArguments();
                    if (genericArgArray.Length != 1)
                    {
                        continue;
                    }

                    Type componentType = genericArgArray[0];

                    if (!this._componentPoolDict.TryGetValue(componentType, out var refToPool))
                    {
                        throw new Exception(
                            $"Cannot install system {system.GetType().ToString()}, component pool {componentType.ToString()} has not been registered");
                    }

                    field.SetValue(system, refToPool);
                }
            }
        }
        
        #endregion
        
        #region Entity
        public int CreateEntity()
        {
            int id = 0;

            for (; id < this._idTakenList.Count; id++)
            {
                if (!this._idTakenList[id])
                {
                    this._idTakenList[id] = true;
                    // if it exists than it had been allocated once and is supposed to be properly freed 
                    // so skip the pool allocation
                    return id;
                }
            }

            id = this._idTakenList.Count;
            this._idTakenList.Add(true);
            
            // Allocate all component pools for the id (in other words create all columns for new row)
            foreach (var pool in this._componentPoolDict.Values)
            {
                pool.AllocateComponent(id);
            }

            return id;
        }

        public void DestroyEntity(int id)
        {
            if (id >= this._idTakenList.Count)
            {
                throw new Exception($"Cannot destroy an entity with id {id}, it's out of range");
            }

            this._idTakenList[id] = false;
            
            // Free all components
            foreach (var pool in this._componentPoolDict.Values)
            {
                pool.FreeComponent(id);
            }
        }
        #endregion
        
        #region Data access

        public void SetComponentData<T>(int id, ref T data) where T : struct
        {
            Type componentType = typeof(T);
            
            // Check if the type is supported
            if (!this._componentPoolDict.TryGetValue(componentType, out var pool))
            {
                throw new Exception($"Cannot set data for a component {componentType.ToString()}, no pool found");
            }

            ((ComponentPool<T>)pool).SetComponent(id, ref data);
        }

        public ref T GetComponentData<T>(int id) where T : struct
        {
            Type componentType = typeof(T);
            
            // Check if type supported
            if (!this._componentPoolDict.TryGetValue(componentType, out var pool))
            {
                throw new Exception($"Cannot get data for a component {componentType.ToString()}");
            }

            return ref ((ComponentPool<T>)pool).GetComponent(id);
        }
        
        #endregion
        
        #region for -> Unity Loop

        public void OnUpdate()
        {
            foreach(var system in this._updateSystemList)
            {
                for(var id = 0; id < this._idTakenList.Count; id++)
                {
                    if (!this._idTakenList[id])
                    {
                        continue;
                    }

                    system.OnUpdate(id);
                }
            }
        }

        public void OnFixedUpdate()
        {
            foreach(var system in this._fixedUpdateSystemList)
            {
                for(var id = 0; id < this._idTakenList.Count; id++)
                {
                    if (!this._idTakenList[id])
                    {
                        continue;
                    }

                    system.OnFixedUpdate(id);
                }
            }
        }

        public void OnLateUpdate()
        {
            foreach(var system in this._lateUpdateSystem)
            {
                for(var id = 0; id < this._idTakenList.Count; id++)
                {
                    if (!this._idTakenList[id])
                    {
                        continue;
                    }

                    system.OnLateUpdate(id);
                }
            }
        }

        #endregion
    }
}