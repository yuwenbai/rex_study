/**
 * @Author HaiLong.Zhang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
using MahjongPlayType;
namespace projectQ
{
    public partial class MahjongUIManager
    {
        private UISiJiaMaiMa _SiJiaMaiMa;
        public void RegisterModuleEvent_SiJiaMaiMaUIModule()
        {
            //重连事件机制
            _MahjongEventHelper.AddEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_FirstShowUI.ToString(), ReconnectFirstShowUISiJaiMaiMa);
            _MahjongEventHelper.AddEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_KaiMaResultAnim.ToString(), LoadAnimKaiMa);
            //正常动画压栈事件机制
            _AnimEventHelper.AddEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_FirstShowUI.ToString(), LoadFirstShowUISiJaiMaiMa);
            _AnimEventHelper.AddEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_KaiMaResultAnim.ToString(), LoadAnimKaiMa);
        }
        private void LoadFirstShowUISiJaiMaiMa(object[] param)
        {
            GetDataSiJiaMaiMa(true, true);
        }
        private void ReconnectFirstShowUISiJaiMaiMa(object[] param)
        {
            GetDataSiJiaMaiMa(true, false);
        }
        private void LoadAnimKaiMa(object[] param)
        {
            GetDataSiJiaMaiMa(false, false);
        }
        private void GetDataSiJiaMaiMa(bool isFirst, bool isReconned)
        {
            SiJiaMaiMaData data = MjDataManager.Instance.MjData.ProcessData.processSiJiaMaiMa.SiJiaMaiMaData;
            if (data.sjmm_subData != null && data.sjmm_subData.Count > 0)
            {
                int selfSeatID = MjDataManager.Instance.MjData.curUserData.selfSeatID;
                int dealerSeatID = MjDataManager.Instance.MjData.ProcessData.dealerSeatID;
                List<int> mjPaiList = new List<int>();
                for (int i = 0; i < data.sjmm_subData.Count; i++)
                {
                    int curSeatID = data.sjmm_subData[i].seatID;
                    if (isFirst)
                    {
                        int uiSeatID = CardHelper.GetMJUIPosByServerPos(curSeatID, selfSeatID);
                        mjPaiList = data.sjmm_subData[i].mjCode;
                        SetSiJiaMaiMaDeskShow(uiSeatID, mjPaiList);
                        if (isReconned)
                        {
                            _ui3D.SetSiJiaMaiMa(mjPaiList);
                        }
                    }
                    else
                    {
                        long userID = MjDataManager.Instance.GetCurDeskPlayerInfoBySeatID(curSeatID).userID;
                        string headUrl = MemoryData.PlayerData.get(userID).PlayerDataBase.HeadURL;
                        data.sjmm_subData[i].headUrl = headUrl;
                    }
                }

                if (!isFirst)
                {
                    _ui2D.ClearGameInfo();
                    _SiJiaMaiMa = LoadUISelf<UISiJiaMaiMa>(GameConst.path_MahjongSiJiaMaiMa, new StructUIAnchor(), false, data.sjmm_subData, selfSeatID, dealerSeatID, ConstDefine.Mj_Anim2d_SiJiaMaiMa[0], ConstDefine.Mj_Anim2d_SiJiaMaiMa[1]);
                }
            }
        }


        private void SetSiJiaMaiMaDeskShow(int uiSeatID, List<int> mjPaiList)
        {
            if (_desk != null)
            {
                _desk.ShowSiJiaMaiMaFirstPai(uiSeatID, mjPaiList);
            }
        }
    }

    public class SiJiaMaiMaUIModule : SpecialUIModule
    {
        public override void AddEvents()
        {

        }

        public override void ReconnectedPreparedUIManager()
        {
            //判断数据层的数据是否存在
            bool containMaiMa = MjDataManager.Instance.CheckPlayDataOprateContain("OPRATE_SiJiaMaiMa");
            if (containMaiMa)
            {
                SiJiaMaiMaData data = MjDataManager.Instance.MjData.ProcessData.processSiJiaMaiMa.SiJiaMaiMaData;
                if (data.selectType == 1)
                {
                    EventDispatcher.FireEvent(MJEnum.MjSiJiaMaiMaEvent.MJSJMM_FirstShowUI.ToString());
                }
            }
        }
    }
}
