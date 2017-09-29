using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMahjongMingDaBtn : UIViewBase
    {
        public GameObject BtnItem;
        public UIGrid BtnGrid;
        public UILabel TitleLab;

        public override void Init()
        {
        }

        public override void OnHide()
        {
        }

        public override void OnShow()
        {
        }
        
        private GameObject CreatItem()
        {
            GameObject addObj = NGUITools.AddChild(BtnGrid.gameObject, BtnItem.gameObject);
            addObj.gameObject.SetActive(true);
            return addObj;
        }

        private void CreateShow(EnumMjSpecialCheck type, List<int> values)
        {
            switch (type)
            {
                default:
                    int childCount = BtnGrid.transform.childCount;

                    if (childCount > 0)
                    {
                        for (int i = 0; i < childCount; i++)
                        {
                            Destroy(BtnGrid.GetChild(0));
                        }
                    }

                    for (int i = 0; i < values.Count; i++)
                    {
                        GameObject createObj = CreatItem();
                        UILabel creatLab = createObj.GetComponentInChildren<UILabel>();

                        creatLab.text = MjDataManager.Instance.MjData.ProcessData.processMingDaCard.GetBtnName(type, values[i]);
                        createObj.name = values[i].ToString();

                        UIEventListener.Get(createObj).onClick = OnClickBtn;
                    }
                    break;
            }
        }
        
        public void RefreshUI(EnumMjSpecialCheck type, bool isSelect, List<int> values)
        {
            MahjongPlayType.MjMingDaData data = MjDataManager.Instance.GetMainPlayer();
            if (NullHelper.IsObjectIsNull(data))
            {
                return;
            }

            if (isSelect)
            {
                BtnGrid.gameObject.SetActive(false);
            }
            else
            {
                if (!BtnGrid.gameObject.activeSelf)
                    BtnGrid.gameObject.SetActive(true);

                CreateShow(type, values);
            }
        }

        private void OnClickBtn(GameObject obj)
        {
            int value = int.Parse(obj.name);
            EventDispatcher.FireEvent(MJEnum.MingDaEvents.MD_ClickBtn.ToString(), value);
        }

        public void RefreshResultUI()
        {
        }
    }
}
