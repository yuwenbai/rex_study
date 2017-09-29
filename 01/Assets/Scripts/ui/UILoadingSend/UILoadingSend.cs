/**
 * @Author lyb
 * 发送消息Loading条关闭
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UILoadingSend : MonoBehaviour
    {
        void OnEnable()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.LoadingSendClose_Event, C2CLoadingSend_Close);            
        }

        void OnDisable()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.LoadingSendClose_Event, C2CLoadingSend_Close);
        }

        void C2CLoadingSend_Close(object[] data)
        {
            GameObject.Destroy(gameObject);
        }
    }
}