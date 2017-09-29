/**
 * @Author Hailong.Zhang
 *归类为广东地区的麻将玩法网络接收
 *
 */

using System.Collections.Generic;
using Msg;


namespace projectQ
{

    public partial class ModelNetWorker
    {
        public void initDefaultHandleOfMahjongGD()
        {
            #region 四家买马
            ModelNetWorker.Regiest<MjSiJiaMaiMaNotify>(SiJiaMaiMaNotify);
            #endregion
            #region 风圈标识
            ModelNetWorker.Regiest<MjFengQuanNotify>(FengQuanNotify);
            #endregion

        }

        #region 四家买马
        private void SiJiaMaiMaNotify(object rsp)
        {
            if (rsp != null)
            {
                MjSiJiaMaiMaNotify prsp = rsp as MjSiJiaMaiMaNotify;
                ParserSiJiaMaiMaData(prsp, true);
            }
        }

        private void ParserSiJiaMaiMaData(MjSiJiaMaiMaNotify prsp, bool isNormalProcess)
        {
            if (prsp != null)
            {
                MahjongPlayType.SiJiaMaiMaData siJiaMaiMaData = new MahjongPlayType.SiJiaMaiMaData(prsp);
                if (prsp.DataCell != null && prsp.DataCell.Count > 0)
                {
                    for (int i = 0; i < prsp.DataCell.Count; i++)
                    {
                        MahjongPlayType.SiJiaMaiMaCellData cellData = new MahjongPlayType.SiJiaMaiMaCellData(prsp.DataCell[i].ComnonValue, prsp.DataCell[i].DataConfirm);
                        siJiaMaiMaData.sjmm_subData.Add(cellData);
                    }
                }
                MjDataManager.Instance.MjData.ProcessData.processSiJiaMaiMa.SiJiaMaiMaData = siJiaMaiMaData;
                if (isNormalProcess)
                {
                    MjDataManager.Instance.InitSiJiaMaiMaData();
                }
            }
        }
        #endregion

        #region 风圈标识
        private void FengQuanNotify(object rsp)
        {
            if (rsp != null)
            {
                MjFengQuanNotify prsp = rsp as MjFengQuanNotify;
                ParserFengQuanData(prsp,true);
            }
        }

        private void ParserFengQuanData(MjFengQuanNotify prsp,bool isNormalProcess)
        {
            if (prsp != null)
            {
                int deskID = prsp.DeskID;
                int identify = (int)prsp.FengQuan;
                MahjongPlayType.FengQuanData data = new MahjongPlayType.FengQuanData(deskID, identify);
                MjDataManager.Instance.MjData.ProcessData.processFengQuan.FengQuanData = data;
                if(isNormalProcess)
                {
                    MjDataManager.Instance.InitFengQuanData();
                }
            }
        }
        #endregion


    }
}
