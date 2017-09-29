/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;

namespace MjCreateFactory
{
    public class SpecialLogicModuleCreateFactory
    {
        /// <summary>
        /// 特殊玩法逻辑模块类
        /// </summary>
        /// <param name="dic"></param>
        public static void CreateSpcialLogicModule(Dictionary<System.Type, BaseLogicModule> dic)
        {
            if (NullHelper.IsObjectIsNull(dic))
            { return; }
            dic.Add(typeof(GangHouNaChiLogicModule), new GangHouNaChiLogicModule());
            dic.Add(typeof(BankerLogicModule), new BankerLogicModule());
            dic.Add(typeof(DuAnGangLogicModule), new DuAnGangLogicModule());
            dic.Add(typeof(MingDaLogicModule), new MingDaLogicModule());
            dic.Add(typeof(LiangyiLogicModule), new LiangyiLogicModule());
            dic.Add(typeof(NiuPaiLogicModule), new NiuPaiLogicModule());
            dic.Add(typeof(HuPiaoGenPiaoLogicModule), new HuPiaoGenPiaoLogicModule());
            dic.Add(typeof(LiangSiDaYiLogicModule), new LiangSiDaYiLogicModule());
            dic.Add(typeof(LiangGangTouLogicModule), new LiangGangTouLogicModule());
            dic.Add(typeof(DuanMenLogicModule), new DuanMenLogicModule());
            dic.Add(typeof(SiJiaMaiMaLogicModule), new SiJiaMaiMaLogicModule());
            dic.Add(typeof(FengQuanLogicModule), new FengQuanLogicModule());
            dic.Add(typeof(JiangMaLogicModule), new JiangMaLogicModule());
        }
    }
}

