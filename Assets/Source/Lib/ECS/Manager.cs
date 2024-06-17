using System;
using UnityEngine;

namespace Lib.ECS
{
    public class Manager : MonoBehaviour
    {
        private readonly World _world = new(); // Instantiate the only world here
        private void Awake()
        {
            // TODO register components
            // TODO register systems 
            
            // TODO install world
            
            // TODO Init every entity (where I get them from at this point?
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