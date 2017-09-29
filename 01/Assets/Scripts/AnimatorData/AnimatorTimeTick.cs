using UnityEngine;
using System.Collections.Generic;

namespace projectQ
{



    class AnimTimeTickObject
    {

        public static Queue<AnimTimeTickObject> _pool = new Queue<AnimTimeTickObject>();
        public static AnimTimeTickObject Get()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            else
            {
                var o = new AnimTimeTickObject(0, null, float.MaxValue, -1);
                return o;
            }
        }

        public static void Return(AnimTimeTickObject o)
        {
            o.Reset();
            _pool.Enqueue(o);
        }

        public int name;
        public System.Action<string> action;
        public float ttr;
        public float _cttr;
        public int runTime;
        public string _param;

        public bool needDestory;

        public AnimTimeTickObject countTtr(float delta)
        {
            //Debug.LogError("Name " + this.name + "D:" + delta + "C" + _cttr);

            this._cttr -= delta;

            if (this._cttr <= 0)
            {

                // Debug.LogError("delta-" + _cttr);

                this._cttr = this.ttr; //重设时间
                if (this.action != null)
                {
                    try
                    {
                        this.action(this._param);
                    }
                    catch (System.Exception ex)
                    {
                        QLoger.ERROR("计时器错误" + ex.ToString());
                    }
                }
                if (this.runTime > 0)
                {
                    this.runTime--;
                    //Debug.LogError("runTime" + runTime);

                    if (this.runTime <= 0)
                    {

                        this.needDestory = true;
                    }
                }
            }
            //Debug.LogError("Name " + this.name + "D:" + delta + "C" + _cttr);

            return this;
        }
        public void Reset()
        {
            this.name = 0;
            this.action = null;
            this.ttr = float.MaxValue;
            this._cttr = this.ttr;
            this.runTime = -1;
            this.needDestory = false;
        }

        public AnimTimeTickObject(
            int name,
            System.Action<string> action,
            float timeStep,
            int runTime
            )
        {
            this.name = name;
            this.action = action;
            this.ttr = timeStep;
            this._cttr = this.ttr;
            this.runTime = runTime;
            this.needDestory = false;
        }
    }


    public class AnimTimeTick : MonoBehaviour
    {
        #region 静态引用
        private static AnimTimeTick _instance;
        public static AnimTimeTick Instance
        {
            get { return _instance; }
        }
        #endregion

        int name_idx = 0;
        public int getName()
        {
            return name_idx == int.MaxValue ? 0 : ++name_idx;
        }
        Dictionary<int, AnimTimeTickObject> _action_pool =
            new Dictionary<int, AnimTimeTickObject>();
        List<int> _remove_action = new List<int>();
        List<int> _running_action = new List<int>();
        public int SetAction(
            System.Action<string> action,
            float ttr,
            int time = -1,
            string param = ""
            )
        {

            var o = AnimTimeTickObject.Get();

            o.name = getName();
            o.action = action;
            o.ttr = ttr;
            o._cttr = o.ttr;
            o.runTime = time;
            o._param = param;

            _action_pool.Add(o.name, o);
            _running_action.Add(o.name);

            return o.name;
        }

        public void RemoveAction(System.Action action)
        {
            if (action == null)
            {
                return;
            }

            for (int i = 0; i < this._running_action.Count; i++)
            {
                var name = this._running_action[i];
                if (_action_pool[name].action.Equals(action))
                {
                    _remove_action.Add(name);
                }
            }

            if (_remove_action.Count > 0)
            {
                for (int i = 0; i < _remove_action.Count; i++)
                {
                    RemoveAction(_remove_action[i]);
                }
                _remove_action.Clear();
            }

        }

        public void RemoveAction(int name)
        {
            var o = _action_pool[name];
            AnimTimeTickObject.Return(o);
            _action_pool.Remove(name);
            _running_action.Remove(name);
        }
        private void Awake()
        {
            _instance = this;
        }
        private void Update()
        {
            TickTick(Time.deltaTime);
        }
        public void Destory()
        {
            _action_pool.Clear();
            _running_action.Clear();
        }
        public void TickTick(float delta)
        {
            //if(_action_pool.)


            for (int i = 0; i < _running_action.Count; i++)
            {
                int name = _running_action[i];

                if (_action_pool !=null && _action_pool[_running_action[i]].countTtr(delta).needDestory)
                {
                    _remove_action.Add(name);
                }
            }

            if (_remove_action.Count > 0)
            {
                for (int i = 0; i < _remove_action.Count; i++)
                {
                    RemoveAction(_remove_action[i]);
                }
                _remove_action.Clear();
            }
        }
    }


}
