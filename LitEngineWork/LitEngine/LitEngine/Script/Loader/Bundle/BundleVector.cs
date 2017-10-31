using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LitEngine
{
    namespace Loader
    {
        public class BundleVector
        {
            private Dictionary<string, BaseBundle> mList = new Dictionary<string, BaseBundle>();
            public BundleVector()
            {
            }

            public BaseBundle this[string key]
            {
                get
                {
                    return mList[key];
                }
                set
                {
                    mList[key] = value;
                }
            }

            public Dictionary<string, BaseBundle>.ValueCollection values
            {
                get
                {
                    return mList.Values;
                }
            }

            public Dictionary<string, BaseBundle>.KeyCollection Keys
            {
                get
                {
                    return mList.Keys;
                }
            }

            public void Clear(bool _clear)
            {
                ArrayList tlist = new ArrayList(mList.Keys);
                foreach (string tkey in tlist)
                {
                    Remove(tkey, _clear);
                }
            }
            public void ClearAllGC1(bool _clear)
            {
                ArrayList tlist = new ArrayList(mList.Keys);
                foreach (string tkey in tlist)
                {
                    ClearGC1(tkey, _clear);
                }
            }

            public void ClearGC1(string _key, bool _clear)
            {
                if (!Contains(_key))
                    return;
                this[_key].ClearGC(_clear);
            }

            public bool Contains(string _key)
            {
                return mList.ContainsKey(_key);
            }

            public void Add(BaseBundle _bundle)
            {
                _bundle.Parent = this;
                mList.Add(_bundle.AssetName, _bundle);
            }

            public void Remove(BaseBundle _bundle, bool _Clear)
            {
                if (_bundle == null)
                {
                    Debug.LogWarning("can not remove null");
                    return;
                }

                Remove(_bundle.AssetName, _Clear);
            }

            public void Remove(string _key, bool _Clear)
            {
                if (!Contains(_key))
                    return;
                BaseBundle tbundle = this[_key];
                mList.Remove(_key);
                tbundle.Destory(_Clear);
            }

            public void ReleaseBundle(BaseBundle _bundle)
            {
                if (_bundle == null)
                {
                    Debug.LogWarning("can not release null");
                    return;
                }
                ReleaseBundle(_bundle.AssetName);
            }

            public void ReleaseBundle(string _key)
            {
                if (Contains(_key))
                    this[_key].Release();
            }
        }
    }
}

   
