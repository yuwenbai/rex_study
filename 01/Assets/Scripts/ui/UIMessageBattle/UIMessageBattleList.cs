/**
 * @Author lyb
 *  单个战绩信息展示
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMessageBattleList : MonoBehaviour
    {
        public delegate void SelectBtnDelegate(bool isSelect, int deskId);
        public SelectBtnDelegate OnClickCallBack;

        public delegate void BattleInfoBtnDelegate(int deskId);
        public BattleInfoBtnDelegate OnBattleInfoClickCallBack;

        /// <summary>
        /// 牌桌Id
        /// </summary>
        public int MessageBattleId;
        /// <summary>
        /// 战绩时间 - 月 日
        /// </summary>
        public UILabel MessageBattleTime;
        /// <summary>
        /// 小圆点
        /// </summary>
        public UISprite DianSpr;
        /// <summary>
        /// 战绩时间 - 时 分
        /// </summary>
        public UILabel MessageBattleMinute;
        /// <summary>
        /// 战绩名字
        /// </summary>
        public UILabel MessageBattleName;
        /// <summary>
        /// 战绩头像
        /// </summary>
        public UITexture MessageBattleHeadTex;
        /// <summary>
        /// 战绩局数
        /// </summary>
        public UILabel MessageBattleNum;
        /// <summary>
        /// 战绩类型
        /// </summary>
        public UILabel MessageBattleType;
        /// <summary>
        /// 战绩详情按钮
        /// </summary>
        public GameObject MessageBattleInfoBtn;
        /// <summary>
        /// 选中按钮
        /// </summary>
        public UIToggle MessageBattleSelectBtn;

        private GameResult battleData;

        void Start() { }

        void OnDestroy()
        {
            MessageBattleTime = null;
            MessageBattleMinute = null;
            MessageBattleName = null;
            MessageBattleHeadTex = null;
            MessageBattleNum = null;
            MessageBattleType = null;
        }

        /// <summary>
        /// 初始化 - index 判断是不是第一个。。如果是第一个显示小点
        /// </summary>
        public void MessageBattleInit(GameResult deskData)
        {
            battleData = deskData;

            MessageBattleId = battleData.deskID;

            long bTime = deskData.recordTime;
            string timeStr = bTime.ToTimeFormatString();
            timeStr = timeStr.Replace(" ", "");

            string[] values = timeStr.Split(new char[] { '@' });
            MessageBattleMinute.text = values[1];

            //DianSpr.gameObject.SetActive(false);
            //MessageBattleSelectBtn.gameObject.SetActive(false);

            MessageBattleTime.text = "";
            string[] vData = values[0].Split(new char[] { '/' });
            MessageBattleTime.text = string.Format("{0}月{1}日", vData[1], vData[2]);

            MessageBattleNum.text = (deskData.bureauDetailList == null ? 0 : deskData.bureauDetailList.Count) + "/" + deskData.maxBouts.ToString();

            MessageBattleType.text = CardHelper.GetGameMessageByType(deskData.gameTypeSub);

            foreach (GameResultPlayer pData in deskData.resultPlayerList)
            {
                long userID = pData.userID;
                if (userID == deskData.ownerUserID)
                {
                    //PlayerDataModel model = MemoryData.PlayerData.get(userID);

                    // 这个人是房主。。显示这个人的信息
                    MessageBattleName.text = string.Format("{0}的牌局", pData.nickName);

                    DebugPro.Log(DebugPro.EnumLog.HeadUrl, "UIMessageBattleList__MessageBattleInit", pData.HeadUrl);
                    DownHeadTexture.Instance.WeChat_HeadTextureGet(pData.HeadUrl, HeadTexCallBack);
                }
            }

            UIEventListener.Get(MessageBattleInfoBtn).onClick = OnMessageBattleInfoBtnClick;
        }

        /// <summary>
        /// 头像回调
        /// </summary>
        void HeadTexCallBack(Texture2D HeadTexture, string headName)
        {
            MessageBattleHeadTex.mainTexture = HeadTexture;
        }

        /// <summary>
        /// 点击选择按钮
        /// </summary>
        public void BattleInfoSelectBtnClick()
        {
            //QLoger.LOG("点击选择按钮 MessageBattleSelectBtn.value = " + MessageBattleSelectBtn.value);

            if (OnClickCallBack != null)
            {
                OnClickCallBack(MessageBattleSelectBtn.value, battleData.deskID);
            }
        }

        /// <summary>
        /// 点击战绩单个面板对局详情按钮
        /// </summary>
        public void OnMessageBattleInfoBtnClick(GameObject obj)
        {
            if (OnBattleInfoClickCallBack != null)
            {
                OnBattleInfoClickCallBack(battleData.deskID);
            }
        }
    }
}