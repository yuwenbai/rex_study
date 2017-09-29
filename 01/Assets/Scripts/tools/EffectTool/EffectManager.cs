/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
public class EffectManager : MonoBehaviour
{
    private List<EffectItem> _GameEffects = new List<EffectItem>();

    private static EffectManager _Instance = null;

    public static EffectManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "EffectManager";
                _Instance = obj.AddComponent<EffectManager>();
            }
            return _Instance;
        }
    }
    public EffectItem CreateEffect(string path)
    {

        GameObject effect = PrefabCachePool.Intance.CreateGameObject(path);
        if (effect == null)
        {
            DebugPro.DebugError("PrefabCachePool CreateGameObject return null,path :", path);
            return null;
        }
        EffectItem effectItem = new EffectItem(effect);
        _GameEffects.Add(effectItem);
        return effectItem;
    }
    public EffectItem CreateEffect(PrefabConfig config)
    {
        if (config == null)
        {
            DebugPro.DebugError("CreateEffect PrefabConfig is null");
            return null;
        }
        GameObject effect = PrefabCachePool.Intance.CreateGameObject(config);
        if (effect == null)
        {
            DebugPro.DebugError("PrefabCachePool CreateGameObject return null, config id :", config.ID);
            return null;
        }
        EffectItem effectItem = new EffectItem(effect);
        _GameEffects.Add(effectItem);
        return effectItem;
    }

    void Update()
    {
        for (int i = 0; i < _GameEffects.Count; i++)
        {
            _GameEffects[i].Update(Time.deltaTime);
        }
    }

    public void PauseEffects()
    {
        for (int i = 0; i < _GameEffects.Count; i++)
        {
            _GameEffects[i].PauseEffect();
        }
    }
    public void ResumeEffects()
    {
        for (int i = 0; i < _GameEffects.Count; i++)
        {
            _GameEffects[i].ResumeEffect();
        }
    }
    public void DestroyAllEffects()
    {
        for (int i = 0; i < _GameEffects.Count; i++)
        {
            DestroyEffect(_GameEffects[i]);
        }
    }
    public void DestroyEffect(EffectItem item)
    {
        if (item == null)
        {
            return;
        }
        item.DestroyEffect();
        PrefabCachePool.Intance.DestroyGameObject(item.EffectObj);
    }

}
    