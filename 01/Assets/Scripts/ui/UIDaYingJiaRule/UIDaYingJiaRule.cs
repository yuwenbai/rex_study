using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIDaYingJiaRule : UIViewBase
    {
        public UIGrid m_PlayerGrid;
        public DaYingJiaItem m_PlayerItem;
        public UILabel m_Label_Title;

        public GameObject m_Btn_Close;

        public UILabel m_Label_Des;

        private GameResult m_ResultData;

        #region 重新
        public override void Init()
        {
        }
        public override void OnShow()
        {
        }
        public override void OnHide()
        {
        }
        #endregion

        public override void OnPushData(object[] data)
        {
            if (data.Length < 1)
            {
                return;
            }

            m_ResultData = (GameResult)data[0];

            if(m_ResultData == null)
            {
                return;
            }

            UIEventListener.Get(m_Btn_Close).onClick = OnClickClose;

            InitShow();
        }

        private void InitShow()
        {
            List<int> keys = new List<int>();
            foreach (var item in m_ResultData.seeDaYingJiaDict)
            {
                if (item.Value.isDaYingJia)
                    keys.Add(item.Key);
            }

            if (keys.Count < 1)
            {
                foreach (var item in m_ResultData.seeDaYingJiaDict)
                {
                    keys.Add(item.Key);
                }

                m_Label_Des.text = "入桌时间";
                m_Label_Title.text = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_110);
                if (keys.Count < 1)
                    return;
            }
            else
            {
                m_Label_Des.text = "“大赢家”入桌时间";
                m_Label_Title.text = MemoryData.XmlData.XmlBuildDataText_Get(GTextsEnum.G_Text_109);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                GameObject addObj = NGUITools.AddChild(m_PlayerGrid.gameObject, m_PlayerItem.gameObject);
                addObj.gameObject.SetActive(true);
                DaYingJiaItem itemData = addObj.GetComponent<DaYingJiaItem>();
                if (itemData == null)
                    continue;

                var data = m_ResultData.seeDaYingJiaDict[keys[i]];
                if (data == null)
                    continue;

                string setName = data.nickName;
                if (keys[i] == m_ResultData.selfSeatID)
                    setName = "我";
                itemData.InitItem(setName, data.data.joinRoomTime, data.data.costCardNum);
            }

            m_PlayerGrid.Reposition();
        }

        private void OnClickClose(GameObject obj)
        {
            this.Close();
        }
    }
}
