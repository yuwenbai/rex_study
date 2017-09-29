

using System;
/**
* 作者：周腾
* 作用：
* 日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIActivity_old : UIViewBase
    {
        public UIActivityModel_old Model { get { return _model as UIActivityModel_old; } }
        public ActivityLuckDraw luckDrawActivity;
        public NormalActivity normalActivity;
        //public ActivityRight activityRight;
        public UIScrollView activityNameScrollView;
        public UIGrid activityNameGrid;
        public GameObject activityNameItem;
        public List<GameObject> activityNameList = new List<GameObject>();
        public GameObject closeBtn;

		public GameObject root ;


        bool FirstLogin = false;

        void ClosePage(GameObject go)
        {
            if (FirstLogin)
            {
                FirstLogin = false;
                OnItemClick(1);
            }
            else
            {
                //this.Hide();
                this.Close();
            }

        }
        void OnItemClick(int index)
        {
            FirstLogin = false;
            ShowSelectOrNoSelect(index);
        }
        /// <summary>
        /// 设置显示选中的item
        /// </summary>
        /// <param name="index"></param>
        void ShowSelectOrNoSelect(int index)
        {
            for (int i = 0; i < activityNameList.Count; i++)
            {
                if (i == index)
                {
                    activityNameList[i].GetComponent<ActivityNameItem>().ShowSelect();
                    ShowTextureOrTextOrLuckDraw(index);
                }
                else
                {
                    activityNameList[i].GetComponent<ActivityNameItem>().HideSelect();
                }
            }
        }

        #region override
        public override void Init()
        {
			//this.OnItemClick (0);
            StartCoroutine(RefreshPanel());
        }

        IEnumerator RefreshPanel()
        {
            yield return new WaitForSeconds(0.35f);


			//this.OnItemClick (0);
            UIEventListener.Get(closeBtn).onClick = ClosePage;
            if (Model.GetAwardConfigList() == null)
            {
                Model.GetAwardConfigReq(Msg.LotteryTypeDef.Lottery_Game);
            }

			UITools.SetActive (root.gameObject, false);
			//UITools.SetActive (right.gameObject, false);

			//yield return new WaitForEndOfFrame ();
			UITools.SetActive (root.gameObject);
			//UITools.SetActive (right.gameObject);

            EventDispatcher.AddEvent(GEnum.NamedEvent.SysData_Other_Activity_Rsp, OnAcivityRefresh);
        }

        void OnAcivityRefresh(object[] values)
        {
            OnShow();
        }

        public override void OnShow()
        {
			if (MemoryData.SysActivityData.GetActivityList() != null) {
				//luckDrawActivity.GetComponent<ActivityLuckDraw>().RefreshUI();
			}
                
            //activityRight.luckDraw.RefreshUI();
            if (activityNameList.Count > 1)
            {
                return;
            }
            List<ActivityData> dataList = Model.GetActivityList();

            if (dataList == null)
            {
                string[] btnNames = new string[1];
                btnNames[0] = "确定";
                LoadPop(WindowUIType.SystemPopupWindow, "错误提示", "信息显示错误，抱歉！请立即联系客服！", btnNames, OnLuckClick);
            }
            else
            {
                for (int i = 1; i <= dataList.Count; i++)
                {
                    GameObject go = Instantiate(activityNameItem) as GameObject;
                    go.transform.SetParent(activityNameGrid.transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.transform.localEulerAngles = Vector3.zero;

                    go.GetComponent<ActivityNameItem>().InitActivityName(i, dataList[i - 1].ActivityName);
                    
                    activityNameList.Add(go);
                }

                activityNameGrid.Reposition();
                activityNameScrollView.ResetPosition();
                for (int i = 0; i < activityNameList.Count; i++)
                {
                    activityNameList[i].GetComponent<ActivityNameItem>().dele_activityNameClick = OnItemClick;
                }

                OnItemClick(1);
            }
        }
        void OnLuckClick(int index) { }
        public override void OnHide()
        {

        }
        public override void OnPushData(object[] data)
        {
            if (data != null && data.Length > 0)
            {
                string key = data[0] as string;
                if (key == "FirstLogin")
                {
                    this.FirstLogin = true;
                    return;
                }
            }
            this.FirstLogin = false;
        }
        #endregion
        /// <summary>
        /// 根据活动类型(或者ID)，如果是抽奖，就显示抽奖，如果不是抽奖:
        /// 如果没有图片，则只显示文字
        /// 如果图片和文字都有，都显示
        /// 如果没有文字，只显示图片
        /// </summary>
        public void ShowTextureOrTextOrLuckDraw(int index)
        {
            if (index == 0)
            {
                luckDrawActivity.gameObject.SetActive(true);
                normalActivity.gameObject.SetActive(false);
                if (MemoryData.SysActivityData.GetActivityList() != null)
                    luckDrawActivity.GetComponent<ActivityLuckDraw>().RefreshUI();
                //activityRight.ShowLuckDraw();
            }
            else
            {
                string textureUrl = Model.GetActivityList()[index - 1].ResUrl;
                string desc = Model.GetActivityList()[index - 1].ActivityDesc;
                desc = desc.Trim();
                normalActivity.gameObject.SetActive(true);
                luckDrawActivity.gameObject.SetActive(false);
                if (string.IsNullOrEmpty(textureUrl))
                {
                    normalActivity.ShowText(desc);
                }
                else if (string.IsNullOrEmpty(desc))
                {
                    normalActivity.ShowTexture(textureUrl);
                }
                else
                {
                    normalActivity.ShowTextTexture(desc, textureUrl);
                }
                //activityRight.ShowNormalActivity(Model.GetActivityList()[index - 1].ResUrl, Model.GetActivityList()[index - 1].ActivityDesc);
            }

        }
    }
}
