using System;
using UnityEngine;
namespace LitEngine
{
    using ScriptInterface;
    public interface MonoInterface
    {
        Transform transform { get; }
        GameObject gameobject { get;}
        BehaviourInterfaceBase Parent { get;}
        void MonoInterface(BehaviourInterfaceBase _parent);
        void Awake();
    }
}
