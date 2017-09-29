/**
 * @Author YQC
 * 邮件相关数据类信息
 *
 */

using System.Collections.Generic;

namespace projectQ
{
    public class MessageMail
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public enum EnumMessageType
        {
            /// <summary>
            /// 系统消息
            /// </summary>
            System = 1,

            /// <summary>
            /// 好友消息
            /// </summary>
            Friend = 2
        }

        public long id;
        public string title;
        public string sMsg;
        public string contetn;
        public bool isRead;
        public bool isPickUp;
        public EnumMessageType type;
        public string time;

        public List<ItemInfo> attr = new List<ItemInfo>();

        public void MessageMailInit(Msg.MailInfo mail)
        {
            this.id = mail.MailID;
            this.title = mail.Title;
            this.sMsg = mail.ShortMsg;
            this.contetn = mail.Contents;
            this.isPickUp = mail.IsPickup;
            this.isRead = mail.IsRead;
            this.type = (EnumMessageType)mail.MailType;
            this.time = mail.MailTime;

            this.attr = new List<ItemInfo>();
            for (int j = 0; j < mail.AttachList.Count; j++)
            {
                var info = new ItemInfo();
                info.id = mail.AttachList[j].ItemID;
                info.count = mail.AttachList[j].ItemCount;
                this.attr.Add(info);
            }
        }
    }

    public class SysMessageData
    {
        private List<MessageMail> _MailList = new List<MessageMail>();
        public List<MessageMail> MailList
        {
            get
            {
                return _MailList;
            }
        }

        /// <summary>
        /// 邮件数据初始化
        /// </summary>
        public void MailMessage_Init(List<Msg.MailInfo> mailList)
        {
            for (int i = 0; i < mailList.Count; i++)
            {
                MessageMail op = new MessageMail();
                op.MessageMailInit(mailList[i]);

                MailMessage_Update(op);
            }
        }

        /// <summary>
        /// 邮件数据更新
        /// </summary>
        public void MailMessage_Update(MessageMail mail)
        {
            var op = MailMessage_Find(mail.id);
            if (op == null)
            {
                _MailList.Add(mail);
            }
            else
            {
                _MailList.Remove(mail);
                _MailList.Add(mail);
            }
        }

        /// <summary>
        /// 邮件数据查找
        /// </summary>
        public MessageMail MailMessage_Find(long mid)
        {
            for (int i = 0; i < _MailList.Count; i++)
            {
                if (_MailList[i].id == mid)
                {
                    return _MailList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 是否已读状态切换
        /// </summary>
        public void IsReadState(long mailId)
        {
            MessageMail mail = MailMessage_Find(mailId);
            mail.isRead = true;
        }
    }

    #region 内存数据 --------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_MESSAGE_MAIN_DATA = "USER_MESSAGE_MAIN_DATA";
    }

    public partial class MemoryData
    {
        static public SysMessageData MessageData
        {
            get
            {
                SysMessageData itemData = MemoryData.Get<SysMessageData>(MKey.USER_MESSAGE_MAIN_DATA);
                if (itemData == null)
                {
                    itemData = new SysMessageData();
                    MemoryData.Set(MKey.USER_MESSAGE_MAIN_DATA, itemData);
                }
                return itemData;
            }
        }
    }

    #endregion ---------------------------------------------------------------------    
}