/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ;
namespace projectQ
{
    public partial class MahjongUIManager
    {
        private UIPlayingTips _BankerAlert;
        public void RegisterModuleEvent_BankerAlertUIModule()
        {
            _MahjongEventHelper.AddEvent(MJEnum.BankerPutAlert.BPA_OpenUIAlert.ToString(), OpenUIAlert);
            _MahjongEventHelper.AddEvent(MJEnum.BankerPutAlert.BPA_CloseUIAlert.ToString(), CloseUIAlert);
        }
        private void OpenUIAlert(object[] param)
        {
            if (!NullHelper.IsObjectIsNull(_BankerAlert))
            {
                CloseUIAlert(param);
            }
            _BankerAlert = LoadUISelf<UIPlayingTips>(PrefabPathDefine.UIPlayingBankerTips, new StructUIAnchor(), false);
            if (NullHelper.IsObjectIsNull(_BankerAlert))
            {
                return;
            }
            _BankerAlert.RefreshUI(UIConstStringDefine.BankerFirstAlert, UIPlayingTipsType.OnlyTips, Vector2.zero);
        }
        private void CloseUIAlert(object[] param)
        {
            if (NullHelper.IsObjectIsNull(_BankerAlert))
            {
                return;
            }
            CloseUISelf<UIPlayingTips>(_BankerAlert);
        }
    }
}
public class BankerAlertUIModule : SpecialUIModule
{
    public override void AddEvents()
    {

    }


    public override void ReconnectedPreparedUIManager()
    {
        EventDispatcher.FireEvent(MJEnum.BankerPutAlert.BPA_BankerFirstTurn.ToString());
    }
}
