/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace projectQ {

    public class ItemData {
        public long guid ;
        public int id ;
        public int count ;

    }

    public class ItemInfo {
        public int id  ;
        public int count ;
    }

    public class SystemItemData {
        public Dictionary<long,ItemData> _mItems = new Dictionary<long, ItemData>();

        public ItemData this[long idx] {
            get { 
                if(this._mItems.ContainsKey(idx)) {
                    return this._mItems[idx];
                }
                return null;
            }
            set { 
                _mItems [idx] = value;

                //TODO 发送道具变化消息
            }
        }

        public void addItem(long guid , int count)
        {
            var i = this[guid];
            if( i!= null) {
                i.count += count;
                //TODO 发送道具变化消息
            }
        }
            
    }

    #region 内存数据
    public partial class MKey {
        public const string USER_ITEM_DATA = "USER_ITEM_DATA";
    }

    public partial class MemoryData {
        static public SystemItemData ItemData {
            get  {
                SystemItemData itemData = MemoryData.Get<SystemItemData>(MKey.USER_ITEM_DATA);
                if(itemData == null) {
                    itemData = new SystemItemData();
                    MemoryData.Set (MKey.USER_ITEM_DATA, itemData);
                }
                return itemData;
            }
        }
    }
    #endregion
}

