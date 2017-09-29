/**
 * @Author Xin.Wang
 * 牌口管理
 *
 */

using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;

namespace projectQ
{
    public partial class MahongDataModel
    {
        public partial class GameProcessData
        {
            public MahjongPlayOpratePaikou processPaikouData
            {
                get
                {
                    string key = MjPlayOprateType.OPRATE_PAIKOU.ToString();
                    if (!playOprateDic.ContainsKey(key))
                    {
                        MahjongPlayOpratePaikou item = new MahjongPlayOpratePaikou();
                        playOprateDic.Add(key, item);
                    }

                    return playOprateDic[key] as MahjongPlayOpratePaikou;
                }

            }
        }
    }

    public partial class MjPlayOprateType
    {
        public const string OPRATE_PAIKOU = "OPRATE_PAIKOU";
    }


    public partial class MjDataManager
    {
        public void SetPaikouShowData(int rulerID, int rulerType)
        {
            MjData.ProcessData.processPaikouData.SetPaikouShowData(rulerID, rulerType);
        }

        public string checkPaikouShowData(int rulerID)
        {
            return MjData.ProcessData.processPaikouData.GetPaikouShow(rulerID);
        }


        public EnumMjOpAction GetPaikouActionType(int rulerID, EnumMjOpAction originalType)
        {
            return MjData.ProcessData.processPaikouData.GetPaikouShowAction(rulerID, originalType);
        }


        public string GetPakouIconName(EnumMjOpAction action)
        {
            string spriteName = null;
            switch (action)
            {
                case EnumMjOpAction.MjOp_Ting:
                    {
                        spriteName = "desk_icon_mingpai";
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglou:
                    {
                        spriteName = "desk_icon_minglou";
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglao:
                    {
                        spriteName = "desk_icon_minglao";
                    }
                    break;
                case EnumMjOpAction.MjOp_Minglv:
                    {
                        spriteName = "desk_icon_minglv";
                    }
                    break;
                case EnumMjOpAction.MjOp_MinglouMu:
                    {
                        spriteName = "desk_icon_mingloumu";
                    }
                    break;
                case EnumMjOpAction.MjOp_AnlouMu:
                    {
                        spriteName = "desk_icon_anloumu";
                    }
                    break;
                case EnumMjOpAction.MjOp_AnlouShou:
                    {
                        spriteName = "desk_icon_anlou";
                    }
                    break;
            }

            return spriteName;
        }

    }

}


namespace MahjongPlayType
{
    public class PaikouShowData
    {
        public int rulerID;
        public EnumPaikouShow rulerType;

        public PaikouShowData(int rulerID, int rulerTypeID)
        {
            this.rulerID = rulerID;
            this.rulerType = (EnumPaikouShow)rulerTypeID;
        }
    }


    public enum EnumPaikouShow
    {
        MjRulerChangeType_PICI = 1,			    //皮次
        MjRulerChangeType_MINGLAO = 2,			//明捞
        MjRulerChangeType_MINGLOUMU = 3,          //明楼（木
        MjRulerChangeType_ANLOUSHOU = 4,          //暗搂（手
        MjRulerChangeType_ANLOUMU = 5,          //暗楼（木
        MjRulerChangeType_MINGLV = 6,           //明缕
    }


    public class MahjongPlayOpratePaikou : MahjongPlayOprateBase
    {
        private Dictionary<int, PaikouShowData> _paikouShowDic = new Dictionary<int, PaikouShowData>();

        public void SetPaikouShowData(int rulerID, int rulerType)
        {
            PaikouShowData item = new PaikouShowData(rulerID, rulerType);
            SetPaikouShowDataItem(item);
        }

        public void SetPaikouShowData(List<PaikouShowData> showData)
        {
            if (showData != null && showData.Count > 0)
            {
                for (int i = 0; i < showData.Count; i++)
                {
                    SetPaikouShowDataItem(showData[i]);
                }
            }
        }

        private void SetPaikouShowDataItem(PaikouShowData item)
        {
            int curID = item.rulerID;
            if (CheckPaikouShowContain(curID))
            {
                _paikouShowDic[curID] = item;
            }
            else
            {
                _paikouShowDic.Add(curID, item);
            }
        }



        /// <summary>
        /// 检查是否有该牌口的特殊现实存在
        /// </summary>
        /// <param name="rulerID"></param>
        /// <returns></returns>
        public bool CheckPaikouShowContain(int rulerID)
        {
            return _paikouShowDic.ContainsKey(rulerID);
        }

        /// <summary>
        /// 获取牌口显示
        /// </summary>
        /// <param name="rulerID"></param>
        /// <returns></returns>
        public string GetPaikouShow(int rulerID)
        {
            if (!CheckPaikouShowContain(rulerID))
            {
                return null;
            }
            PaikouShowData data = _paikouShowDic[rulerID];

            return CheckShowRuler(data.rulerType);
        }

        public EnumMjOpAction GetPaikouShowAction(int ruler, EnumMjOpAction originalAction)
        {
            if (!CheckPaikouShowContain(ruler))
            {
                return originalAction;
            }

            PaikouShowData data = _paikouShowDic[ruler];

            return CheckShowRulerAction(data.rulerType);
        }

        private EnumMjOpAction CheckShowRulerAction(EnumPaikouShow rulerType)
        {
            switch (rulerType)
            {
                case EnumPaikouShow.MjRulerChangeType_PICI:
                    {
                        return EnumMjOpAction.MjOp_PiCi;
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_MINGLAO:
                    {
                        return EnumMjOpAction.MjOp_Minglao;
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_MINGLV:
                    {
                        return EnumMjOpAction.MjOp_Minglv;
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_MINGLOUMU:
                    {
                        return EnumMjOpAction.MjOp_MinglouMu;
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_ANLOUMU:
                    {
                        return EnumMjOpAction.MjOp_AnlouMu;
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_ANLOUSHOU:
                    {
                        return EnumMjOpAction.MjOp_AnlouShou;
                    }
                    break;
            }

            return EnumMjOpAction.Null;
        }



        private string CheckShowRuler(EnumPaikouShow rulerType)
        {
            string prefabName = null;
            switch (rulerType)
            {
                case EnumPaikouShow.MjRulerChangeType_PICI:
                    {
                        //皮次
                        prefabName = "pici";
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_MINGLAO:
                    {
                        prefabName = "Minglao";
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_MINGLV:
                    {
                        prefabName = "minglv";
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_MINGLOUMU:
                    {
                        prefabName = "MingMulou";
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_ANLOUMU:
                    {
                        prefabName = "anmulou";
                    }
                    break;
                case EnumPaikouShow.MjRulerChangeType_ANLOUSHOU:
                    {
                        prefabName = "anlou";
                    }
                    break;
            }

            return prefabName;
        }


    }
}

