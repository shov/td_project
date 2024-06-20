using System;
using Game.ECS.Commands;
using Game.ECS.Components;
using Game.ECS.Systems;
using UnityEngine;

namespace Lib.ECS
{
    public class Manager : MonoBehaviour
    {
        private readonly World _world = new(); // Instantiate the only world here
        private void Awake()
        {
            // Registration of components
            this._world.RegisterComponent<MoveStateComponent>();
            this._world.RegisterComponent<MoveSpeedComponent>();
            this._world.RegisterComponent<TransformComponent>();
            this._world.RegisterComponent<AnimatorComponent>();
            // Commands (are also components)
            this._world.RegisterComponent<MoveToPositionCommand>();
            
            // Register Systems
            this._world.RegisterSystem<MovementSystem>();
            this._world.RegisterSystem<MoveAnimationSystem>();
            this._world.RegisterSystem<MoveToPositionSystem>();
            
            // Install World
            this._world.Install();
            
            // Init entities
            Entity[] entities = FetchAllEntities();
            foreach (Entity entity in entities)
            {
                entity.Init(this._world);
            }
        }

        private Entity[] FetchAllEntities()
        {
            // Find all entities in the scene TODO improve this
            return FindObjectsOfType<Entity>();
        }
        
        #region Unity Loop

        private void Update()
        {
            this._world.OnUpdate();
        }

        private void FixedUpdate()
        {
            this._world.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            this._world.OnLateUpdate();
        }

        #endregion
    }
}