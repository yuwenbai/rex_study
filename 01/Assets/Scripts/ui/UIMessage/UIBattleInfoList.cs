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
    public class UIBattleInfoList : MonoBehaviour
    {
        public delegate void MessageBattleInfoBtnDelegate(int deskId);
        public MessageBattleInfoBtnDelegate OnMessageBattleInfoClickCallBack;

        /// <summary>
        /// 消息ID
        /// </summary>
        public int BattleDeskId;
        /// <summary>
        /// 消息标题
        /// </summary>
        public UILabel ButtionInfoTitle;
        /// <summary>
        /// 详情按钮
        /// </summary>
        public GameObject DetailBtn;
        /// <summary>
        /// 箭头
        /// </summary>
        public GameObject ArrowsObj;

        public UITweener TweerObj;
        public UIPlayTween TweerPlayObj;

        public List<UIBattlePlayerInfoList> BPlayerInfoList;

        /// <summary>
        /// 消息状态
        /// 0 未读 , 1 已读 , 2 删除
        /// </summary>
        private int BattleInfoState;

        void Start() { }

        void OnDestroy()
        {
            ButtionInfoTitle = null;
            TweerObj = null;
            TweerPlayObj = null;
            BPlayerInfoList = null;
        }

        /// <summary>
        /// 初始化页签按钮
        /// </summary>
        public void ButtionInfoInit(GameResult bData)
        {
            UIEventListener.Get(DetailBtn).onClick = OnDetailtnClick;

            BattleDeskId = bData.deskID;
            BattleInfoState = bData.showState;
            long bTime = bData.recordTime;
            string timeStr = bTime.ToTimeFormatString();
            timeStr = timeStr.Replace(" ", "");
            string[] values = timeStr.Split(new char[] { '@' });
            string[] vData = values[0].Split(new char[] { '/' });
            string str = string.Format("{0}-{1}-{2}", vData[0], vData[1], vData[2]);

            ButtionInfoTitle.text = string.Format("{0}    {1} 战绩", str, values[1]);
            if (BattleInfoState == (int)MessageBattleStateEnum.MESSAGE_BATTLE_READ)
            {
                //ButtionInfoTitle.color = Color.gray;
            }

            for (int i = 0; i < bData.resultPlayerList.Count; i++)
            {

                BPlayerInfoList[i].BatterPlayerInfoInit(bData.resultPlayerList[i]);
            }
        }

        /// <summary>
        /// 详情按钮被点击
        /// </summary>
        private void OnDetailtnClick(GameObject go)
        {
            if (OnMessageBattleInfoClickCallBack != null)
            {
                OnMessageBattleInfoClickCallBack(BattleDeskId);
            }
        }

        /// <summary>
        /// 信件展开完毕
        /// </summary>
        public void PlayTweenFinish()
        {
            if (TweerObj.tweenFactor == 1)
            {
                // 信件是被展开 ， 往服务器发送消息
                Vector3 v3 = new Vector3(0.0f, 0.0f, 180.0f);
                ArrowsObj.transform.localRotation = Quaternion.Euler(v3);

                if (BattleInfoState == (int)MessageBattleStateEnum.MESSAGE_BATTLE_UNREAD)
                {
                    List<int> readList = new List<int>();
                    readList.Add(BattleDeskId);
                    ModelNetWorker.Instance.C2SMessageBattleDeleteReq(MessageBattleStateEnum.MESSAGE_BATTLE_READ, readList);
                }

                //ButtionInfoTitle.color = Color.gray;
            }
            else
            {
                ArrowsObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }

            //QLoger.LOG(" ################### 战绩信件展开完毕");
        }
    }
}


