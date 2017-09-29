/**
 * @Author rexzhao
 *
 *
 */

using Msg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class SysPlayerOtherData
    {
        private Dictionary<int, List<int>> ReplayHandCardList = new Dictionary<int, List<int>>(4);
        public void RefreshData(ShowDeskCard data)
        {
            ReplayHandCardList.Clear();
            foreach (var item in data.ShowList)
            {
                ReplayHandCardList.Add(item.SeatID, item.MjList);
            }
        }
        public List<int> GetHandCardListBySeatId(int seatId)
        {
            List<int> HandCardList;
            ReplayHandCardList.TryGetValue(seatId, out HandCardList);
            return HandCardList;
        }
    }

    public partial class MKey
    {
        public const string SYS_PLAYER_OTHER_DATA = "SYS_PLAYER_OTHER_DATA";
    }

    public partial class MemoryData
    {
        static public SysPlayerOtherData PlayerOtherData
        {
            get
            {
                SysPlayerOtherData itemData = MemoryData.Get<SysPlayerOtherData>(MKey.SYS_PLAYER_OTHER_DATA);
                if (itemData == null)
                {
                    itemData = new SysPlayerOtherData();
                    MemoryData.Set(MKey.SYS_PLAYER_OTHER_DATA, itemData);
                }
                return itemData;
            }
        }
    }
}