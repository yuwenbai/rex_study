using UnityEngine;
using System.Collections.Generic;

//由于右键创建配置文件
[CreateAssetMenu(fileName = "NewAnimatorHandData", menuName = "ScriptableData/NewAnimatorHandData")]
public class AnimatorHandData : ScriptableObject
{
    /// <summary>
    /// 开始的位置
    /// </summary>
    public Vector3 StartV3;
    /// <summary>
    /// 收的缩放比例
    /// </summary>
    public Vector3 ScaleV3;
    /// <summary>
    /// 移动到p0点的时间
    /// </summary>
    public float TimeMoveP0;
    /// <summary>
    /// 出牌动作落下停顿时间
    /// </summary>
    public float DelayTime;
    /// <summary>
    /// 移动到p1点的时间
    /// </summary>
    public float TimeMoveP1;
    /// <summary>
    /// 回到初始位置前的延迟执行方法
    /// </summary>
    public float DelayMoveInitTime;
    /// <summary>
    /// 移动到初始点的时间
    /// </summary>
    public float TimeMoveInit;
    /// <summary>
    /// 到达p0点手抬起的高度 - 仅适用于插牌动作
    /// </summary>
    public float UpHight;
}