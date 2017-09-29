/**
* @Author lyb
* Tips界面
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UITips : MonoBehaviour
    {
        /// <summary>
        /// 进度条
        /// </summary>
        public UILabel TipsLab;

        /// <summary>
        /// 本地随机tips列表
        /// </summary>
        private string[] RandomLocalTipStr;

        void Awake()
        {
            LocalTipArrayInit();            
        }

        void OnEnable()
        {
            if (TipsTypeDic.Count > 0)
            {
                TipsRandom();
            }
            else
            {
                TipsRandomData_Set();
            }            
        }

        void Start(){}

        #region 初始化---------------------------------------------------

        /// <summary>
        /// 本地随机tips列表初始化填充，适用于没有资源加载的时候
        /// </summary>
        void LocalTipArrayInit()
        {
            RandomLocalTipStr = new string[10];
            RandomLocalTipStr[0] = "加入官方麻将试玩群，开桌找麻友，想玩就玩。";
            RandomLocalTipStr[1] = "关联馆长棋牌室，桌卡更便宜哦。";
            RandomLocalTipStr[2] = "点击切换地区，可选择全国不同省区，这么全，这么好总有一款适合你。";
            RandomLocalTipStr[3] = "邀请好友有豪礼，桌卡/红包送给你。";
            RandomLocalTipStr[4] = "点击我要当馆长，一分钟自建网络棋牌室，自己的品牌，自己的生意。";
            RandomLocalTipStr[5] = "有问题，就找客服妹子，全天在线回复你哦！";
            RandomLocalTipStr[6] = "活动专区，日常福利多，参与更惊喜！";
            RandomLocalTipStr[7] = "点击查看规则，里面有36计哦。";
            RandomLocalTipStr[8] = "点击战绩即可查询历史战绩。";
            RandomLocalTipStr[9] = "369产品仅用于娱乐，严禁馆长用于任何其他用途。严禁赌博，一经发现，取消馆长资格。";
        }

        #endregion-------------------------------------------------------

        #region 随机规则算法---------------------------------------------

        private Dictionary<int, List<string>> TipsTypeDic = new Dictionary<int, List<string>>();

        /// <summary>
        /// 随机Tips数据获取
        /// </summary>
        void TipsRandomData_Set()
        {
            if (MemoryData.XmlData.XmlBuildDataDic.ContainsKey("Tips"))
            {
                List<BaseXmlBuild> buildList = MemoryData.XmlData.XmlBuildDataDic["Tips"];

                List<string> typeList1 = new List<string>();
                List<string> typeList2 = new List<string>();
                List<string> typeList3 = new List<string>();
                foreach (BaseXmlBuild build in buildList)
                {
                    Tips tip = (Tips)build;

                    if (tip.game_type == "1")
                    {
                        typeList1.Add(tip.tips_content);
                    }
                    else if (tip.game_type == "2")
                    {
                        typeList2.Add(tip.tips_content);
                    }
                    else
                    {
                        typeList3.Add(tip.tips_content);
                    }
                }

                TipsTypeDic.Add(1, typeList1);
                TipsTypeDic.Add(2, typeList2);
                TipsTypeDic.Add(3, typeList3);

                TipsRandom();
            }
            else
            {
                //QLoger.LOG(" Tips数据表不存在，循环本地 ");

                int num = Random.Range(0, RandomLocalTipStr.Length);

                TipsLab.text = RandomLocalTipStr[num];
            }
        }

        /// <summary>
        /// 获取随机值
        /// </summary>
        void TipsRandom()
        {
            List<string> typeStrList = new List<string>();

            int num = Random.Range(0, 10);

            if (num >= 0 && num < 5)
            {
                //Type == 1
                typeStrList = TipsTypeDic[1];
            }
            else if (num >= 5 && num < 8)
            {
                //Type == 2
                typeStrList = TipsTypeDic[2];
            }
            else
            {
                //Type == 3
                typeStrList = TipsTypeDic[3];
            }

            int index = Random.Range(0, typeStrList.Count);
            TipsLab.text = typeStrList[index];
        }

        #endregion-------------------------------------------------------
    }
}
