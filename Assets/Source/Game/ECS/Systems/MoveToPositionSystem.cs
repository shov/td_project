using Game.ECS.Commands;
using Game.ECS.Components;
using Lib.ECS;
using UnityEngine;

namespace Game.ECS.Systems
{
    public sealed class MoveToPositionSystem : IFixedUpdateSystem
    {
        private const float MIN_SQR_DISTANCE = 0.01f;
        
        private ComponentPool<MoveToPositionCommand> _commandPool;
        private ComponentPool<MoveStateComponent> _statePool;
        private ComponentPool<TransformComponent> _transformPool;
        
        void IFixedUpdateSystem.OnFixedUpdate(int id)
        {
            if (!this._commandPool.HasComponent(id))
            {
                return;
            }

            ref MoveToPositionCommand command = ref this._commandPool.GetComponent(id);
            ref MoveStateComponent state = ref this._statePool.GetComponent(id);
            ref TransformComponent transformComponent = ref this._transformPool.GetComponent(id);

            Vector3 direction = command.destination - transformComponent.value.position;
            if (Vector3.SqrMagnitude(direction) <= MIN_SQR_DISTANCE)
            {
                state.isMoveRequired = false;
                return;
            }

            state.direction = direction.normalized;
            state.isMoveRequired = true;
        }
        
    }
}