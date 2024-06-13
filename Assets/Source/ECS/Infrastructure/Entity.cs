using UnityEngine;

namespace ECS.Infrastructure
{
    public class Entity : MonoBehaviour
    {
        public const int NO_ID = -1;

        public int Id
        {
            get { return this._id; }
        }

        private int _id;
        private World _world;

        public void Init(World world)
        {
            this._id = world.CreateEntity();
            this._world = world;
            this.OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public void Dispose()
        {
            this._world.DestroyEntity(this._id);
            this._world = null;
            this._id = NO_ID;
        }
    }
}