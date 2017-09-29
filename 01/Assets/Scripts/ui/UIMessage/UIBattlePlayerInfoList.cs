/**
 * @Author lyb
 *  战绩邮件中的个人信息展示
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIBattlePlayerInfoList : MonoBehaviour
    {
        /// <summary>
        /// 人员姓名
        /// </summary>
        public UILabel PlayerName;
        /// <summary>
        /// 人员胜局
        /// </summary>
        public UILabel PlayerWinValue;
        /// <summary>
        /// 人员积分
        /// </summary>
        public UILabel PlayerScoreValue;

        void Start() { }

        void OnDestroy()
        {
            PlayerName = null;
            PlayerWinValue = null;
            PlayerScoreValue = null;
        }

        /// <summary>
        /// 初始化页签按钮
        /// </summary>
        public void BatterPlayerInfoInit(GameResultPlayer playerInfo)
        {
            PlayerName.text = playerInfo.nickName;


            string cStr = "[FFFFFF]";
            if (playerInfo.WinBouts > 0)
            {
                // 绿色
                cStr = "[66CD00]";
            }
            else if (playerInfo.WinBouts < 0)
            {
                // 红色
                cStr = "[FF0000]";
            }
            PlayerWinValue.text = cStr + playerInfo.WinBouts + "[-]";


            string colorStr = "[FFFFFF]";
            if (playerInfo.Score > 0)
            {
                // 绿色
                colorStr = "[66CD00]";
            }
            else if (playerInfo.Score < 0)
            {
                // 红色
                colorStr = "[FF0000]";
            }
            PlayerScoreValue.text = colorStr + playerInfo.Score + "[-]";
        }
    }
}


