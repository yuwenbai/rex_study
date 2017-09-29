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
    public class UIOptionModel : UIModelBase
    {
        public UIOption UI
        {
            get{ return this._ui as UIOption; }
        }

        #region override------------------------------------------------------------------

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {};
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
        }

        #endregion------------------------------------------------------------------------

        #region 设置按钮初始化------------------------------------------------------------

        /// <summary>
        /// 设置面板初始化
        /// </summary>
        public void OptionInit()
        {
            //1 未关联 2 关联 3 解除关联中
            int bindRoomState = MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.BindRoomState;

            if (bindRoomState == 1)
            {
                //UI.VersionStrLab.gameObject.SetActive(true);
                UI.RelieveBtn.SetActive(false);
            }
            else if (bindRoomState == 2)
            {
                UI.RelieveBtn.SetActive(true);
                UI.RelieveBtn.GetComponent<BoxCollider>().enabled = true;
                //UI.VersionStrLab.gameObject.SetActive(false);
            }
            else
            {
                UI.RelieveBtn.SetActive(true);
                UI.RelieveBtn.GetComponent<UIDefinedButton>().isEnabled = false;
                //UI.VersionStrLab.gameObject.SetActive(false);
            }
        }

        #endregion------------------------------------------------------------------------

        #region 设置按钮初始化------------------------------------------------------------

        public void OptionSingleInit()
        {
            UI.VersionStrLab.text = "当前版本号 " + "v" + Application.version;
            
            foreach (UIOptionList setList in UI.OptionSingleList)
            {
                setList.OptionList_Init();
                setList.OnClickCallBack = OnOptionListClickCallBack;
            }
        }

        /// <summary>
        /// 设置按钮点击回调
        /// </summary>
        void OnOptionListClickCallBack(int optionId)
        {
            UI.OptionSingleList[optionId - 1].OptionList_SetValue();
        }

        #endregion------------------------------------------------------------------------
    }
}
