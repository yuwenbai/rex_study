
using UnityEngine;

namespace projectQ
{
    public class UIModificationAccountModel : UIModelBase {
        private UIModificationAccount UI
        {
            get { return _ui as UIModificationAccount; }
        }
      

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.EModificationAccountResult,
            };
        }
        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                case GEnum.NamedEvent.EModificationAccountResult:
                    int resultCode = (int)data[0];
                    if (resultCode == (int)Msg.ErrorCode.ErrCode_Success)//成功
                    {
                        WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "ID修改成功", new string[] { "确认" }, null);
                    }
                    else if (resultCode == (int)Msg.ErrorCode.ErrCode_Reg_AccExist)
                    {
                        WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "修改失败，帐号重复", new string[] { "确认" }, null);
                    }
                    else if (resultCode == (int)Msg.ErrorCode.ErrCode_Auth_AccNone)
                    {
                        WindowUIManager.Instance.CreateOrAddWindow(WindowUIType.SystemPopupWindow, "提示", "账号验证，账号数据不存在", new string[] { "确认" }, null);
                    }
                    break;
                
            }
        }
        #endregion

        private void Start()
        {
           
        }

    }
}
