/**
 * @Author lyb
 *  单个消息信息展示
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMessageList : MonoBehaviour
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public long MessageId;
        /// <summary>
        /// 消息标题
        /// </summary>
        public UILabel MessageTitle;
        /// <summary>
        /// 消息内容
        /// </summary>
        public UILabel MessageDoc;
        /// <summary>
        /// 箭头
        /// </summary>
        public GameObject ArrowsObj;

        public UITweener TweerObj;
        public UIPlayTween TweerPlayObj;

        private MessageMail MessageData;

        void Start() { }

        void OnDestroy()
        {
            MessageTitle = null;
            MessageDoc = null;
            TweerObj = null;
            TweerPlayObj = null;
        }

        /// <summary>
        /// 初始化页签按钮
        /// </summary>
        public void MessageInit(MessageMail mData)
        {
            MessageData = mData;
            MessageId = mData.id;
            MessageTitle.text = mData.title;

            if (mData.type == MessageMail.EnumMessageType.Friend)
            {
                MessageTitle.text = "【" + mData.time + "】   " + mData.title;
            }

            MessageDoc.text = mData.contetn;

            if (MessageData.isRead)
            {
                //MessageTitle.color = Color.gray;
            }
        }

        /// <summary>
        /// 信件展开完毕
        /// </summary>
        public void PlayTweenFinish()
        {
            if (TweerObj.tweenFactor == 1)
            {
                if (!MessageData.isRead)
                {
                    // 没有读过信件
                    // 信件是被展开 ， 往服务器发送消息
                    //ModleNetWorker.Instance.MailReadReq(MessageId);

                    MemoryData.MessageData.IsReadState(MessageId);
                }

                Vector3 v3 = new Vector3(0.0f, 0.0f, 180.0f);
                ArrowsObj.transform.localRotation = Quaternion.Euler(v3);

                //MessageTitle.color = Color.gray;

                //QLoger.LOG(" ################### 信件展开完毕");
            }
            else
            {
                ArrowsObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
}