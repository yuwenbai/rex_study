/**
 * @Author lyb
 * 大厅牌匾的逻辑控制脚本
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMainTopHallTitle : UIMainBase
    {
        /// <summary>
        /// 牌匾Tex
        /// </summary>
        public UITexture Tex_Board;
        /// <summary>
        /// 麻将馆名字
        /// </summary>
        public UILabel HallRoomName;
        /// <summary>
        /// 麻将馆Id
        /// </summary>
        public UILabel HallRoomId;
        /// <summary>
        /// 装扮按钮
        /// </summary>
        public GameObject BoardChangeBtn;
        /// <summary>
        /// 宣言滚动条
        /// </summary>
        public NGUIBroadcasts Advert;
        /// <summary>
        /// 地区名字
        /// </summary>
        public UILabel RegionLab;
        /// <summary>
        /// 切换区域
        /// </summary>
        public GameObject RegionSwitchBtn;
        /// <summary>
        /// 装修按钮特效
        /// </summary>
        public GameObject Effect_BoardChange;
        /// <summary>
        /// 牌匾特效
        /// </summary>
        public GameObject[] EffectBoardArray;

        private MjRoom mjHall;

        private void Awake()
        {
            UIEventListener.Get(BoardChangeBtn).onClick = OnButtonChangeClick;
            UIEventListener.Get(RegionSwitchBtn).onClick = OnRegionSwitchBtnClick;
        }

        void Start(){}

        public void HallTitleInit(UIMain.EnumUIMainState state , MjRoom mjHall)
        {
            this.mjHall = mjHall;
            MemoryData.OtherData.SetCurrBoard(mjHall.BoardID);

            //切换按钮
            BoardChangeBtn.SetActive(state == UIMain.EnumUIMainState.Master);

            //更新牌匾
            UpdateBoard();

            //更新地区
            UpdateRegion();

            HallRoomName.text = mjHall.RoomName;
            HallRoomId.text = "ID:" + mjHall.RoomID.ToString();

            Advert.Clear();
            var adData = new NGUIBroadcastsData(mjHall.AdText, 2);
            adData.LoopCount = 9999;
            Advert.Push(adData);
        }

        /// <summary>
        /// 更新牌匾
        /// </summary>
        public void UpdateBoard()
        {
            var boardData = MemoryData.OtherData.GetBoard(this.mjHall.BoardID);

            if (boardData.BoardTexture == null)
            {
                boardData.BoardTexture = ResourcesDataLoader.Load<Texture>(boardData.GetPath());
            }

            Tex_Board.mainTexture = boardData.BoardTexture;
        }

        /// <summary>
        /// 更新地区
        /// </summary>
        public void UpdateRegion()
        {
            string regionName = MemoryData.XmlData.XmlBuildDataRegion_Get();
            if (regionName != "")
            {
                string str = regionName.Substring(0, 2);
                RegionLab.text = string.Format("{0}专区", str);
            }
            else
            {
                RegionLab.text = "请选择地方玩法";
            }
        }

        #region ButtonEvent ------------------------------------------------

        /// <summary>
        /// 装饰按钮点击事件响应
        /// </summary>
        private void OnButtonChangeClick(GameObject go)
        {
            MemoryData.OtherData.ChangeNextBoar();
            ModelNetWorker.Instance.FMjRoomChangeBoardReq(MemoryData.OtherData.GetCurrBoard().BoardKey);
            UpdateBoard();

            Effect_BoardChange.SetActive(false);
            Effect_BoardChange.SetActive(true);

            StartCoroutine(UITools.WaitExcution(EffectBoardPlay, 0.1f));
        }

        void EffectBoardPlay()
        {
            int boardKey = int.Parse(MemoryData.OtherData.GetCurrBoard().BoardKey);
            EffectBoardArray[boardKey - 1].SetActive(false);
            EffectBoardArray[boardKey - 1].SetActive(true);
        }

        /// <summary>
        /// 切换区域按钮点击事件响应
        /// </summary>
        private void OnRegionSwitchBtnClick(GameObject go)
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_UIMap_Open);
        }

        #endregion ---------------------------------------------------------
    }
}
