using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
namespace NetTool
{
    public class HttpData
    {
        static protected HttpNet sHttpObj = null;

        string mKey;
        string mUrl;
        byte[] mBuffer;
        Dictionary<string, string> mHeaders;
        System.Action<string, byte[]> mDelgate;
        bool mSending = false;
        public HttpData(string _key,string _url, System.Action<string,byte[]> _delgate)
        {
            mKey = _key;
            mUrl = _url;
            mDelgate = _delgate;
            mHeaders =new Dictionary<string, string>();
            if (sHttpObj == null)
            {
                GameObject tobj = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(tobj);
                sHttpObj = tobj.AddComponent<HttpNet>();
                tobj.name = "Http" + "-Object";
            }
        }
        static public void DestroyHttpobj()
        {
            if (sHttpObj == null) return;
            GameObject.DestroyImmediate(sHttpObj.gameObject);
            sHttpObj = null;
        }
        public void SetHeard(Dictionary<string, string> _headers)
        {
            mHeaders = _headers;
        }
        public void AddHeard(string _key,string _head)
        {
            if (mHeaders == null)
                mHeaders = new Dictionary<string, string>();
            if (mHeaders.ContainsKey(_key))
                mHeaders[_key] = _head;
            else
                mHeaders.Add(_key, _head);
        }
        public void SetHeardByKey(string _key,string _data)
        {
            if (mHeaders.ContainsKey(_key))
                mHeaders[_key] = _data;
            else
                DLog.LOG(DLogType.Error,"无效的header关键字");
        }
        public void AddBinaryData(byte[] _data)
        {
            mBuffer = _data;
        }
        public void SendMessage()
        {
            if (mSending) return;
            mSending = true;
            sHttpObj.StartCoroutine(Send());
        }

        IEnumerator Send()
        {
           // System.Collections.Hashtable headers = wform.headers;
           // headers["Cookie"] = define.sessionName;
            WWW www = new WWW(mUrl, mBuffer, mHeaders);
            yield return www;
            if (www.error == null)
            {
                if (mDelgate != null)
                    mDelgate(mKey, www.bytes);
                www.Dispose();
            }
            else {
                DLog.LOG(DLogType.Error,"HttpErro-"+www.error);
                if (mDelgate != null)
                    mDelgate(mKey, null);
            }
            mSending = false;
        }

    }
    public class HttpNet : MonoBehaviour
    {
        public HttpNet()
        {
        }
        static public void DestroyNet()
        {
            HttpData.DestroyHttpobj();
        }
        static public void SendMessage(string _url,byte[] _data, string _key, System.Action<string, byte[]> _delegate)
        {
            HttpData tdata = new HttpData(_key, _url, _delegate);
            tdata.AddBinaryData(_data);
            tdata.SendMessage();
        }
        static public void SendMessage(HttpData _data)
        {
            _data.SendMessage();
        }
    }
}
