/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class MessageStagingData
    {
        public string key;
        public object data;
    }
    public class MessageStagingManager : SingletonTamplate<MessageStagingManager>
    {
        public Dictionary<string, Queue<MessageStagingData>> MessageStagingMap = new Dictionary<string, Queue<MessageStagingData>>();

        public void Add(MessageStagingData data)
        {
            if (data == null || data.key == null) return;

            if(!MessageStagingMap.ContainsKey(data.key))
            {
                MessageStagingMap.Add(data.key,new Queue<MessageStagingData>());
            }

            if(MessageStagingMap[data.key] == null)
            {
                MessageStagingMap[data.key] = new Queue<MessageStagingData>();
            }

            var queue = MessageStagingMap[data.key];
            queue.Enqueue(data);
        }

        public MessageStagingData Get(string key)
        {
            if(MessageStagingMap.ContainsKey(key))
            {
                var value = MessageStagingMap[key];
                if (value != null && value.Count > 0)
                    return value.Dequeue();
            }
            return null;
        }

        public List<MessageStagingData> PopAll(string key)
        {
            List<MessageStagingData> result = null;

            if (MessageStagingMap.ContainsKey(key))
            {
                var value = MessageStagingMap[key];
                if (value != null && value.Count > 0)
                {
                    result = new List<MessageStagingData>(value.ToArray());
                    value.Clear();
                }
            }
            return result;
        }

        public void Clear(string key)
        {
            if(MessageStagingMap.ContainsKey(key) && MessageStagingMap[key] != null)
            {
                MessageStagingMap[key].Clear();
            }
        }

        public void ClearAll()
        {
            MessageStagingMap.Clear();
        }


    }

}
