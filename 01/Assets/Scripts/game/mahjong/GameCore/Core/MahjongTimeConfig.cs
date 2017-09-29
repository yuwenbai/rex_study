/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace projectQ
{
    public class MahjongTimeConfig : MonoBehaviour
    {
        #region single

        private static MahjongTimeConfig _instance;
        public static MahjongTimeConfig Instance
        {
            get { return _instance; }
        }

        #endregion


        #region Path Control
        private string systemPath = "GamePrefab/Mahjong/TimeConfig/SystemTimeConfigData";


        #endregion




        /// <summary>
        /// 麻将系统级别的时间
        /// </summary>
        private SystemTimeConfig _systemTime = null;
        public SystemTimeConfig MjSystemTime
        {
            get
            {
                if (_systemTime == null)
                {
                    _systemTime = Resources.Load<SystemTimeConfig>(systemPath);
                }
                return _systemTime;
            }
        }







        private void Awake()
        {
            _instance = this;
        }

    }

}
