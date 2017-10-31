using UnityEngine;
using System;
namespace LitEngine
{
    public interface ManagerInterface : IDisposable
    {
        void DestroyManager();
    }
    public class MonoManagerBase : MonoBehaviour, ManagerInterface
    {
        protected bool mDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool _disposing)
        {
            if (mDisposed)
                return;

            if (_disposing)
                DisposeNoGcCode();

            mDisposed = true;
        }
        virtual protected void DisposeNoGcCode()
        {
        }

        virtual public void DestroyManager()
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
}
