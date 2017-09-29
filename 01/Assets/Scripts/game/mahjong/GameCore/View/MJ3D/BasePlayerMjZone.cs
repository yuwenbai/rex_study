/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MjZoneReconnectData
{

}
public abstract class BasePlayerMjZone : ICanClearUp, ICanReconect
{
    private MonoHelper _MonoHelper = null;
    public MonoHelper MonoHelper
    {
        get
        {
            if (_MonoHelper == null)
            {
                GameObject obj = new GameObject();
                _MonoHelper = obj.AddComponent<MonoHelper>();
            }
            return _MonoHelper;
        }
    }
    protected int _SeatUIIndex = -1;
    public virtual void Init(int seatUIIndex)
    {
        _SeatUIIndex = seatUIIndex;
    }

    public virtual void SetCardHeighLight(bool state, int mjCode)
    { }
    public virtual void ClearUp()
    {
        if (_MonoHelper != null)
        {
            _MonoHelper.StopAllCoroutines();
            GameObject.Destroy(_MonoHelper.gameObject);
            _MonoHelper = null;
        }
    }
    /// <summary>
    /// 必须继承这个方法
    /// </summary>
    /// <param name="param"></param>
    public virtual void OnReconnect(object param)
    {

    }
}
