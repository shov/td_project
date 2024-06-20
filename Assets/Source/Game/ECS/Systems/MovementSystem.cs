using Game.ECS.Components;
using Lib.ECS;
using UnityEngine;

namespace Game.ECS.Systems
{
    public sealed class MovementSystem : IFixedUpdateSystem
    {
        private ComponentPool<MoveStateComponent> _statePool;
        private ComponentPool<MoveSpeedComponent> _speedPool;
        private ComponentPool<TransformComponent> _transformPool;
        
        void IFixedUpdateSystem.OnFixedUpdate(int id)
        {
            if (!this._statePool.HasComponent(id))
            {
                return;
            }

            ref MoveStateComponent state = ref this._statePool.GetComponent(id);
            if (!state.isMoveRequired)
            {
                return;
            }
            
            ref TransformComponent transformComponent = ref this._transformPool.GetComponent(id);
            ref MoveSpeedComponent speedComponent = ref this._speedPool.GetComponent(id);

            Vector3 direction = state.direction;
            var offset = direction * (speedComponent.speed * Time.fixedDeltaTime);
            transformComponent.value.position += offset;
            transformComponent.value.rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            state.isMoveRequired = false;
        }
    }
}