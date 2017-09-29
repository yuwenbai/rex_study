/**
 * @Author lyb
 *  记录初始化的时候4个手的位置
 *
 */

using UnityEngine;
using System.Collections.Generic;

//由于右键创建配置文件
[CreateAssetMenu(fileName = "NewAnimatorHandPos", menuName = "ScriptableData/NewAnimatorHandPos")]
public class AnimatorHandPos : ScriptableObject
{
    [System.Serializable]
    public class PosDataBase
    {
        /// <summary>
        /// 模型运动的起点位置
        /// </summary>
        public Vector3 FromV3;
        /// <summary>
        /// 模型的旋转
        /// </summary>
        public Vector3 RotateV3;
        /// <summary>
        /// 模型的大小
        /// </summary>
        public Vector3 ScaleV3;
    }
    
    /// <summary>
    /// 4个手初始化位置列表
    /// </summary>
    public List<PosDataBase> HandPosList;
}