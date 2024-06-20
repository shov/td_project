using System;
using Game.ECS.Components;
using Lib.ECS;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.ECS.Entities
{
    public class UnitEntity : Entity
    {
        [SerializeField] private float speed;
        

        protected override void OnInit()
        {
           this.SetData(new MoveStateComponent());
           this.SetData(new MoveSpeedComponent
           {
               speed = this.speed
           });
           this.SetData(new TransformComponent
           {
               value = this.transform
           });
           this.SetData(new AnimatorComponent
           {
               value = this.GetComponent<Animator>()
           });
        }
        
    }
}