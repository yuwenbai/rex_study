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
    public class UICheatKeyShouPaiItem : MonoBehaviour {
        public UILabel LabelName;
        public UIGrid Grid;
        public UIDefinedButton ButtonDelete;
        public UIDefinedButton ButtonCreate;
        private CkData uiData;



        private Dictionary<GameObject, int> Go2IndexMap = new Dictionary<GameObject, int>();
        
        public void SetData(CkData data/*, System.Action<GameObject, CkData, int> clickFunc*/)
        {
            this.uiData = data;
            //this.clickCallBack = clickFunc;
        }

        public void Refresh()
        {
            LabelName.color = this.uiData == CheatKeyManager.Instance.CurrSelectCkData ? Color.red : Color.green;
            ButtonCreate.isEnabled = !(this.uiData.MaxPaiNum > 0 && this.uiData.PaiList.Count >= this.uiData.MaxPaiNum);
            ButtonDelete.isEnabled = !this.uiData.IsEmtry();
            LabelName.text = uiData.name;
            Go2IndexMap.Clear();
            UITools.CreateChild<int>(Grid.transform, null, uiData.PaiList, CreateChildCallBack);
            Grid.Reposition();
        }

        private void CreateChildCallBack(GameObject go, int index, int paiNum)
        {
            Go2IndexMap.Add(go, index);
            UISprite spr = go.GetComponent<UISprite>();
            //设置序号
            spr.cachedTransform.GetComponentInChildren<UILabel>().text = (index + 1).ToString();
            //设置透明度
            if(CheatKeyManager.Instance.CurrSelectCkData == this.uiData && CheatKeyManager.Instance.CurrSelectPaiIndex == index)
            {
                spr.alpha = 0.7f;
            }
            else
            {
                spr.alpha = 1f;
            }
            //设置图片
            if (paiNum > 0)
            {
                spr.spriteName = UICheatKeyModel.PaiSpriteNamePre + paiNum;
            }
            else
            {
                spr.spriteName = UICheatKeyModel.PaiBackSpriteName;
            }
            UIEventListener.Get(go).onClick = OnItemClick;
        }
        private void OnSelfClick(GameObject go)
        {
            CheatKeyManager.Instance.SelectShouPai(uiData);
        }

        private void OnItemClick(GameObject go)
        {
            if(Go2IndexMap.ContainsKey(go))
            {
                CheatKeyManager.Instance.SelectShouPai(uiData, Go2IndexMap[go]);
                go.GetComponent<UISprite>().alpha = 0.7f;
            }
        }

        private void OnButtonCreate(GameObject go)
        {
            CheatKeyManager.Instance.CreateCurrSelectPai(this.uiData);
            Refresh();
        }

        private void OnButtonDelete(GameObject go)
        {
            CheatKeyManager.Instance.DeleteCurrSelectPai(this.uiData);
            Refresh();
        }

        void Awake()
        {
            UIEventListener.Get(gameObject).onClick = OnSelfClick;
            UIEventListener.Get(ButtonCreate.gameObject).onClick = OnButtonCreate;
            UIEventListener.Get(ButtonDelete.gameObject).onClick = OnButtonDelete;
        }
    }
}