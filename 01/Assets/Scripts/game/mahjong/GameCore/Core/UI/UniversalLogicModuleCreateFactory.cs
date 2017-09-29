/**
 * @Author JEFF
 * 有顺序追加模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalLogicModuleCreateFactory
{

    /// <summary>
    /// 通用玩法逻辑模块类
    /// </summary>
    /// <param name="dic"></param>
    public static void CreateUniversalLogicModule(Dictionary<System.Type, BaseLogicModule> dic)
    {
        if (NullHelper.IsObjectIsNull(dic))
        { return; }

        dic.Add(typeof(OpenSpecialLogicModule), new OpenSpecialLogicModule());                    //翻开定混
    }
}
