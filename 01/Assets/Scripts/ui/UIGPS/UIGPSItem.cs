/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UI.ScrollViewTool;

namespace projectQ
{
    public class UIGPSItemData
    {
        public GPSData gps;
        public int index;
        public System.Action<UIGPSItemData> onClick;

        public UIGPSItemData(int index,GPSData gps)
        {
            this.index = index;
            this.gps = gps;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(index).Append(". ").Append(this.gps.ToString());
            return sb.ToString();
        }
        public string ToStringLine(bool isLinek)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(index).Append(". ");
            if(isLinek)
            {
                sb.Append(this.gps.ToStringLine(true));
            }
            else
            {
                sb.Append(this.gps.ToString());
            }
            return sb.ToString();
        }
    }

    public class UIGPSItem : ScrollViewItemBase<UIGPSItemData>
    {
        public UILabel label;
        public override void Refresh()
        {
            label.text = this.UIData.ToString();
        }
        private void OnItemClick(GameObject go)
        {
            if (this.UIData != null && this.UIData.onClick != null)
                this.UIData.onClick(this.UIData);
        }
        private void Awake()
        {
            UIEventListener.Get(gameObject).onClick = OnItemClick;
        }
    }
}
