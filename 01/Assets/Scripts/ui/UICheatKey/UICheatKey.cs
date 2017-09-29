/**
* @Author YQC
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.ScrollViewTool;
using projectQ.chectKey;

namespace projectQ
{
    public class UICheatKey : UIViewBase
    {
        private UICheatKeyModel Model
        {
            get { return _model as UICheatKeyModel; }
        }

        public UIGrid GridPai;
        public UITable TabelShouPaiRoot;
        public UIInput InputName;
        public GameObject PrefabShouPaiItem;
        public GameObject ButtonSave;
        public GameObject ButtonReset;
        public GameObject ButtonClose;

        #region 牌
        public void RefreshPai()
        {
            CheatKeyManager.Instance.ResetPaiData(ref Model.PaiList);

            UITools.CreateChild<CheatKeyPai>(GridPai.transform,null,Model.PaiList, OnCreatePai);
            GridPai.Reposition();
            //GridPai.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        }
        private void OnCreatePai(GameObject go,CheatKeyPai value)
        {
            go.GetComponent<UISprite>().spriteName = value.SpriteName;
            UILabel label = go.transform.FindChild("LabelNum").GetComponent<UILabel>();
            label.text = "x" + value.CurrNum;
            if (value.CurrNum >= 0)
            {
                label.color = Color.green;
            }
            else
            {
                label.color = Color.red;
            }
            UIEventListener.Get(go).onClick = OnPaiClick;
        }
        private void OnPaiClick(GameObject go)
        {
            string name = go.GetComponent<UISprite>().spriteName;
            string[] names = name.Split('_');
            int temp = 0;
            if(int.TryParse(names[names.Length - 1],out temp))
            {
                if(CheatKeyManager.Instance.SelectPai(temp))
                {
                    RefreshShouPai(false);
                    RefreshPai();
                }
            }
        }
        #endregion

        #region 手牌
        /// <summary>
        /// 刷新手牌列表
        /// </summary>
        public void RefreshShouPai(bool isResetPos = true)
        {
            var data = CheatKeyManager.Instance.GetCheatKeyData();
            InputName.value = data.Name;
            UITools.CreateChild<CkData>(TabelShouPaiRoot.transform, PrefabShouPaiItem, data.ContentList, CreateShouPaiItem);
            //if(isResetPos)
            {
                TabelShouPaiRoot.Reposition();
                //TabelShouPaiRoot.transform.parent.GetComponent<UIScrollView>().ResetPosition();
            }
        }
        /// <summary>
        /// 创建手牌
        /// </summary>
        /// <param name="go"></param>
        /// <param name="data"></param>
        private void CreateShouPaiItem(GameObject go, CkData data)
        {
            var script = go.GetComponent<UICheatKeyShouPaiItem>();
            script.SetData(data);
            script.Refresh();
        }

        #endregion

        #region 按钮事件
        private void OnButtonSaveClick(GameObject go)
        {
            var data = CheatKeyManager.Instance.GetCheatKeyData();
            data.Name = InputName.value.Trim();
            CheatKeyManager.Instance.SaveCheatData();
            if(!data.IsFull())
            {
                LoadTip("提示：手牌配置未满 发送测试码会失败");
            }
        }
        private void OnButtonResetClick(GameObject go)
        {
            CheatKeyManager.Instance.ReadCheatData();
            RefreshPai();
            RefreshShouPai();
        }
        private void OnButtonCloseClick(GameObject go)
        {
            this.Close();
        }
        #endregion

        public override void Init()
        {
            UIEventListener.Get(ButtonSave).onClick = OnButtonSaveClick;
            UIEventListener.Get(ButtonReset).onClick = OnButtonResetClick;
            UIEventListener.Get(ButtonClose).onClick = OnButtonCloseClick;
            //EventDelegate.Add(InputName.onChange, OnInputNameChange);
        }

        public override void OnHide()
        {
            CheatKeyManager.Instance.ReadCheatData();
        }

        public override void OnShow()
        {
            RefreshPai();
            RefreshShouPai();
        }
    }
}