using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projectQ
{
    public class User_hand_info
    {
        public long uid;
        public SexEnum sex;
        public bool dirty;
        public User_hand_info(long uid, bool d)
        {
            this.uid = uid;
            this.dirty = d;
            this.sex = SexEnum.Man;
        }
    };
    public abstract class AnimatorData
    {
        public List<User_hand_info> mUserHandInfoList = null;
        /// <summary>
        /// 
        /// </summary>
        private int nPlayerCount;
        public int PlayerCount
        {
            get { return nPlayerCount; }
            set { nPlayerCount = value; }
        }

        //"Model/HandPos"
        private string strPosCfg;
        public string AnimatorHandPosCfg
        {
            get { return strPosCfg; }
            set { strPosCfg = value; }
        }

        private AnimatorHandPos mStData;
        public AnimatorHandPos StData
        {
            get { return mStData; }
            set { mStData = value; }
        }

        public abstract void Init();

        public abstract void Clear();
    }
}