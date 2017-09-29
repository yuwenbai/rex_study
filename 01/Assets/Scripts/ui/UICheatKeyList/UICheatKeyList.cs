

using System;
/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using projectQ.chectKey;

namespace projectQ
{
    public class UICheatKeyList : UIViewBase
    {
        public UIGrid GridCheatName;
        public UIInput InputFilePath;

        private Dictionary<GameObject, CheatKeyData> GoAndData = new Dictionary<GameObject, CheatKeyData>();
        #region 刷新
        [ContextMenu("手动刷新下")]
        public void Refresh()
        {
            //CheatKeyManager.Instance.ReadCheatData();
            GoAndData.Clear();
            UITools.CreateChild<CheatKeyData>(GridCheatName.transform,null,CheatKeyManager.Instance.CheatDataListAll, OnCreateChildCallBack);
            GridCheatName.Reposition();
        }
        private void OnCreateChildCallBack(GameObject go, CheatKeyData data)
        {
            GoAndData.Add(go, data);
            go.GetComponentInChildren<UILabel>().text = data.Name;
            UIEventListener.Get(go).onClick = OnItemClick;
            if(data.Name == CheatKeyManager.Instance.CurrCheatName)
            {
                go.GetComponent<UISprite>().color = Color.red;
            }
            else
            {
                go.GetComponent<UISprite>().color = Color.white;
            }
        }
        private void OnItemClick(GameObject go)
        {
            CheatKeyManager.Instance.SelectCheatName(GoAndData[go].Name);
            Refresh();
        }
        #endregion

        #region ButtonEvent
        private void OnButtonSendClick(GameObject go)
        {
            var data = CheatKeyManager.Instance.GetCheatKeyData();
            if (!data.IsFull())
            {
                LoadTip("提示：手牌配置未满 发送测试码会失败");
            }
            CheatKeyManager.Instance.SendCode(CheatKeyManager.Instance.CurrCheatName);
        }
        private void OnButtonCreateClick(GameObject go)
        {
            CheatKeyManager.Instance.CreateUpdateData("新建", null);
            CheatKeyManager.Instance.SelectCheatName("新建");
            LoadUIMain("UICheatKey");
            this.Refresh();
        }
        private void OnButtonEditClick(GameObject go)
        {
            LoadUIMain("UICheatKey");
        }
        private void OnButtonDelClick(GameObject go)
        {
            CheatKeyManager.Instance.DeleteData(CheatKeyManager.Instance.GetCheatKeyData());
            CheatKeyManager.Instance.SaveCheatData();
            this.Refresh();
        }
        private void OnButtonImportClick(GameObject go)
        {
            string error = CheatKeyManager.Instance.Import(InputFilePath.value);
            if(error != null )
            {
                this.LoadTip(error);
            }
        }
        private void OnButtonExportClick(GameObject go)
        {
            CheatKeyManager.Instance.Export(InputFilePath.value);
        }

        private void OnButtonCloseClick(GameObject go)
        {
            this.Close();
        }
        private void OnButtonSaveClick(GameObject go)
        {
            CheatKeyManager.Instance.SaveCheatData();
            this.Refresh();
        }
        private void OnButtonClearClick(GameObject go)
        {
            CheatKeyManager.Instance.CheatDataMapAll_Clear();
            this.Refresh();
        }
        #endregion

        public override void Init()
        {
            UIEventListener.Get(transform.FindChild("Bg/ButtonGrid/ButtonSend").gameObject).onClick = OnButtonSendClick;
            UIEventListener.Get(transform.FindChild("Bg/ButtonGrid/ButtonCreate").gameObject).onClick = OnButtonCreateClick;
            UIEventListener.Get(transform.FindChild("Bg/ButtonGrid/ButtonEdit").gameObject).onClick = OnButtonEditClick;
            UIEventListener.Get(transform.FindChild("Bg/ButtonGrid/ButtonDel").gameObject).onClick = OnButtonDelClick;
            UIEventListener.Get(transform.FindChild("Bg/ButtonGrid/ButtonSave").gameObject).onClick = OnButtonSaveClick;
            UIEventListener.Get(transform.FindChild("Bg/ButtonGrid/ButtonClear").gameObject).onClick = OnButtonClearClick;

            UIEventListener.Get(transform.FindChild("Bg/Buttons/ButtonImport").gameObject).onClick = OnButtonImportClick;
            UIEventListener.Get(transform.FindChild("Bg/Buttons/ButtonExport").gameObject).onClick = OnButtonExportClick;

            UIEventListener.Get(transform.FindChild("Bg/ButtonClose").gameObject).onClick = OnButtonCloseClick;

            CheatKeyManager.Instance.ReadCheatData();
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
            Refresh();
        }
    }
}