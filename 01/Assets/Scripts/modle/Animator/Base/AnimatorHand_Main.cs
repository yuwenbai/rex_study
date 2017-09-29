/**
 * @Author lyb
 * 手动画全局绑定脚本
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class AnimatorHand_Main : MonoBehaviour
    {

        const string handNan = "Model/Prefab_Hand/Hand_Man";
        const string handNv = "Model/Prefab_Hand/Hand_WoMan";

        private static AnimatorHand_Main _instance;
        public static AnimatorHand_Main Instance
        {
            get { return _instance; }
        }

        private AnimatorData mAnimatorData;
        public static void LOG(LogType t, string p, params object[] v)
        {
            QLoger.LOG(typeof(AnimatorHand_Main), t, p, v);
        }

        public void SetMahjongData(AnimatorData mData)
        {
            this.mAnimatorData = mData;
            AnimatorHand_Creat();
        }

        /// <summary>
        /// 当前选择的是男还是女
        /// </summary>
        private SexEnum sexEnum;
        /// <summary>
        /// 创建的4只手的索引
        /// </summary>
		//private GameObject[] AnimHandList = new GameObject[] { null, null, null, null };
        private List<GameObject> AnimHandList = null;


        void OnEnable()
        {
            //EventDispatcher.AddEvent(MJEnum.HandModelEvent.HME_Init.ToString(), AnimatorHand_Init);
            EventDispatcher.AddEvent(MJEnum.HandModelEvent.HME_Play.ToString(), AnimatorHand_Play);
            EventDispatcher.AddEvent(MJEnum.HandModelEvent.HME_Destroy.ToString(), AnimatorHand_Destroy);
        }

        void OnDisable()
        {
           // EventDispatcher.RemoveEvent(MJEnum.HandModelEvent.HME_Init.ToString(), AnimatorHand_Init);
            EventDispatcher.RemoveEvent(MJEnum.HandModelEvent.HME_Play.ToString(), AnimatorHand_Play);
            EventDispatcher.RemoveEvent(MJEnum.HandModelEvent.HME_Destroy.ToString(), AnimatorHand_Destroy);
        }

        #region ----------初始化创建4只手--------------------------------

        /// <summary>
        /// 创建手的模型
        /// </summary>
        void AnimatorHand_Init(object[] data)
        {
            AnimatorHand_Creat();
        }

        private void Awake()
        {
            _instance = this;
        }
        private GameObject CreateHand(string key, SexEnum sex, AnimatorHandPos.PosDataBase posData)
        {
            GameObject prefab = ResourcesDataLoader.Load<GameObject>(key);
            GameObject obj = Object.Instantiate(prefab) as GameObject;

            Animator_Hand modelHand = obj.GetComponent<Animator_Hand>();
            modelHand.AnimSex = sex;

            modelHand.transform.localPosition = posData.FromV3;
            modelHand.transform.localRotation = Quaternion.Euler(posData.RotateV3);
            modelHand.transform.localScale = posData.ScaleV3;

            return obj;
        }

        void AnimatorHand_Creat()
        {
            AnimTimeTick.Instance.Destory();
            AnimatorHand_Destroy(null);
            if (this.mAnimatorData != null)
            {
                AnimHandList = new List<GameObject>(this.mAnimatorData.PlayerCount);
                if (this.mAnimatorData.StData != null)
                {
                    for (int i = 0; i < this.mAnimatorData.StData.HandPosList.Count; ++i)
                    {
                        var p = this.mAnimatorData.StData.HandPosList[i];
                        var hand = this.CreateHand(this.mAnimatorData.mUserHandInfoList[i].sex == SexEnum.Man ? handNan : handNv, this.mAnimatorData.mUserHandInfoList[i].sex, p);
                        AnimHandList.Add(hand);
                    }
                    
                }
            }
            else
            {

            }

            //AnimatorHandPos StData = Resources.Load<AnimatorHandPos>("Model/HandPos");

            //if (StData != null)
            //{
            //    for (int i = 0; i < StData.HandPosList.Count; i++)
            //    {
            //        var p = StData.HandPosList[i];

            //        if (UserIDCache[i].dirty)
            //        {
            //            var hand = AnimHandList[i];

            //            if (hand != null)
            //            {
            //                Object.Destroy((Object)hand);
            //                AnimHandList[i] = null;
            //            }

            //            hand = this.CreateHand(UserIDCache[i].sex == SexEnum.Man ? handNan : handNv, UserIDCache[i].sex, p);
            //            UserIDCache[i].dirty = false;
            //            AnimHandList[i] = hand;

            //            LOG(LogType.ELog, "初始化手{0} {1}", UserIDCache[i].uid, UserIDCache[i].sex);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 清除手的模型
        /// </summary>
        public void AnimatorHand_Destroy(object[] data)
        {
            if (AnimHandList != null)
            {
                for (int i = 0; i < AnimHandList.Count; i++)
                {
                    if (AnimHandList[i] != null)
                    {
                        Object.Destroy(AnimHandList[i]);

                        AnimHandList[i] = null;
                    }
                }
                AnimHandList.Clear();
            }
        }

        #endregion ------------------------------------------------------     

        #region ----------调用动画播放-----------------------------------

        /// <summary>
        /// 参数1 枚举 AnimatorEnum
        /// 参数2 Vector3 p0
        /// 参数3 Vector3 p1
        /// 参数4 哪家 0是本家往右转依次为 1 2 3
        /// </summary>
        void AnimatorHand_Play(object[] data)
        {
            AnimatorEnum animEnum = (AnimatorEnum)data[0];
            Vector3 p0 = (Vector3)data[1];
            Vector3 p1 = (Vector3)data[2];
            int index = (int)data[3];
            int extend = (int)data[4];

            if (index >= 4)
            {
                LOG(LogType.EError, "牌桌手动画播放错误，没有索引{0}", index);

                return;
            }

            //AnimatorPosInit(index);
            if (AnimHandList[index] != null)
            {
                AnimHandList[index].GetComponent<Animator_Hand>().Animator_Init(animEnum, p0, p1, index, extend);
            }
            else
            {
                //LOG(LogType.EError, "手不存在，没有创建UID{0}SEX{1}", UserIDCache[index].uid, UserIDCache[index].sex);
            }

            //StartCoroutine(Animator_DelayPos(animEnum, p0, p1, index, extend));
        }

        //IEnumerator Animator_DelayPos(AnimatorEnum animEnum, Vector3 p0, Vector3 p1, int index, int extend)
        //{
        //    yield return new WaitForSeconds(0.1f);

        //    AnimHandList[index].GetComponent<Animator_Hand>().Animator_Init(animEnum, p0, p1, index, extend);
        //}

        #endregion ------------------------------------------------------

        #region ----------初始化手的坐标---------------------------------

        /// <summary>
        /// 初始化位置坐标
        /// </summary>
        void AnimatorPosInit(int index)
        {
            string animSetpath = "Model/HandPos";

            AnimatorHandPos StPosData = Resources.Load<AnimatorHandPos>(animSetpath);

            AnimatorHandPos.PosDataBase posData = StPosData.HandPosList[index];

            if (posData != null)
            {
                AnimHandList[index].transform.localPosition = posData.FromV3;
                AnimHandList[index].transform.localRotation = Quaternion.Euler(posData.RotateV3);
                AnimHandList[index].transform.localScale = posData.ScaleV3;

                AnimHandList[index].GetComponent<Animator_Hand>().Model_Hand.GetComponent<Animator>().CrossFade("Idle", 0);
            }
        }

        #endregion ------------------------------------------------------ 

#region ----------测试 测试--------------------------------------
#endregion ------------------------------------------------------
        ///// <summary>
        ///// 创建模型
        ///// </summary>
        //public void AnimatorHand_Creat(SexEnum sexEnum)
        //{
        //    string handStr = "";
        //    if (sexEnum == SexEnum.Man)
        //    {
        //        handStr = "Model/Prefab_Hand/Hand_Man";
        //    }
        //    else
        //    {
        //        handStr = "Model/Prefab_Hand/Hand_WoMan";
        //    }

        //    string animSetpath = "Model/HandPos";

        //    AnimatorHandPos StData = Resources.Load<AnimatorHandPos>(animSetpath);

        //    if (StData != null)
        //    {
        //        foreach (AnimatorHandPos.PosDataBase posData in StData.HandPosList)
        //        {
        //            GameObject prefab = ResourcesDataLoader.Load<GameObject>(handStr);
        //            GameObject obj = GameObject.Instantiate(prefab) as GameObject;

        //            Animator_Hand modelHand = obj.GetComponent<Animator_Hand>();
        //            modelHand.AnimSex = sexEnum;
        //            modelHand.transform.localPosition = posData.FromV3;
        //            modelHand.transform.localRotation = Quaternion.Euler(posData.RotateV3);
        //            modelHand.transform.localScale = posData.ScaleV3;
        //        }
        //    }
        //}
    }

}