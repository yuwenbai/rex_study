/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItem  {
    private GameObject _EffectObj;
    public GameObject EffectObj
    {
        get { return _EffectObj; }
    }
    private List<ParticleSystem> _Effects = new List<ParticleSystem>();
    public EffectItem(GameObject obj)
    {
        _EffectObj = obj;
        if (_EffectObj == null)
        {
            DebugPro.DebugError("_EffectObj  is null");
        }
        ParticleSystem[] particles = obj.GetComponentsInChildren<ParticleSystem>();
        if (particles == null || particles.Length <= 0)
        {
            DebugPro.DebugError("no effects in obj.name:", obj.name);
            return;
        }
        for (int i = 0; i < particles.Length; i++)
        {
            _Effects.Add(particles[i]);
        }
    }
    public void Update(float dt)
    {
 ///
    }
    public void PauseEffect()
    {
        for (int i = 0; i < _Effects.Count; i++)
        {
            if (_Effects[i].isPlaying)
            {
                _Effects[i].Pause();
            }
        }
    }
    public void ResumeEffect()
    {
        for (int i = 0; i < _Effects.Count; i++)
        {
            if (_Effects[i].isStopped)
            {
                _Effects[i].Play();
            }
        }
    }
    public void StopEffect()
    {
        for (int i = 0; i < _Effects.Count; i++)
        {
            if (_Effects[i].isPlaying || _Effects[i].isStopped)
            {
                _Effects[i].Stop();
            }
        }
    }
    public void DestroyEffect()
    {
 
    }
}
