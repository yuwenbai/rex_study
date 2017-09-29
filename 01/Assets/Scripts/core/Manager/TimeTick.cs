using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{



    class TimeTickObject
    {

        public static Queue<TimeTickObject> _pool = new Queue<TimeTickObject>();
        public static TimeTickObject Get()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            else
            {
                var o = new TimeTickObject(0, null, float.MaxValue, -1);
                return o;
            }
        }

        public static void Return(TimeTickObject o)
        {
            o.Reset();
            _pool.Enqueue(o);
        }

        public int name;
        public System.Action action;
        public float ttr;
        public float _cttr;
        public int runTime;

        public bool needDestory;

        public TimeTickObject countTtr(float delta)
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
                        this.action();
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

        public TimeTickObject(
            int name,
            System.Action action,
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


    public class TimeTick : SingletonTamplate<TimeTick>
    {

        int name_idx = 0;
        public int getName()
        {
            return name_idx == int.MaxValue ? 0 : ++name_idx;
        }
        Dictionary<int, TimeTickObject> _action_pool =
            new Dictionary<int, TimeTickObject>();
        List<int> _remove_action = new List<int>();
        List<int> _running_action = new List<int>();
        public int SetAction(
            System.Action action,
            float ttr,
            int time = -1)
        {

            var o = TimeTickObject.Get();

            o.name = getName();
            o.action = action;
            o.ttr = ttr;
            o._cttr = o.ttr;
            o.runTime = time;

            _action_pool.Add(o.name, o);
            _running_action.Add(o.name);

            //Debug.LogError(o.ttr);

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
            TimeTickObject.Return(o);
            _action_pool.Remove(name);
            _running_action.Remove(name);
        }

        public void TickTick(float delta)
        {
            //if(_action_pool.)


            for (int i = 0; i < _running_action.Count; i++)
            {
                int name = _running_action[i];

                if (_action_pool[_running_action[i]].countTtr(delta).needDestory)
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
