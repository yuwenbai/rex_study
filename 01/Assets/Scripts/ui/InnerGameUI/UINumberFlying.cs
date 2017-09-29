/**
 * @Author JEFF
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MahjongPlayType;
namespace projectQ
{
    public class UINumberFlying : UIViewBase
    {
        #region 组件引用
        public UIGrid Grid;
        public GameObject SubmitBtn;
        public UINumberFlyingItem TemplateItem;

        public UISprite Bg;
        public UILabel TitleLab;
        #endregion

        #region 数据
        private List<UINumberFlyingItem> _ItemList = new List<UINumberFlyingItem>();
        private List<MjXuanPiaoData.CommonData> _Data = new List<MjXuanPiaoData.CommonData>();
        #endregion


        #region Override
        public override void Init()
        {
            if (!NullHelper.IsObjectIsNull(SubmitBtn))
            {
                UIEventListener.Get(SubmitBtn).onClick = OnSubmit;
            }
        }
        private void OnSubmit(GameObject obj)
        {
            if (_Data != null)
            {
                EventDispatcher.FireEvent(GEnum.NamedEvent.UINumberFlyingSubmit, _Data);
            }
            this.Close();
        }

        public override void OnHide()
        {

        }

        public override void OnShow()
        {
            if (NullHelper.IsObjectIsNull(TemplateItem) || NullHelper.IsObjectIsNull(Grid))
            {
                return;
            }

            int width = 0;
            int height = 0;

            string titName = "";

            for (int i = 0; i < _Data.Count; i++)
            {
                UINumberFlyingItem itemControl = GameTools.InstantiatePrefabAndReturnComp<UINumberFlyingItem>(TemplateItem.gameObject, Grid.transform, true, true);
                if (NullHelper.IsObjectIsNull(itemControl))
                {
                    break;
                }
                itemControl.OnPushData(new object[] { _Data[i] });

                titName = _Data[i].TitleName;

                itemControl.gameObject.SetActive(true);
                itemControl.CenterOnTarget();
                GameObjectHelper.NormalizationTransform(itemControl.transform);

                if (width == 0)
                    width = itemControl.Bg.width;
                else
                    width = (width < itemControl.Bg.width ? itemControl.Bg.width : width);

                height += (int)Grid.cellHeight;
            }
            this.Grid.Reposition();

            TitleLab.text = titName;

            for (int i = 0; i < _Data.Count; i++)
            {
                Transform obj = Grid.transform.GetChild(i);
                if (obj == null)
                    continue;
                UINumberFlyingItem itemControl = obj.GetComponent<UINumberFlyingItem>();
                if (NullHelper.IsObjectIsNull(itemControl))
                {
                    break;
                }
                itemControl.ChangeBgSprite(width);
            }

            Bg.width = width + 30;
            Bg.height = height + 110;
        }
        #endregion

        public override void OnPushData(object[] data)
        {
            if (NullHelper.IsObjectIsNull(data))
            {
                return;
            }
            if (!NullHelper.IsInvalidIndex(0, data))
            {
                _Data = (List<MjXuanPiaoData.CommonData>)data[0];
            }
        }

    }
}


