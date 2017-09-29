/**
 * @Author JEFF
 *
 *
 */

using projectQ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MjCreateFactory
{
    /// <summary>
    /// 特殊玩法
    /// </summary>
    public class SpecialModuleUIManagerCreateFactory
    {
        public static void CreateSpecialGameUIModules(Dictionary<System.Type, BaseUIModule> UIModules)
        {
            if (NullHelper.IsObjectIsNull(UIModules))
            { return; }
            UIModules.Add(typeof(DuAnGangUIModule), new DuAnGangUIModule());
            UIModules.Add(typeof(GangHouNaChiUIModule), new GangHouNaChiUIModule());
            UIModules.Add(typeof(BankerAlertUIModule), new BankerAlertUIModule());
            //类似增加其他模块
            UIModules.Add(typeof(LiangYiUIModule), new LiangYiUIModule());
            UIModules.Add(typeof(MingDaUIModule), new MingDaUIModule());
            UIModules.Add(typeof(NiuPaiUIModule), new NiuPaiUIModule());
            UIModules.Add(typeof(HuPiaoGenPiaoUIModule), new HuPiaoGenPiaoUIModule());
            UIModules.Add(typeof(LiangSiDaYiUIModule), new LiangSiDaYiUIModule());
            UIModules.Add(typeof(LiangGangTouUIModule), new LiangGangTouUIModule());
            UIModules.Add(typeof(DuanMenUIModule), new DuanMenUIModule());
            UIModules.Add(typeof(JiangMaUIModule), new JiangMaUIModule());
            UIModules.Add(typeof(SiJiaMaiMaUIModule), new SiJiaMaiMaUIModule());
            UIModules.Add(typeof(FengQuanUIModule), new FengQuanUIModule());
        }

    }
}
