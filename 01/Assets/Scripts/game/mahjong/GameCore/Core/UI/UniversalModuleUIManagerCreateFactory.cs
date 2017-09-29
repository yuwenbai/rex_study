/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;

namespace MjCreateFactory
{
    /// <summary>
    /// 通用游戏UI管理模块
    /// </summary>
    public class UniversalModuleUIManagerCreateFactory
    {
        public static void CreateUniversalGameUIModules(Dictionary<System.Type, BaseUIModule> UIModules)
        {
            if (NullHelper.IsObjectIsNull(UIModules))
            { return; }
            //类似增加其他模块
        }
    }
}
