using System;
using UnityEngine;

namespace Game.ECS.Commands
{
    [Serializable]
    public struct MoveToPositionCommand
    {
        public Vector3 destination;
    }
}