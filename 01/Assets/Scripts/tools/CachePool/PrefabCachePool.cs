/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

public class PrefabCachePool : MonoBehaviour
{
    private bool _UseCachePool = false;
    private static Transform _CachePoolRoot = null;
    private static PrefabCachePool _Instance = null;
    public static PrefabCachePool Intance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "PrefabCachePool";
                _Instance = obj.AddComponent<PrefabCachePool>();
                _CachePoolRoot = obj.transform;
            }
            return _Instance;
        }
    }
    private Dictionary<int, List<GameObject>> _Pool = new Dictionary<int, List<GameObject>>();
    private const int _MaxCacheCount = 10;

    public void CachePrefabs()
    {//PrefabConfig
        List<BaseXmlBuild> list = MemoryData.XmlData.XmlBuildDataDic["PrefabConfig"];
        if (list == null)
        {
            DebugPro.DebugError("List<PrefabConfig> is null");
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            PrefabConfig config = list[i] as PrefabConfig;
            if (config == null)
            {
                continue;
            }
            int count = 0;
            int.TryParse(config.CacheCount, out count);
            for (int j = 0; j < count; j++)
            {
                CacheGameObject(config);
            }
        }

    }
    //缓存配置表中对应数量的物体
    public void CacheGameObject(PrefabConfig config)
    {
        GameObject instance = LoadGameObject(config);
        if (instance == null)
        {
            DebugPro.DebugError("CacheGameObject error return null");
            return;
        }
        int id = -1;
        int.TryParse(config.ID, out id);
        if (id != -1)
        {
            if (!_Pool.ContainsKey(id))
            {
                _Pool.Add(id, new List<GameObject>());
            }
            _Pool[id].Add(instance);
        }
    }
    private GameObject LoadGameObject(PrefabConfig config)
    {
        if (config == null)
        {
            DebugPro.DebugError("PrefabConfig is null");
            return null;
        }
        Object obj = Resources.Load(config.Path);
        if (obj == null)
        {
            DebugPro.DebugError("Resources.Load error  return null");
            return null;
        }

        GameObject instance = GameObject.Instantiate(obj) as GameObject;
        if (instance == null)
        {
            DebugPro.DebugError("GameObject.Instantiate error return null");
            return null;
        }
        if (_UseCachePool)
        {
            int id = -1;
            int.TryParse(config.ID, out id);
            if (id != -1)
            {
                PoolItem poolItem = instance.AddComponent<PoolItem>();
                poolItem.PoolID = id;
            }
        }
        instance.transform.parent = _CachePoolRoot;
        GameObjectHelper.NormalizationTransform(instance.transform);
        instance.gameObject.SetActive(false);
        // Resources.UnloadAsset(obj);
        return instance;
    }
    public GameObject CreateGameObject(int prefabConfigID)
    {
        PrefabConfig config = MemoryData.XmlData.FindPrefabConfigById(prefabConfigID.ToString());
        return CreateGameObject(config);
    }
    public GameObject CreateGameObject(string path)
    {
        Object obj = Resources.Load(path);
        if (obj == null)
        {
            DebugPro.DebugError("Resources.Load error  return null");
            return null;
        }

        GameObject instance = GameObject.Instantiate(obj) as GameObject;
        if (instance == null)
        {
            DebugPro.DebugError("GameObject.Instantiate error return null");
            return null;
        }
       
        instance.transform.parent = _CachePoolRoot;
        GameObjectHelper.NormalizationTransform(instance.transform);
        instance.gameObject.SetActive(false);
        // Resources.UnloadAsset(obj);
        return instance;
    }
    public GameObject CreateGameObject(PrefabConfig config)
    {
        if (NullHelper.IsObjectIsNull(config))
        {
            return null;
        }
        GameObject obj = null;
        int id = -1;
        int.TryParse(config.ID, out id);

        if (_UseCachePool)
        {
            if (!_Pool.ContainsKey(id) || _Pool[id].Count <= 0)
            {
                CacheGameObject(config);
            }
            obj = _Pool[id][_Pool[id].Count - 1];
            _Pool[id].Remove(obj);
        }
        else
        {
            obj = LoadGameObject(config);
        }
        return obj;
    }
    public void DestroyGameObject(GameObject obj)
    {
        if (!_UseCachePool)
        {
            GameObject.Destroy(obj);
            return;
        }
        if (obj == null)
        {
            DebugPro.DebugError("DestroyGameObject obj is  null");
            return;
        }
        PoolItem poolData = obj.GetComponent<PoolItem>();
        if (poolData == null)
        {
            return;
        }
        if (_Pool.ContainsKey(poolData.PoolID))
        {
            _Pool[poolData.PoolID].Add(obj);
        }
        else
        {
            DebugPro.DebugError("DestroyGameObject error obj is :" + obj.name);
        }
    }

}
