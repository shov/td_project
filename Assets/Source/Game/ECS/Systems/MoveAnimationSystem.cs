using Game.ECS.Components;
using Lib.ECS;
using UnityEngine;

namespace Game.ECS.Systems
{
    public sealed class MoveAnimationSystem : ILateUpdateSystem
    {
        private static readonly int ANIM_STATE = Animator.StringToHash("state");

        private ComponentPool<AnimatorComponent> _animatorPool;
        private ComponentPool<MoveStateComponent> _statePool;

        void ILateUpdateSystem.OnLateUpdate(int id)
        {
            if (!this._statePool.HasComponent(id) || !this._animatorPool.HasComponent(id))
            {
                return;
            }

            ref MoveStateComponent state = ref this._statePool.GetComponent(id);
            ref AnimatorComponent animatorComponent = ref this._animatorPool.GetComponent(id);

            animatorComponent.value.SetInteger(
                ANIM_STATE,
                state.isMoveRequired
                    ? 1
                    : 0
            );
        }
    }
}