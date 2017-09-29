/**
 * @Author lyb
 * 大厅最底下的每个按钮控制脚本
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public enum FunctionEnum
    {
        /// <summary>
        /// Null
        /// </summary>
        FUNCTION_NULL = -1,
        /// <summary>
        /// 桌卡
        /// </summary>
        FUNCTION_ROOM_CARD = 0,
        /// <summary>
        /// 消息
        /// </summary>
        FUNCTION_MESSAGE = 1,
        /// <summary>
        /// 活动
        /// </summary>
        FUNCTION_ACTIVITY = 2,
        /// <summary>
        /// 规则
        /// </summary>
        FUNCTION_RULE = 3,
        /// <summary>
        /// 战绩
        /// </summary>
        FUNCTION_BATTLE_INFO = 4,
        /// <summary>
        /// 发红包
        /// </summary>
        FUNCTION_RED_PACKET = 5,
        /// <summary>
        /// 巡视
        /// </summary>
        FUNCTION_LOOK_CHECK = 6,
        /// <summary>
        /// 后台管理
        /// </summary>
        FUNCTION_MANAGE = 7,
        /// <summary>
        /// 进货
        /// </summary>
        FUNCTION_GOODS = 8,
    }

    public class UIMainFunctionBtn : UIMainBase
    {
        public delegate void FunctionBtnDelegate(FunctionEnum fType);
        public FunctionBtnDelegate OnClickCallBack;

        public FunctionEnum FunctionType = FunctionEnum.FUNCTION_NULL;
        /// <summary>
        /// 馆主显示否
        /// </summary>
        public bool isMaster;

        void Start(){}

        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            if (state == UIMain.EnumUIMainState.Master)
            {
                gameObject.SetActive(isMaster);
            }
            else
            {
                gameObject.SetActive(!isMaster);
            }            
        }

        /// <summary>
        /// 初始化按钮的功能
        /// </summary>
        public void FunctionBtnInit()
        {

        }
        
        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(FunctionType);
            }
        }
    }
}
