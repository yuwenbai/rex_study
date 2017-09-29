/**
 * @Author lyb
 * 大厅右侧按钮逻辑控制脚本
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIMainRightList : UIMainBase
    {
        public delegate void RightListBtnDelegate(string btnName);
        public RightListBtnDelegate OnClickCallBack;

        /// <summary>
        /// 按钮要打开的面板的名字
        /// </summary>
        public string RightBtnFunctionName;
        /// <summary>
        /// 按钮名字
        /// </summary>
        public UILabel RightBtnLab;
        /// <summary>
        /// 按钮特效1
        /// </summary>
        public GameObject EffectObj1;
        /// <summary>
        /// 按钮特效2
        /// </summary>
        public GameObject EffectObj2;

        void Start() { }

        public void HallRightListInit(UIMainRight.ButtonData bData)
        {
            RightBtnFunctionName = bData.UIName;
            RightBtnLab.text = bData.UIText.Replace("\\n", "\n");

            EffectObj1.SetActive(false);
            if (bData.UIName == "MasterApply")
            {
                //我要当馆长
                EffectObj1.SetActive(true);
            }

            if (bData.UIName == "MyMjHall")
            {
                if (MemoryData.PlayerData.MyPlayerModel.PlayerDataBase.OwnerRoomID > 0)
                {
                    // 馆主
                    RightBtnLab.text = "我的\n棋牌室";
                }
            }

            FunctionShowCheck functionShowCheck = GetComponent<FunctionShowCheck>();
            if(bData.FunctionId != GEnum.FunctionIconEnum.None)
            {
                if (functionShowCheck == null)
                {
                    functionShowCheck = gameObject.AddComponent<FunctionShowCheck>();
                }                    

                functionShowCheck.FunctionId = bData.FunctionId;
                //functionShowCheck.ParentGrid = transform.parent.GetComponent<UIGrid>();
            }
            else
            {
                if (functionShowCheck != null)
                {
                    GameObject.DestroyImmediate(functionShowCheck);
                }
            }
        }

        void OnClick()
        {
            if (OnClickCallBack != null)
            {
                OnClickCallBack(RightBtnFunctionName);
            }
        }
    }
}
