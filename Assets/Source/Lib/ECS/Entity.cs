using UnityEngine;

namespace Lib.ECS
{
    public class Entity : MonoBehaviour
    {
        private const int NO_ID = -1;

        public int Id
        {
            get { return this._id; }
        }

        private int _id;
        private World _world;

        #region Lifetime
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
        
        #endregion
        
        #region Data

        public void SetData<T>(T data) where T : struct
        {
            this._world.SetComponentData(this._id, ref data);
        }

        public ref T GetData<T>() where T : struct
        {
            return ref this._world.GetComponentData<T>(this._id);
        }
        
        #endregion
    }
}