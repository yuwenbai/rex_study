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
    public class UIMainTop : UIMainBase
    {
        /// <summary>
        /// 牌匾引用
        /// </summary>
        public UIMainTopHallTitle TopHallTitle;
        /// <summary>
        /// Logo引用
        /// </summary>
        public UIMainTopHallLogo TopHallLogo;
        /// <summary>
        /// 背景图
        /// </summary>
        public UITexture TopBgTex;
        /// <summary>
        /// 特效引用
        /// </summary>
        public GameObject TopEffectObj;
        /// <summary>
        /// 背景的Anchor、
        /// </summary>
        public UIAnchor MainBgAnchorObj;

        private MjRoom mjHall;

        /// <summary>
        /// 麻将馆数据 
        /// </summary>
        public void SetData(MjRoom hall)
        {
            this.mjHall = hall;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshUI(UIMain.EnumUIMainState state)
        {
            string bgTexPath = "";

            if (this.mjHall == null || state == UIMain.EnumUIMainState.NormalMain || state == UIMain.EnumUIMainState.LinkMain)
            {
                TopHallTitle.gameObject.SetActive(false);
                TopHallLogo.gameObject.SetActive(true);
                TopHallLogo.HallLogoInit();

                TopEffectObj.SetActive(true);

                bgTexPath = GameAssetCache.Texture_MainBg_Path;
            }
            else
            {
                TopHallLogo.gameObject.SetActive(false);
                TopHallTitle.gameObject.SetActive(true);
                TopHallTitle.HallTitleInit(state, this.mjHall);

                TopEffectObj.SetActive(false);

                bgTexPath = GameAssetCache.Texture_MainMahjongBg_Path;

                if (state == UIMain.EnumUIMainState.MasterCheck)
                {
                    //bgTexPath = GameAssetCache.Texture_MainBg02_Path;
                }
            }

            MainBgAnchorObj.enabled = true;
            Texture bgTex = ResourcesDataLoader.Load<Texture>(bgTexPath);
            TopBgTex.mainTexture = bgTex;
        }

        /// <summary>
        /// 更新牌匾
        /// </summary>
        public void UpdateBoard()
        {
            TopHallTitle.UpdateBoard();
        }

        /// <summary>
        /// 更新地区
        /// </summary>
        public void UpdateRegion()
        {
            //换区切换该区跑马灯公告
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_Broadcast_Update);
            TopHallTitle.UpdateRegion();
            TopHallLogo.UpdateRegion();
        }
    }
}