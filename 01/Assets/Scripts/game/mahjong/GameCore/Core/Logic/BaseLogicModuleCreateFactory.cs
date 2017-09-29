/**
 * @Author JEFF
 * 基本流程模块
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLogicModuleCreateFactory
{
    /// <summary>
    /// 基本玩法逻辑模块类
    /// </summary>
    /// <param name="dic"></param>
    public static void CreateBaseLogicModule(Dictionary<System.Type, BaseLogicModule> dic)
    {
        if (NullHelper.IsObjectIsNull(dic))
        { return; }

        dic.Add(typeof(MjProcessBasicLogic), new MjProcessBasicLogic());                    //基本操作流程
    }
}
