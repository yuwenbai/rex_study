/**
* @Author lyb
* 代理专区
*
*/

using UnityEngine;

namespace projectQ
{
    public class UIAgentRequest : UIViewBase
    {
        public GameObject ButtonClose;

        #region override -----------------------------------------------

        public override void Init()
        {
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
        }

        public override void OnHide() { }

        public override void OnShow() { }

        #endregion -----------------------------------------------------

        #region Event --------------------------------------------------

        private void OnButtonCloseClick(GameObject go)
        {
            this.Close();
        }

        #endregion -----------------------------------------------------
    }
}