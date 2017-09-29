/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UILoadingModel : UIModelBase {
        private UILoading UI { get { return _ui as UILoading; } }

        private List<string> OpenList = new List<string>();

        private void PushKey(string key,bool isEmpty = false)
        {
            if(OpenList.IndexOf(key) == -1)
                OpenList.Add(key);
            if(isEmpty)
            {
                UI.ShowLoadingEmpty();
            }
            else
            {
                UI.ShowLoading();
                //UI.ShowLoadingEmpty(alpha);
            }
        }

        private void PopKey(string key)
        {
            if(OpenList.IndexOf(key) != -1)
            {
                OpenList.Remove(key);
            }
            if(OpenList.Count == 0)
            {
                UI.DelayHideLoading();
            }
        }
        
        private void Kill()
        {
            OpenList.Clear();
            UI.HideLoading();
        }

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                 GEnum.NamedEvent.SysUI_SmallLoading_Open,
                 GEnum.NamedEvent.SysUI_SmallLoading_Close,
                 GEnum.NamedEvent.SysUI_SmallLoading_Kill,
                 GEnum.NamedEvent.SysUI_SmallLoading_SetContent,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.SysUI_SmallLoading_Open:
                    if(data.Length >= 2)
                    {
                        QLoger.LOG("小Loading开启 透明" + data[0] as string);
                        PushKey(data[0] as string,(bool)data[1]);
                    }
                    else
                    {
                        QLoger.LOG("小Loading开启 正常" + data[0] as string);
                        PushKey(data[0] as string);
                    }
                    break;
                case GEnum.NamedEvent.SysUI_SmallLoading_Close:
                    QLoger.LOG("小Loading关闭" + data[0] as string);
                    PopKey(data[0] as string);
                    break;
                case GEnum.NamedEvent.SysUI_SmallLoading_Kill:
                    QLoger.LOG("小LoadingKill");
                    Kill();
                    break;
                case GEnum.NamedEvent.SysUI_SmallLoading_SetContent:
                    UI.SetLabelContent(data[0] as string);
                    break;
            }
        }
        #endregion
    }
}
