/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMainDown : UIMainBase
    {
        [Tooltip("官方麻将试玩群")]
        public GameObject OfficialGroupObj;

        [Tooltip("普通用户下方的按钮群")]
        public GameObject FunctionBtnObj;
        
        /// <summary>
        /// 按钮列表
        /// </summary>
        public List<UIMainFunctionBtn> FunctionList;
        
        private MjRoom mjHall;
        public void SetData(MjRoom mjHall)
        {
            this.mjHall = mjHall;
        }

        void Awake(){}

        void Start()
        {
            foreach (UIMainFunctionBtn fBtn in FunctionList)
            {
                fBtn.OnClickCallBack = ClickFunctionOpenCallBack;
            }
        }
        
        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            this.OfficialGroupObj.gameObject.SetActive(true);
            this.FunctionBtnObj.gameObject.SetActive(true);

            foreach (UIMainFunctionBtn fBtn in FunctionList)
            {
                fBtn.RefreshUI(state);
            }

            if(state == UIMain.EnumUIMainState.MasterCheck || state == UIMain.EnumUIMainState.Master
                || state == UIMain.EnumUIMainState.LinkedMjHall || state == UIMain.EnumUIMainState.NotLinkMjHall)
            {
                this.OfficialGroupObj.gameObject.SetActive(false);                
            }

            if (state == UIMain.EnumUIMainState.MasterCheck)
            {
                this.FunctionBtnObj.gameObject.SetActive(false);
            }
        }
        
        #region 按钮点击触发事件-----------------------------------------

        void ClickFunctionOpenCallBack(FunctionEnum fType)
        {
            switch (fType)
            {
                case FunctionEnum.FUNCTION_ROOM_CARD:
                case FunctionEnum.FUNCTION_GOODS:
                    //桌卡
                    if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID != 0)
                    {
                        // 馆主
                        this.ui.LoadUIMain("UIRoomCardMaster");
                    }
                    else
                    {
                        //非馆主
                        this.ui.LoadUIMain("UIRoomCard");
                    }
                    break;
                case FunctionEnum.FUNCTION_MESSAGE:
                    //消息
                    this.ui.LoadUIMain("UIMessage");
                    break;
                case FunctionEnum.FUNCTION_ACTIVITY:
                    //活动
                    this.ui.LoadUIMain("UIActivity");
                    break;
                case FunctionEnum.FUNCTION_RULE:
                    //规则
                    int configId = MemoryData.MahjongPlayData.GetMahjongPlayOnlyConfigId();
                    if (configId != 0)
                    {
                        this.ui.LoadUIMain("UIRule", configId);
                    }
                    else
                    {
                        this.ui.LoadUIMain("UIRuleBaseInfo");
                    }
                    break;
                case FunctionEnum.FUNCTION_BATTLE_INFO:
                    //战绩
                    this.ui.LoadUIMain("UIMessageBattle");
                    break;
                case FunctionEnum.FUNCTION_RED_PACKET:
                    //发红包
                    this.ui.LoadUIMain("UIRedPackge");
                    break;
                case FunctionEnum.FUNCTION_LOOK_CHECK:
                    //巡视
                    this.ui.ChangeState(UIMain.EnumUIMainState.MasterCheck);
                    break;
                case FunctionEnum.FUNCTION_MANAGE:
                    {
                        SDKManager.Instance.SDKFunction("WWW_OPEN_ADMIN",new WebSDKParams("WWW_OPEN_ADMIN"));
                    }
                    break;
            }
        }

        #endregion-------------------------------------------------------
    }
}