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
    public class UIMainStateBase
    {
        private UIMain ui;
        public virtual UIMain.EnumUIMainState SelfState{
            get{ return UIMain.EnumUIMainState.None; }    
        }
        public UIMain UI
        {
            get { return ui; }
        }
        public static void ChangeState(UIMainStateBase script,UIMain ui)
        {
            QLoger.LOG("切换到" + script.SelfState.ToString());
            ui.StateScript = script;
            script.Init(ui);
            script.RefreshUI();
        }

        public virtual void Init(UIMain ui)
        {
            this.ui = ui;
        }

        public virtual bool SetData(GEnum.NamedEvent gEnum, object[] data)
        {
            return false;
        }

        public virtual void RefreshUI()
        {
            QLoger.LOG(this.SelfState + "RefreshUI");
        }
    }
}