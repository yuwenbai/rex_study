/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MjCreateFactory
{
    public class BaseModuleUIManagerCreateFactory
    {
        public static void CreateBaseModuleUIManager(Dictionary<System.Type, BaseUIModule> dic)
        {
            if (NullHelper.IsObjectIsNull(dic))
            { return; }

            dic.Add(typeof(MjProcessBasicView), new MjProcessBasicView());

        }
    }
}
