/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using projectQ;
using DG.Tweening;
public abstract class GamblingAnimBase
{
    protected System.Action<GamblingAnimBase> m_Action;
    protected Transform m_AnimParent;
    public GamblingAnimBase(Transform animParent, object[] data, System.Action<GamblingAnimBase> animCallback)
    {
        m_AnimParent = animParent;
        m_Action = animCallback;
    }
    public abstract void OnShow();
}
public class GamblingAnimZhama : GamblingAnimBase
{
    private EnumMjOpenMaType _MjOpenMaType;
    private int _SeatID;
    private Vector3 _TargetPos = Vector3.zero;
    public GamblingAnimZhama(Transform animParentTrans, object[] data, System.Action<GamblingAnimBase> animCallback)
        : base(animParentTrans, data, animCallback)
    {
        if (data != null)
        {
            _MjOpenMaType = (EnumMjOpenMaType)data[0];
            if (!NullHelper.IsInvalidIndex(1, data))
            {
                _SeatID = (int)data[1];
            }
            if (!NullHelper.IsInvalidIndex(2, data))
            {
                _TargetPos = (Vector3)data[2];
            }
        }
    }

    public override void OnShow()
    {
        string path = null;
        switch (_MjOpenMaType)
        {
            case EnumMjOpenMaType.MaiMa:
            case EnumMjOpenMaType.ZhaMa:
            case EnumMjOpenMaType.YiMaQuanZhong:
                path = PrefabPathDefine.ZhaMaAnim;
                break;
            case EnumMjOpenMaType.ZhuaNiao:
                path = PrefabPathDefine.ZhuaNiaoAnim;
                break;
            default:
                break;
        }
        if (path==null)
        {
            DebugPro.DebugError("configID can't find  ui type: _MjOpenMaType:", _MjOpenMaType);
            return;
        }
        EffectItem effect = EffectManager.Instance.CreateEffect(path);
        if (NullHelper.IsObjectIsNull(effect))
        {
            return;
        }
        if (NullHelper.IsObjectIsNull(m_AnimParent))
        {
            return;
        }
        GameObject effectObj = effect.EffectObj;
        if (NullHelper.IsObjectIsNull(effectObj))
        {
            return;
        }
        effectObj.gameObject.SetActive(true);
        effectObj.transform.parent = m_AnimParent;
        GameObjectHelper.NormalizationTransform(effectObj.transform);

        Vector3 deltaVec = _TargetPos - effectObj.transform.position;
        if (deltaVec.x < 0)
        {//翻转图片
            Vector3 scale = Vector3.one;
            scale.x = -scale.x;
            effectObj.transform.localScale = scale;
        }
        float angle = Mathf.Atan(deltaVec.x / deltaVec.y) * 180f / Mathf.PI;
        Vector3 deltaRotation = Vector3.zero;
        deltaRotation.z = 90 - Mathf.Abs(angle);
        // DebugPro.DebugInfo("===== angle is:", angle);
        effectObj.transform.localRotation = Quaternion.Euler(deltaRotation);
        DG.Tweening.ShortcutExtensions.DOMove(effectObj.transform, _TargetPos, ConstDefine.CatchHorseAnimTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjGameAnimOverNotify, this._MjOpenMaType, this._SeatID);
            UITexture tex = effectObj.transform.GetComponentInChildren<UITexture>();
            if (tex != null)
            {
                tex.gameObject.SetActive(false);
            }
            DG.Tweening.ShortcutExtensions.DOMove(effectObj.transform, effectObj.transform.position, ConstDefine.CatchHorseAnimSmokeTime).OnComplete(() =>
            {
                //这个只是希望掩饰做个回掉
                if (m_Action != null)
                {
                    m_Action(this);
                    m_Action = null;
                }
                else
                {
                    DebugPro.DebugError("_Action is null");
                }
            });
        });
    }

}