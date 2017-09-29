/**
 * @Author GarFey
 *  麻将开房模块
 */

using UnityEngine;

namespace projectQ
{

	public class UICreateRoom : UIViewBase 
	{
		public UICreateRoomModel Model
		{
			get { return _model as UICreateRoomModel; }
		}
		/// <summary>
		/// The button back.关闭面板按钮
		/// </summary>
		public GameObject BtnBack;
		
        /// <summary>
        /// 流行
        /// </summary>
        public CreateRoomUp CreatUp;
        /// <summary>
        /// 地区
        /// </summary>
        public CreateRoomBottom CreatBottom;
        /// <summary>
        /// 上次玩法
        /// </summary>
        public CreateRoomDown CreatDown;
        /// <summary>
        /// 排布全部背景
        /// </summary>
        public GameObject BgFull;
        /// <summary>
        /// 排布少数背景
        /// </summary>
        public GameObject BgNum;

        /// <summary>
        /// 打开面板，传递参数，先调用该方法
        /// </summary>
        public override void OnPushData(object[] data)
		{

		}
		public override void Init()
		{
			UIEventListener.Get(BtnBack).onClick = CloseBtnClick;
        }

		public override void OnHide() { }

		public override void OnShow()
		{
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.RegionID <= 0)
            {
                this.LoadUIMain("UIMap");
            }
            else
            {
                Model.RulePlayTabBtnCreat();
    			Model.RefUpPlayData ();
            }
		}
        public override void GoBack()
        {
            this.CloseBtnClick(null);
        }
        /// <summary>
        /// 返回按钮
        /// </summary>
        private void CloseBtnClick(GameObject go)
		{
			EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_OpenMain, EnumChangeSceneType.GamePrepare_To_Main);
			this.Close();
		}
	}
}
