using System;
using UnityEngine;

namespace Game.ECS.Components
{
    [Serializable]
    public struct MoveStateComponent
    {
        public bool isMoveRequired; // default false
        public Vector3 direction;
    }
}