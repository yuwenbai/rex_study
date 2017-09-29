/****************
 * 
 * @Author GarFey
 * 
 * ***************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class CreateRoomDown : MonoBehaviour
    {
        UICreateRoomModel Model;
        /// <summary>
        /// 上次玩法说明文字
        /// </summary>
        public UILabel UpPlayTitle;
        /// <summary>
		/// The button quick room. 快速开桌按钮 全部
		/// </summary>
		public GameObject FullBtnQuickRoom;
        /// <summary> 
		/// 上次玩法名称 全部
		/// </summary>
		public UILabel FullUplayName;
        /// <summary>
		/// 上次玩法规则 全部
		/// </summary>
		public UILabel FullUpRuleInfo;

        /// <summary>
        /// The button quick room. 快速开桌按钮 少数
        /// </summary>
        public GameObject NumBtnQuickRoom;
        /// <summary> 
		/// 上次玩法名称 少数
		/// </summary>
		public UILabel NumUplayName;
        /// <summary>
		/// 上次玩法规则 少数
		/// </summary>
		public UILabel NumUpRuleInfo;

        public GameObject DownFullObj;
        public GameObject DownNumObj;

        /// <summary>
        /// 麻将数据
        /// </summary>
        private MahjongPlay mMahjonPlay;
        /// <summary>
        /// 当前选择玩法数据
        /// </summary>
        private SelectedPlay mSelectedData;

        void Awake()
        {
            UIEventListener.Get(FullBtnQuickRoom).onClick = BtnQuickRoomClick;
            UIEventListener.Get(NumBtnQuickRoom).onClick = BtnQuickRoomClick;
        }
        public void InitDown(UICreateRoomModel _model)
        {
            Model = _model;
            mSelectedData = MemoryData.MahjongPlayData.GetPlayID();
            if (mSelectedData != null)
            {
                MahjongPlay mUpmMahjonPlay = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(mSelectedData.configID);
                if (mUpmMahjonPlay != null)
                {
                    mMahjonPlay = mUpmMahjonPlay;
                    UpPlayDate();
                }
                else
                {
                    mMahjonPlay = MemoryData.MahjongPlayData.FashionPlayList[0];
                    RecomPlayDate();
                }
            }
            else
            {
                mMahjonPlay = MemoryData.MahjongPlayData.FashionPlayList[0];
                RecomPlayDate();
            }
        }

        // 上次玩法
        void UpPlayDate()
        {
            string[] strArr = MemoryData.MahjongPlayData.GetMahjongPlayOptionStr(mSelectedData.configID, mSelectedData.SelectedItemList, 30, 10, false);
            string str = "";
            for (int i = 0; i < strArr.Length; i++)
            {
                if (string.IsNullOrEmpty(strArr[i]))
                    break;
                str += strArr[i] + '\n';
            }
            if (Model.IsFull())
            {
                DownFullObj.SetActive(true);
                DownNumObj.SetActive(false);
                UpPlayTitle.text = "上一次玩法";
                FullUplayName.text = mMahjonPlay.Name;
                FullUpRuleInfo.text = str;
            }else
            {
                DownFullObj.SetActive(false);
                DownNumObj.SetActive(true);
                NumUplayName.text = mMahjonPlay.Name;
                NumUpRuleInfo.text = str;
            }
            
        }
        // 推荐玩法
        void RecomPlayDate()
        {
            mSelectedData = MemoryData.MahjongPlayData.GetSelected(mMahjonPlay);
            if (mSelectedData != null)
            {
                string[] strArr = MemoryData.MahjongPlayData.GetMahjongPlayOptionStr(mMahjonPlay.ConfigId, mSelectedData.SelectedItemList, 30, 10, false);
                string str = "";
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (string.IsNullOrEmpty(strArr[i]))
                        break;
                    str += strArr[i] + '\n';
                }
                if (Model.IsFull())
                {
                    DownFullObj.SetActive(true);
                    DownNumObj.SetActive(false);
                    UpPlayTitle.text = "最新推荐玩法";
                    FullUplayName.text = mMahjonPlay.Name;
                    FullUpRuleInfo.text = str;
                }
                else
                {
                    DownFullObj.SetActive(false);
                    DownNumObj.SetActive(true);
                    NumUplayName.text = mMahjonPlay.Name;
                    NumUpRuleInfo.text = str;
                }
            }
            else
            {
                DownFullObj.SetActive(false);
                DownNumObj.SetActive(false);
            }
        }

        /// <summary>
		/// 快速开桌按钮回调
		/// </summary>
		private void RuleQuickCallBack()
        {
            //QLoger.ERROR ("快速开桌按钮点击");
            if (mSelectedData != null)
            {
                //mSelectedData.ViewScore = this.ToggleConsentLook.value;

                MemoryData.MahjongPlayData.SavePlayID(mSelectedData.configID);
                //var play = MemoryData.MahjongPlayData.GetMahjongPlayByConfigId(mSelectedData.configID);
                //play.OptionLogic.ImportSelectedList(mSelectedData.SelectedList);

                //MemoryData.MahjongPlayData.SavePlaySet(mSelectedData.configID, mSelectedData);

                //创建房间

                //List<int> rulerList = new List<int>();
                //for (int i = 0; i < mSelectedData.SelectedList.Count; i++)
                //{
                //    QLoger.LOG(mSelectedData.SelectedList[i]);
                //    rulerList.Add(mSelectedData.SelectedList[i]);
                //}

                //MjDeskInfo createDesk = new MjDeskInfo(mMahjonPlay.ConfigId, 0, 0, mMahjonPlay.PlayId, rulerList, 0, 0, mSelectedData.ViewScore, "");
                MjDeskInfo createDesk = new MjDeskInfo(mMahjonPlay, 0);
                createDesk.MjType = mMahjonPlay.MjType;
                MemoryData.DeskData.TempDeskData = createDesk;

                _R.ui.GetUI("UICreateRoom").Close();
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenPrepareGame);
            }
        }
        /// <summary>
		/// 快速开桌按钮点击
		/// </summary>
		private void BtnQuickRoomClick(GameObject go)
        {
            RuleQuickCallBack();
        }
    }
}
