/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMessageBattleModel : UIModelBase
    {
        public UIMessageBattle UI
        {
            get { return _ui as UIMessageBattle; }
        }

        private List<GameObject> MBattleList = new List<GameObject>();

        /// <summary>
        /// 被选中按钮的桌号列表
        /// </summary>
        private List<int> BattleInfoSelectList = new List<int>();

        #region override----------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_MessageBattleDelete_Succ,
                GEnum.NamedEvent.SysUI_MessageBattle_Succ,
                GEnum.NamedEvent.ERCCloseingAndRelushUI,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_MessageBattleDelete_Succ:

                    int resultCode = (int)data[0];
                    if (resultCode == 0)
                    {
                        BtnInfoSelectAllClick();

                        //成功。。刷新界面
                        Init();
                    }
                    break;
                case GEnum.NamedEvent.SysUI_MessageBattle_Succ:
                case GEnum.NamedEvent.ERCCloseingAndRelushUI:
                    UI.StopSendLoading();
                    Init();
                    break;
            }
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb----------------战绩面板初始化----------------------------------------------

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            MessageBattleInit();
        }

        public void MessageBattleInit()
        {
            bool isShow = MemoryData.ResultData.ResultData.Count <= 0 ? true : false;
            BtnStateShow(isShow);

            MBattleList = new List<GameObject>();

            /*
            int childCount = UI.GrideBattleObj.transform.childCount;
            List<GameResult> _resultData = new List<GameResult>();
            for (int i = childCount; i < childCount + 5; i++)
            {
                if (MemoryData.ResultData.ResultData.Count > childCount + 5)
                {
                    _resultData.Add(MemoryData.ResultData.ResultData[i]);
                }
            }
            */

            UITools.CreateChild<GameResult>(UI.GrideBattleObj.transform, null, MemoryData.ResultData.ResultData, (go, messageData) =>
            {
                UIMessageBattleList bList = go.GetComponent<UIMessageBattleList>();

                bList.OnClickCallBack = MessageBattleListSelectBtnCallBack;

                bList.OnBattleInfoClickCallBack = MessageBattleListInfoBtnCallBack;

                bList.MessageBattleInit(messageData);

                MBattleList.Add(go);
            });

            UI.GrideBattleObj.Reposition();
            UI.ScrollViewObj.ResetPosition();
        }

        /// <summary>
        /// 按钮组件状态显示
        /// isShow = true 没有可查看战绩
        /// </summary>
        void BtnStateShow(bool isShow)
        {
            UI.NoMessageObj.SetActive(isShow);
            UI.DeleteBtn.SetActive(!isShow);
            UI.SelectAllBtn.gameObject.SetActive(!isShow);
        }

        #endregion-----------------------------------------------------------------------------

        #region lyb----------------按钮相关的处理方法------------------------------------------

        /// <summary>
        /// 点击删除按钮的处理方法
        /// </summary>
        public void BtnDeleteClick()
        {
            ModelNetWorker.Instance.C2SMessageBattleDeleteReq(MessageBattleStateEnum.MESSAGE_BATTLE_DELETE, BattleInfoSelectList);
        }

        /// <summary>
        /// 点击全选按钮
        /// </summary>
        public void BtnInfoSelectAllClick()
        {
            foreach (GameObject obj in MBattleList)
            {
                UIMessageBattleList battleList = obj.GetComponent<UIMessageBattleList>();
                UIToggle toggle = battleList.MessageBattleSelectBtn;
                toggle.Set(UI.SelectAllBtn.value);

                BattleInfoSelect_Check(UI.SelectAllBtn.value, battleList.MessageBattleId);
            }
        }

        /// <summary>
        /// 每条战绩列表的选择按钮被点击回调方法
        /// </summary>
        void MessageBattleListSelectBtnCallBack(bool isSelect, int deskId)
        {
            if (!isSelect)
            {
                UI.SelectAllBtn.Set(false);
            }

            BattleInfoSelect_Check(isSelect, deskId);
        }

        /// <summary>
        /// 每条战绩列表的详细信息按钮被点击回调方法
        /// </summary>
        void MessageBattleListInfoBtnCallBack(int deskId)
        {
            UI.LoadUIMain("UIMahjongResult", deskId, 1);
        }

        #endregion-----------------------------------------------------------------------------

        /// <summary>
        /// 选中一条战绩数据的时候检测选中列表中是否存在如果不存在则加入
        /// 取消一条战绩数据的时候检测选中列表中是否存在如果存在则删除
        /// </summary>
        void BattleInfoSelect_Check(bool isSelect, int deskId)
        {
            bool isbol = false;
            foreach (int dId in BattleInfoSelectList)
            {
                if (dId == deskId)
                {
                    //当前操作的牌桌id存在
                    isbol = true;
                    if (!isSelect)
                    {
                        //如果是删除操作则移除该id
                        BattleInfoSelectList.Remove(dId);
                        return;
                    }
                    else
                    {
                        //如果是加入操作则不执行直接跳过
                        return;
                    }
                }
            }

            if (!isbol)
            {
                //当前操作的牌桌id不存在
                if (isSelect)
                {
                    //如果是加入操作则加入列表
                    BattleInfoSelectList.Add(deskId);
                }
            }

            if (BattleInfoSelectList.Count == MBattleList.Count)
            {
                UI.SelectAllBtn.Set(true);
            }
        }
    }
}