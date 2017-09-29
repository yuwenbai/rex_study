

using System;
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
    public class UIParlorInfo : UIViewBase
    {
        private UIParlorInfoModel Model
        {
            get { return _model as UIParlorInfoModel; }
        }
        private List<SelectRegion> cityTableDataList = new List<SelectRegion>();
        public UILabel LabelTitle;
        public UILabel LabelID;
        public UILabel LabelInfoDate;
        public UILabel LabelInfoContent;

        //关联所需要的=============================
        public GameObject GoLink;
        public UILabel LabelNum;
        public UILabel LabelCity;
        public UILabel LabelMaster_Link;
        public GameObject ButtonLink;

        //查看所需要的=============================
        public GameObject GoCheck;
        public UILabel LabelMaster_Check;
        public UILabel LabelPhoneNo;

        public GameObject ButtonClose;

        //是否查看
        private bool isCheck = false;
        //关闭后的回调
        public Action<object> HideCallBack;
        private object HideCallBackData;

        private int m_RoomID;

        public void RefreshUI()
        {
            System.Text.StringBuilder tempStr = new System.Text.StringBuilder();

            tempStr.Length = 0;
            tempStr.Append(Model.data.RoomName);//.Append("棋牌室");
            //麻将馆名字
            LabelTitle.text = tempStr.ToString();

            //ID
            tempStr.Length = 0;
            tempStr.Append("ID:").Append(Model.data.RoomID);
            LabelID.text = "";
            tempStr.ToString();

            //创建时间 
            tempStr.Length = 0;

            //TODO-周腾，这里添加保护，当没有数据的时候默认显示为空，不能越界
            string createTime = "";
            if (!string.IsNullOrEmpty(Model.data.CreateTime))
            {
                createTime = Model.data.CreateTime.Substring(0, Model.data.CreateTime.Length - 9);
            }

            tempStr.Append("创建于").Append(createTime);
            LabelInfoDate.text = tempStr.ToString();

            //说明
            LabelInfoContent.text = Model.data.AdText;

            GoCheck.SetActive(isCheck);
            GoLink.SetActive(!isCheck);

            if (isCheck)
            {
                //馆长
                tempStr.Length = 0;
                tempStr.Append("馆长:").Append(Model.data.FounderName);
                LabelMaster_Check.text = tempStr.ToString();

                //邀请码
                tempStr.Length = 0;
                tempStr.Append("馆长ID:").Append(Model.data.AgentID);
                LabelPhoneNo.text = tempStr.ToString();
            }
            else
            {
                //人数
                tempStr.Length = 0;
                tempStr.Append("人员:").Append(Model.data.CurMemberNum)
                    .Append("/").Append(Model.data.MaxMemberNum);
                LabelNum.text = tempStr.ToString();

                //地区
                tempStr.Length = 0;
                tempStr.Append("地区:").Append(GetRegionNameByID((Model.data.RegionID).ToString(), (Model.data.CityID).ToString()));
                LabelCity.text = tempStr.ToString();

                //馆长
                tempStr.Length = 0;
                tempStr.Append("馆长:").Append(Model.data.FounderName);
                LabelMaster_Link.text = tempStr.ToString();
            }

            UITools.SetActive(gameObject);
        }

        /// <summary>
        /// By Update lyb 17-08-05 修改地区显示，有省无市显示省 有市显示市
        /// </summary>
        /// <returns></returns>
        string GetRegionNameByID(string regionID, string cityID)
        {
            string cityName = "";
            List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["SelectRegion"];
            foreach (BaseXmlBuild build in buildList)
            {
                SelectRegion info = (SelectRegion)build;

                if (regionID.Equals(info.RegionID))
                {
                    cityName = info.RegionName;
                }

                if (cityID.Equals(info.CityID))
                {
                    cityName = info.CityName;
                }
            }

            if (string.IsNullOrEmpty(cityName))
            {
                cityName = "未知";
            }
            return cityName;
        }

        #region Event
        private void OnButtonLinkClick(GameObject go)
        {
            Model.OnBindHall();
        }
        private void OnButtonCloseClick(GameObject go)
        {
            MemoryData.GameStateData.IsWxShareInvitePlay = false;
            this.Hide();
        }
        #endregion

        #region override
        public override void Init()
        {
            UIEventListener.Get(ButtonLink).onClick = OnButtonLinkClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
        }

        public override void OnHide()
        {
            if (HideCallBack != null)
                HideCallBack(HideCallBackData);
        }

        public override void OnShow()
        {
            this.RefreshUI();
        }
        public override void OnPushData(object[] data)
        {
            isCheck = false;
            HideCallBack = null;
            HideCallBackData = null;
            if (data != null)
            {
                if (data[0] is string)
                {
                    if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.IsHaveMjRoom())
                    {
                        Close();
                        return;
                    }

                    m_RoomID = int.Parse(data[0] as string);

                    if (MemoryData.MjHallData.MjHallList != null)
                    {
                        for (int i = 0; i < MemoryData.MjHallData.MjHallList.Count; i++)
                        {
                            if (MemoryData.MjHallData.MjHallList[i].RoomID == m_RoomID)
                            {
                                Model.data = MemoryData.MjHallData.MjHallList[i] as MjRoom;

                                RefreshUI();
                                m_RoomID = 0;
                                return;
                            }
                        }
                    }

                    this.gameObject.SetActive(false);

                    EventDispatcher.AddEvent(GEnum.NamedEvent.SysData_MjJall_SearchOver, OnSearchOver);
                    EventDispatcher.FireSysEvent(GEnum.NamedEvent.EGetGameData, GEnum.NamedGameData.EC2SMessageRoomSearch, m_RoomID.ToString(), 0, Msg.FMjSortType.FMjSort_RenQi);

                    return;
                }

                Model.data = data[0] as MjRoom;
                if (data.Length > 1)
                    isCheck = (bool)data[1];

                if (data.Length > 2)
                    HideCallBack = data[2] as Action<object>;

                if (data.Length > 3)
                    HideCallBackData = data[3];
                RefreshUI();
            }
        }

        void OnSearchOver(object[] values)
        {
            try
            {
                List<MjRoom> list = values[0] as List<MjRoom>;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RoomID == m_RoomID)
                    {
                        Model.data = list[i] as MjRoom;
                        this.gameObject.SetActive(true);
                        RefreshUI();
                        m_RoomID = 0;
                        return;
                    }
                }
            }
            catch
            {

            }

            EventDispatcher.RemoveEvent(GEnum.NamedEvent.SysData_MjJall_SearchOver, OnSearchOver);
        }
        #endregion
    }
}
