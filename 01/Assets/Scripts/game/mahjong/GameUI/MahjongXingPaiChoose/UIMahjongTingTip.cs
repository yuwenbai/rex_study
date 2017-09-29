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
    public class UIMahjongTingTip : UIViewBase
    {
        public override void Init()
        {
            EventDispatcher.AddEvent(GEnum.NamedEvent.EMjControlSetUIClose, SetClose);
            IniData();
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {

        }

        protected override void OnClose()
        {
            EventDispatcher.RemoveEvent(GEnum.NamedEvent.EMjControlSetUIClose, SetClose);
            base.OnClose();
        }

        private void SetClose(object[] obj)
        {
            this.Close();
        }

        public UIGrid grid_ShowTing = null;
        public Transform showTingParent = null;
        public GameObject item_ShowTing = null;
        public GameObject[] tingTips = null;

        //List<UIMahjongShowTing> showTingList = new List<UIMahjongShowTing>();
        Dictionary<int, UIMahjongShowTing> showTingDic = new Dictionary<int, UIMahjongShowTing>();

        public void RefreshTingNum(int mjCode, int cutDownNum)
        {
            if (showTingDic.ContainsKey(mjCode))
            {
                showTingDic[mjCode].RefreshShowTing(cutDownNum);
            }
        }

        public void RefreshTingTip(MjTingInfo curTingInfo)
        {
            if (showTingDic != null && showTingDic.Count > 0)
            {
                ClearData();
                SetData(curTingInfo);
            }
        }


        private void IniData()
        {
            MjTingInfo info = MjDataManager.Instance.MjData.ProcessData.curTingInfo;

            if (info == null)
            {
                //close
                ClearData();

                showTingParent.gameObject.SetActive(false);
                grid_ShowTing.gameObject.SetActive(false);
            }
            else
            {
                //show
                SetData(info);
            }
        }

        private void ClearData()
        {
            for (int i = 0; i < tingTips.Length; i++)
            {
                tingTips[i].SetActive(false);
                tingTips[i].transform.SetParent(showTingParent);
            }
            grid_ShowTing.transform.DestroyChildren();
            showTingDic.Clear();
        }

        private void SetData(MjTingInfo info)
        {
            for (int i = 0; i < tingTips.Length; i++)
            {
                tingTips[i].SetActive(false);
                tingTips[i].transform.SetParent(showTingParent);
            }
            grid_ShowTing.transform.DestroyChildren();

            for (int i = 0; i < tingTips.Length; i++)
            {
                tingTips[i].transform.SetParent(grid_ShowTing.transform);
                tingTips[i].SetActive(true);
            }


            int huCount = info.huCodeNum.Count;
            List<MjTingInfoModel> modelList = new List<MjTingInfoModel>();
            for (int i = 0; i < huCount; i++)
            {
                MjTingInfoModel item = new MjTingInfoModel(info.huCode[i], info.someNum[i], info.huCodeNum[i]);
                modelList.Add(item);
            }

            MjDataManager.Instance.SortTingInfoModel(modelList, huCount <= 8);

            int showCount = Mathf.Min(huCount, 8);

            for (int i = 0; i < showCount; i++)
            {
                SetObjToTing(modelList[i].mjCode, modelList[i].maxOdd, modelList[i].restCount);
            }


            showTingParent.gameObject.SetActive(true);
            grid_ShowTing.gameObject.SetActive(true);
            grid_ShowTing.Reposition();
        }




        private void SetObjToTing(int mjCode, int someNum, int restNum)
        {
            GameObject obj = UITools.CloneObject(item_ShowTing, grid_ShowTing.gameObject);
            UIMahjongShowTing showTing = obj.GetComponent<UIMahjongShowTing>();
            showTing.IniShowTing(mjCode, someNum, restNum);
            showTingDic.Add(mjCode, showTing);
            obj.SetActive(true);
        }

    }

}

