

/**
* 作者：周腾
* 作用：
* 日期：
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class ActivityLuckDraw : MonoBehaviour
    {

        #region Effect
        public GameObject effect_JianTou;
        #endregion
        #region 动画
        public GameObject runEndAni;
        public GameObject guangdianAni;
        public GameObject HideGuangDian1;
        public GameObject HideGuangDian2;
        public GameObject HideGuangDian3;
        public GameObject HideGuangDian4;
        public GameObject HideGuangDian5;
        public GameObject HideGuangDian6;
        public GameObject HideGuangDian7;
        public List<GameObject> hideGuangDianList;
        #endregion
        public UIActivity_old activity;
        public GameObject contentObj;
        public List<GameObject> awardItemList = new List<GameObject>();
        public GameObject maskObj;
        public GameObject drawOverObj;
        public GameObject drawButton;
        public GameObject needRotateObj;

        public List<GameObject> guangdianList = new List<GameObject>();
        [HideInInspector]
        public bool isBeginTick;
        [HideInInspector]
        public bool isGetPositionOver;
        private int getAwardIndex;
        private bool canLuck = true;
        private void Awake()
        {
            UIEventListener.Get(drawButton).onClick = BeginRun;
            positions = new int[] { 45, 0, 300, 240, 180, 120 };
            drawButton.SetActive(true);
            drawButton.GetComponent<BoxCollider>().enabled = false;
            drawButton.GetComponent<UIButton>().enabled = false;
            drawButton.GetComponent<UISprite>().spriteName = "lottery_Button_cj2";
            //activity.Model.GetAwardConfigReq(Msg.LotteryTypeDef.Lottery_Game);
        }

        void OnLuckClick(int index) { }
        /// <summary>
        /// 如果抽奖次数为零，显示“已抽取”
        /// 如果抽奖次数不为零，不显示“已抽取”
        /// </summary>
        public void RefreshUI()
        {
            if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes == 0)//可以抽奖
            {
                drawButton.GetComponent<UISprite>().spriteName = "lottery_button_01";
                drawButton.GetComponent<BoxCollider>().enabled = true;
                drawButton.GetComponent<UIButton>().enabled = true;
            }
            else//不可以抽奖
            {
                drawButton.GetComponent<UISprite>().spriteName = "lottery_Button_cj2";
                drawButton.GetComponent<BoxCollider>().enabled = false;
                drawButton.GetComponent<UIButton>().enabled = false;
            }
            if (activity.Model.GetAwardConfigList() != null)
            {
                if ((activity.Model.GetAwardConfigList().Count != 6))
                {
                    canLuck = false;
                    drawButton.GetComponent<UISprite>().spriteName = "lottery_Button_cj2";
                    drawButton.GetComponent<BoxCollider>().enabled = false;
                    drawButton.GetComponent<UIButton>().enabled = false;
                    string[] btnNames = new string[1];
                    btnNames[0] = "确定";
                    activity.LoadPop(WindowUIType.SystemPopupWindow, "错误提示", "奖品配置错误，抱歉！请联系客服！", btnNames, OnLuckClick);
               
                }
                for (int i = 0; i < activity.Model.GetAwardConfigList().Count; i++)
                {
                    awardItemList[i].GetComponent<LuckAwardItem>().InitAwardItem(activity.Model.GetAwardConfigList()[i].AwardCount, activity.Model.GetAwardConfigList()[i].AwardName, activity.Model.GetAwardConfigList()[i].ResUrl, activity.Model.GetAwardConfigList()[i].AwardID);
                }
                if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes == 0 && canLuck)//可以抽奖
                {
                    drawButton.GetComponent<UISprite>().spriteName = "lottery_button_01";
                    drawButton.GetComponent<BoxCollider>().enabled = true;
                    drawButton.GetComponent<UIButton>().enabled = true;
                }
                else//不可以抽奖
                {
                    drawButton.GetComponent<UISprite>().spriteName = "lottery_Button_cj2";
                    drawButton.GetComponent<BoxCollider>().enabled = false;
                    drawButton.GetComponent<UIButton>().enabled = false;
                    if (!canLuck)
                    {
                        string[] btnNames = new string[1];
                        btnNames[0] = "确定";
                        activity.LoadPop(WindowUIType.SystemPopupWindow, "错误提示", "奖品配置错误，抱歉！请联系客服！", btnNames, OnLuckClick);
                    }
                }

                runEndAni.SetActive(false);
            }
        }
        /// <summary>
        /// 转盘开始转
        /// </summary>
        public void BeginRun(GameObject go)
        {
            if (isBeginTick)
            {
                return;
            }
            else
            {
                if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes != 0)
                {
                    activity.LoadTip("您没有抽奖次数了");
                }
                else
                {
                    activity.Model.GetLotteryReq(Msg.LotteryTypeDef.Lottery_Game);

                }
            }
        }

        public void CanBeginRun()
        {
            maskObj.SetActive(true);
            needRotateObj.transform.localEulerAngles = Vector3.zero;
            QLoger.LOG("begin run");
            isBeginTick = true;
            runBefore = false;
            #region 箭头特效
            effect_JianTou.SetActive(true);
            #endregion

            MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.lotteryTimes--;
        }

        #region Run
        [HideInInspector]
        public int[] positions;
        private float m_time = 0f;
        private float speed = 100;
        public float a = 50f;
        private float stantSpeed;
        private float endPos;
        private float plusSpeedTime = 0.3f;
        private float contentSpeedTime = 0.8f;
        #endregion
        #region GuangDian
        private float normalGuangdianTime = 0f;//没抽奖时候计时器
        private float guangdianTimer = 0f;//光点闪动的计时器
        #endregion
        /// <summary>
        /// 光点的控制
        /// </summary>
        void ShowGuangdian()
        {
            if (isBeginTick)
            {
                guangdianTimer += Time.deltaTime;
                if (guangdianTimer > 0 && guangdianTimer <= 0.2f)
                {
                    for (int i = 0; i < guangdianList.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            guangdianList[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            guangdianList[i].gameObject.SetActive(false);
                        }
                    }
                }
                else if (guangdianTimer > 0.2f && guangdianTimer <= 0.4f)
                {
                    for (int i = 0; i < guangdianList.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            guangdianList[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            guangdianList[i].gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < guangdianList.Count; i++)
                    {

                        guangdianList[i].gameObject.SetActive(false);

                    }
                    guangdianTimer = 0f;

                }
            }
            else if (runBefore)
            {
                normalGuangdianTime += Time.deltaTime;
                if (normalGuangdianTime >= 0 && normalGuangdianTime < 1)
                {
                    for (int i = 0; i < guangdianList.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            guangdianList[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            guangdianList[i].gameObject.SetActive(false);
                        }
                    }
                }
                else if (normalGuangdianTime >= 1 && normalGuangdianTime < 2)
                {
                    for (int i = 0; i < guangdianList.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            guangdianList[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            guangdianList[i].gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    normalGuangdianTime = 0f;
                }
            }



        }
        private bool runBefore = true;
        private void FixedUpdate()
        {
            ShowGuangdian();
            RunIsBegin();
            BeginShowEffectDi();
            BeginShowSelf();
        }

        void ShowSelfGuangDian()
        {
            for (int i = 0; i < guangdianList.Count; i++)
            {
                guangdianList[i].gameObject.SetActive(false);
            }

            return;

        }
        /// <summary>
        /// 开始特效底板变化
        /// </summary>
        void BeginShowEffectDi()
        {
            if (isBeginShowEffect)
            {
                effectTimer += Time.deltaTime;
                if (effectTimer > 0 && effectTimer <= firstShowTime)
                {
                    for (int i = 0; i < effectList.Count; i++)
                    {
                        if (i == firstShow)
                        {
                            effectList[i].SetActive(true);
                        }
                        else
                        {
                            effectList[i].SetActive(false);
                        }
                    }

                }
                else if (effectTimer >= firstShowTime && effectTimer < secondShowTime)
                {
                    for (int i = 0; i < effectList.Count; i++)
                    {
                        if (i == secondShow1 || i == secondShow2)
                        {
                            effectList[i].SetActive(true);

                        }
                        else
                        {
                            effectList[i].SetActive(false);
                        }
                    }
                }
                else if (effectTimer >= secondShowTime && effectTimer < ThirdShowTime)
                {
                    for (int i = 0; i < effectList.Count; i++)
                    {
                        if (i == thirdShow1 || i == thirdShow2)
                        {
                            effectList[i].SetActive(true);

                        }
                        else
                        {
                            effectList[i].SetActive(false);
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < effectList.Count; i++)
                    {
                        if (i == fourShow)
                        {
                            effectList[i].SetActive(true);

                        }
                        else
                        {
                            effectList[i].SetActive(false);
                        }

                    }
                    StartCoroutine(ShowGetInfo());
                    isBeginShowEffect = false;
                    effectTimer = 0f;

                }
            }
        }

        float stopSpeed;
        float moveDistance;
        bool m_IsStart = false;
        public float endTime = 2;
        float time = 0;

        float func(float end)
        {
            float num = 0;
            float speed = stantSpeed;
            float cha = ((speed - 100) / endTime * Time.deltaTime);
            while (speed >= 100)
            {
                num += (speed -= cha) * Time.deltaTime;
            }

            return Mathf.Abs(num + end) % 360;
        }


        private float m_AddTime = 15;
        private float m_ReTime = 20;
        private float m_AddSpeed = 1;
        private float m_MaxSpeed = 900;
        private float m_RoteTime = 0.5f;
        private bool m_IsAdd = true;

        float GetDistance(float end)
        {
            float num = m_MaxSpeed;
            float result = 0;
            float retime = m_ReTime;
            while (num >= 10)
            {
                num -= retime;

                if (num > m_MaxSpeed / 3 * 2)
                    retime = 17;

                if (num < m_MaxSpeed / 3)
                {
                    retime = 2;
                }

                result += num;
            }

            return Mathf.Abs(result + end) % 360;
        }

        private float stopTime = 0;

        bool m_isStar = false;

        void ChangeAnimation()
        {
            if (isBeginTick)
            {
                if (m_IsAdd)
                {
                    m_AddSpeed += m_AddTime;
                    if (m_AddSpeed > m_MaxSpeed / 3f)
                        m_AddTime = 20;

                    if (m_AddSpeed > m_MaxSpeed / 3*2)
                        m_AddTime = 25;

                    if (m_AddSpeed > m_MaxSpeed)
                        m_AddSpeed = m_MaxSpeed;
                    if (m_AddSpeed == m_MaxSpeed)
                    {
                        m_IsAdd = false;
                        stopSpeed = GetDistance(endPos);
                    }
                }
                else
                {
                    m_RoteTime -= Time.deltaTime;
                    if (m_RoteTime <= 0)
                    {
                        if (Mathf.Abs(needRotateObj.transform.localEulerAngles.z - stopSpeed) > 0 && Mathf.Abs(needRotateObj.transform.localEulerAngles.z - stopSpeed) < 15)
                        {
                            m_isStar = true;
                        }
                        if(m_isStar)
                            m_AddSpeed -= m_ReTime;

                        if (m_AddSpeed < 0)
                            m_AddSpeed = 0;

                        if (m_AddSpeed > m_MaxSpeed / 3 * 2)
                            m_ReTime = 17;

                        if (m_AddSpeed < m_MaxSpeed / 3)
                        {
                            m_ReTime = 2;
                        }
                    }


                    if (m_AddSpeed < 100)
                    {
                        if (Mathf.Abs(needRotateObj.transform.localEulerAngles.z - endPos) > 0 && Mathf.Abs(needRotateObj.transform.localEulerAngles.z - endPos) < 15)
                        {
                            m_IsAdd = true;
                            m_AddSpeed = 1;
                            m_RoteTime = 0.5f;
                            m_AddTime = 15;
                            m_ReTime = 20;
                            isBeginTick = false;
                            m_isStar = false;
                            RunEndShowEffect();
                            ShowSelfGuangDian();
                            ShowSelfGuangEffect();
                        }
                    }

                }
                needRotateObj.transform.Rotate(0, 0, -m_AddSpeed * Time.deltaTime);
            }
        }


        /// <summary>
        /// 转盘指针控制
        /// </summary>
        void RunIsBegin()
        {

            ChangeAnimation();

            #region old
            if (isBeginTick && false)
            {
                m_time += Time.fixedDeltaTime;
                speed += a;
                if (m_time <= plusSpeedTime)
                {
                    needRotateObj.transform.Rotate(0, 0, -(speed + a) * Time.deltaTime);
                    stantSpeed = speed + a;
                }
                else if (m_time > plusSpeedTime && m_time < contentSpeedTime)
                {
                    needRotateObj.transform.Rotate(0, 0, -stantSpeed * Time.deltaTime);
                }
                else
                {
                    if (stopSpeed == 0)
                        stopSpeed = (stantSpeed - 100) / endTime * Time.deltaTime;
                    if (moveDistance == 0)
                        moveDistance = func(endPos);

                    if (Mathf.Abs(needRotateObj.transform.localEulerAngles.z - moveDistance) > 0 && Mathf.Abs(needRotateObj.transform.localEulerAngles.z - moveDistance) < 30)
                    {
                        m_IsStart = true;
                    }
                    if (!m_IsStart)
                    {
                        needRotateObj.transform.Rotate(0, 0, -stantSpeed * Time.deltaTime);
                        //return;
                    }
                    else
                    {
                        time += Time.deltaTime;
                        stantSpeed -= stopSpeed;
                        needRotateObj.transform.Rotate(0, 0, -stantSpeed * Time.deltaTime);
                        if (stantSpeed <= 100)
                        {
                            stantSpeed = 100;
                        }
                        if (stantSpeed == 100)
                        {
                            if (Mathf.Abs(needRotateObj.transform.localEulerAngles.z - endPos) > 0 && Mathf.Abs(needRotateObj.transform.localEulerAngles.z - endPos) < 15)
                            {
                                stopSpeed = 0;
                                moveDistance = 0;
                                m_IsStart = false;
                                m_time = 0f;
                                speed = 0f;
                                QLoger.ERROR(time);
                                time = 0;
                                isBeginTick = false;
                                RunEndShowEffect();
                                ShowSelfGuangDian();
                                ShowSelfGuangEffect();

                            }
                        }
                    }
                }
            }
            #endregion

            if (needRotateObj.transform.localEulerAngles.z > 30 && needRotateObj.transform.localEulerAngles.z < 90)
            {
                effect_JianTou.transform.localEulerAngles = new Vector3(0, 0, 120);
                //effect_JianTou.transform.Rotate(0, 0, 30);
            }
            else if (needRotateObj.transform.localEulerAngles.z > 90 && needRotateObj.transform.localEulerAngles.z < 150)
            {
                effect_JianTou.transform.localEulerAngles = new Vector3(0, 0, 180);
                //effect_JianTou.transform.Rotate(0, 0, 90);
            }
            else if (needRotateObj.transform.localEulerAngles.z > 150 && needRotateObj.transform.localEulerAngles.z < 210)
            {
                effect_JianTou.transform.localEulerAngles = new Vector3(0, 0, 240);
                //effect_JianTou.transform.Rotate(0, 0, 150);
            }
            else if (needRotateObj.transform.localEulerAngles.z > 210 && needRotateObj.transform.localEulerAngles.z < 270)
            {
                effect_JianTou.transform.localEulerAngles = new Vector3(0, 0, 300);
                //effect_JianTou.transform.Rotate(0, 0, 210);
            }
            else if (needRotateObj.transform.localEulerAngles.z > 270 && needRotateObj.transform.localEulerAngles.z < 330)
            {
                effect_JianTou.transform.localEulerAngles = new Vector3(0, 0, 360);
                //effect_JianTou.transform.Rotate(0, 0, 270);
            }
            else //if (needRotateObj.transform.localEulerAngles.z > 30 && needRotateObj.transform.localEulerAngles.z < 90)
            {
                //effect_JianTou.transform.Rotate(0, 0, 330);
                effect_JianTou.transform.localEulerAngles = new Vector3(0, 0, 60);
            }

        }
    
        /// <summary>
        /// 转盘结束，开始显示特效
        /// </summary>
        void RunEndShowEffect()
        {

            effect_JianTou.SetActive(false);
            runEndAni.SetActive(true);
            runEndAni.GetComponent<Animation>().Play();
            isBeginShowEffect = true;
            if (runEndAni.GetComponent<Animation>().isPlaying)
            {
                runEndAni.SetActive(true);
            }
            else
            {
                runEndAni.SetActive(false);
            }

        }
        /// <summary>
        /// 转完之后的光点特效
        /// </summary>
        /// 
        private bool isBeginShowSelf;
        private float showSelfTimer;
        void ShowSelfGuangEffect()
        {
            if (getAwardIndex == 0)
            {
                guangdianAni.transform.localEulerAngles = new Vector3(0, 0, 60);
            }
            else if (getAwardIndex == 1)
            {
                guangdianAni.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (getAwardIndex == 2)
            {
                guangdianAni.transform.localEulerAngles = new Vector3(0, 0, 300);
            }
            else if (getAwardIndex == 3)
            {
                guangdianAni.transform.localEulerAngles = new Vector3(0, 0, 240);
            }
            else if (getAwardIndex == 4)
            {
                guangdianAni.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (getAwardIndex == 5)
            {
                guangdianAni.transform.localEulerAngles = new Vector3(0, 0, 120);
            }

            guangdianAni.gameObject.SetActive(true);
            guangdianAni.GetComponent<Animation>().Play();
            StartCoroutine(GuangDianAniPlayOver(guangdianAni.GetComponent<Animation>().GetClip("eff_UIActivity_lottery_02_ani").length));

        }
        IEnumerator GuangDianAniPlayOver(float duration)
        {
            yield return new WaitForSeconds(duration);
            for (int i = 0; i < hideGuangDianList.Count; i++)
            {
                hideGuangDianList[i].SetActive(false);
            }
            isBeginShowSelf = true;
        }

        /// <summary>
        /// 抽奖完成光点的显示
        /// </summary>
        void BeginShowSelf()
        {
            if (isBeginShowSelf)
            {
                showSelfTimer += Time.deltaTime;
                if (showSelfTimer > 0 && showSelfTimer <= 0.08)
                {
                    HideGuangDian1.SetActive(false);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    HideGuangDian7.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.08 && showSelfTimer <= 0.16)
                {
                    HideGuangDian2.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    HideGuangDian7.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.16 && showSelfTimer <= 0.24)
                {
                    HideGuangDian3.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    HideGuangDian7.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.24 && showSelfTimer <= 0.32)
                {
                    HideGuangDian4.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    HideGuangDian7.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.32 && showSelfTimer <= 0.4)
                {
                    HideGuangDian5.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    HideGuangDian7.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.4 && showSelfTimer <= 0.48)
                {
                    HideGuangDian6.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian7.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.48 && showSelfTimer <= 0.56)
                {
                    HideGuangDian7.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    //HideGuangDian8.SetActive(true);
                }
                else if (showSelfTimer > 0.56 && showSelfTimer <= 0.64)
                {
                    //HideGuangDian8.SetActive(false);
                    HideGuangDian1.SetActive(true);
                    HideGuangDian2.SetActive(true);
                    HideGuangDian3.SetActive(true);
                    HideGuangDian5.SetActive(true);
                    HideGuangDian4.SetActive(true);
                    HideGuangDian6.SetActive(true);
                    HideGuangDian7.SetActive(true);
                }
                else
                {
                    //HideGuangDian8.SetActive(true);
                    //HideGuangDian1.SetActive(true);
                    //HideGuangDian2.SetActive(true);
                    //HideGuangDian3.SetActive(true);
                    //HideGuangDian5.SetActive(true);
                    //HideGuangDian4.SetActive(true);
                    //HideGuangDian6.SetActive(true);
                    //HideGuangDian7.SetActive(true);
                    showSelfTimer = 0f;
                }
            }
        }

        public void Offset(int i)
        {
            //getAwardIndex = 5;
            //endPos = positions[5];
            //ShowWhitch(5);
            getAwardIndex = i;
            endPos = positions[i];
            ShowWhitch(i);
        }
        #region 特效底板
        private bool isBeginShowEffect = false;
        int firstShow = -1;
        int secondShow1 = -1;
        int secondShow2 = -1;
        int thirdShow1 = -1;
        int thirdShow2 = -1;
        int fourShow = -1;
        public float firstShowTime = 1f;
        public float secondShowTime = 2f;
        public float ThirdShowTime = 3f;
        private float effectTimer = 0f;
        public List<GameObject> effectList = new List<GameObject>();
        #endregion
        /// <summary>
        /// 检查底板显示先后顺序
        /// </summary>
        /// <param name="i"></param>
        void ShowWhitch(int i)
        {
            if (i == 0)
            {
                firstShow = 3;
                secondShow1 = 2;
                secondShow2 = 4;
                thirdShow1 = 1;
                thirdShow2 = 5;

            }
            else if (i == 1)
            {
                firstShow = 4;
                secondShow1 = 3;
                secondShow2 = 5;
                thirdShow1 = 0;
                thirdShow2 = 2;
            }
            else if (i == 2)
            {
                firstShow = 5;
                secondShow1 = 4;
                secondShow2 = 0;
                thirdShow1 = 1;
                thirdShow2 = 3;
            }
            else if (i == 3)
            {
                firstShow = 0;
                secondShow1 = 1;
                secondShow2 = 5;
                thirdShow1 = 2;
                thirdShow2 = 4;
            }
            else if (i == 4)
            {
                firstShow = 1;
                secondShow1 = 0;
                secondShow2 = 2;
                thirdShow1 = 3;
                thirdShow2 = 5;
            }
            else if (i == 5)
            {
                firstShow = 2;
                secondShow1 = 1;
                secondShow2 = 3;
                thirdShow1 = 0;
                thirdShow2 = 4;
            }
            fourShow = i;
        }
        /// <summary>
        /// 显示的到的物品
        /// </summary>
        public IEnumerator ShowGetInfo()
        {
            yield return new WaitForSeconds(0.5f);

            //activity.Model.PickUpAwardReq(activity.Model.awardID);
            maskObj.SetActive(false);
            drawButton.GetComponent<UISprite>().spriteName = "lottery_Button_cj2";
            drawButton.GetComponent<BoxCollider>().enabled = false;
            drawButton.GetComponent<UIButton>().enabled = false;
            string[] btnNames = new string[1];
            btnNames[0] = "确定";
            string getName = awardItemList[getAwardIndex].GetComponent<LuckAwardItem>().awardName;
            int getNum = awardItemList[getAwardIndex].GetComponent<LuckAwardItem>().awardNum;
            if (awardItemList[getAwardIndex].GetComponent<LuckAwardItem>().awardNum == 0)
            {
                activity.LoadPop(WindowUIType.SystemPopupWindow, "获得奖品", "很遗憾，没抽到奖品，下次再抽！", btnNames, OnBtnClick);
            }
            else
            {
                activity.LoadPop(WindowUIType.SystemPopupWindow, "获得奖品", "恭喜您获得：" + getName + "x" + getNum, btnNames, OnBtnClick);
            }


        }

        void OnBtnClick(int index)
        {
            activity.Model.PickUpAwardReq(activity.Model.awardID);
        }

    }
}
