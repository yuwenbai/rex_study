using System.Collections;
using System.Collections.Generic;
namespace projectQ
{
    public class EventDispatcher : SingletonTamplate<EventDispatcher>
    {
        public EventDispatcher()
        {
            m_event = new Dictionary<string, System.Action<object[]>>();
            m_cached_event = new Queue<KeyValuePair<string, object[]>>();
            m_nameEventStringPairs = new Dictionary<GEnum.NamedEvent, string>();
        }

        private Queue<KeyValuePair<string, object[]>> m_cached_event;

        private IDictionary<string, System.Action<object[]>> m_event;
        private IDictionary<GEnum.NamedEvent, string> m_nameEventStringPairs;
        static private void CheckAddNameEventStringPair(GEnum.NamedEvent k)
        {
            if (!Instance.m_nameEventStringPairs.ContainsKey(k))
            {
                Instance.m_nameEventStringPairs.Add(k, k.ToString());
            }
        }

        static public void AddEvent(GEnum.NamedEvent k, System.Action<object[]> v)
        {
            CheckAddNameEventStringPair(k);
            AddEvent(Instance.m_nameEventStringPairs[k], v);
        }

        static public void AddEvent(string k, System.Action<object[]> v)
        {
            if (Instance.m_event.ContainsKey(k))
            {
                Instance.m_event[k] -= v;
                Instance.m_event[k] += v;
            }
            else
            {
                Instance.m_event.Add(k, v);
            }
        }
        static public void RemoveEvent(GEnum.NamedEvent k, System.Action<object[]> v)
        {
            CheckAddNameEventStringPair(k);
            RemoveEvent(Instance.m_nameEventStringPairs[k], v);
        }

        static public void RemoveEvent(string k, System.Action<object[]> v)
        {
            if (Instance.m_event.ContainsKey(k))
            {
                Instance.m_event[k] -= v;
                if (Instance.m_event[k] == null)
                {
                    Instance.m_event.Remove(k);
                }
            }
        }

        static public void RegiestEvent(string k, System.Action<object[]> v)
        {
            Instance.m_event[k] = v;
        }

        static public void UnregiestEvent(string k)
        {
            if (Instance.m_event.ContainsKey(k))
            {
                Instance.m_event.Remove(k);
            }
        }

        static public void CacheEventWhenCanFire(string k, params object[] var)
        {
            Instance.m_cached_event.Enqueue(new KeyValuePair<string, object[]>(k, var));
        }

        static public void FireCacheedEvent()
        {
            for (int i = 0; i < Instance.m_cached_event.Count; i++)
            {
                var e = Instance.m_cached_event.Dequeue();
                if (Instance.m_event.ContainsKey(e.Key)
                    && Instance.m_event[e.Key] != null)
                {
                    Instance.m_event[e.Key](e.Value);
                }
                else
                {
                    Instance.m_cached_event.Enqueue(e);
                }
            }
        }

        static public void FireSysEvent(GEnum.NamedEvent k, params object[] var)
        {
            object[] temp;
            if (var == null)
            {
                temp = new object[1];
            }
            else
            {
                temp = new object[var.Length + 1];
                var.CopyTo(temp, 1);
            }
            temp[0] = k;
            FireEvent(k, temp);
        }

        static public void FireEvent(GEnum.NamedEvent k, params object[] var)
        {
            CheckAddNameEventStringPair(k);
            FireEvent(Instance.m_nameEventStringPairs[k], var);
        }

        static public void FireEvent(string k, params object[] var)
        {

            try {
                if (Instance.m_event.ContainsKey(k)
                && Instance.m_event[k] != null) {
                    Instance.m_event[k](var);
                } else {
                    DebugPro.DebugWarning("can't find event:", k);
                }
            } catch (System.Exception ex) {
                string v = "";
                for (int i = 0; i < var.Length; i++) {
                    v += "{" + var[i].ToString() + "}";
                }
                QLoger.ERROR("事件异常" + k.ToString() +v+ ex.ToString());
            }
        }
    }
}