/**
 * @Author lyb
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    /// <summary>
    /// 官方试玩群属性
    /// </summary>
    public class OfficialGroupInfo
    {
        /// <summary>
        /// 官方试玩群ID
        /// </summary>
        public int GroupID;
        /// <summary>
        /// 麻将玩法名字
        /// </summary>
        public string MjGameName;
        /// <summary>
        /// 麻将玩法ID
        /// </summary>
        public int MjGameID;
        /// <summary>
        /// 试玩群信息网页URL
        /// </summary>
        public string PageUrl;
    }
    
    public class SysOfficialGroupData
    {
        /// <summary>
        /// 点击更多的时候调用的url
        /// </summary>
        public string OffcialPageIndex;

        private List<OfficialGroupInfo> _OfficialGroupList = new List<OfficialGroupInfo>();
        public List<OfficialGroupInfo> OfficialGroupList
        {
            get{
                return _OfficialGroupList;
            }
        }
        
        /// <summary>
        /// 官方试玩群数据填充
        /// </summary>
        public void OfficialGroup_Update(List<Msg.OfficialGroupInfo> OGroupList)
        {
            _OfficialGroupList = new List<OfficialGroupInfo>();
            foreach (Msg.OfficialGroupInfo gList in OGroupList)
            {
                OfficialGroupInfo gInfo = new OfficialGroupInfo();
                gInfo.GroupID = gList.GroupID;
                gInfo.MjGameName = gList.MjGameName;
                gInfo.MjGameID = gList.MjGameID;
                gInfo.PageUrl = gList.PageUrl;

                _OfficialGroupList.Add(gInfo);
            }
        }        
    }

    #region 内存数据------------------------------------------------------------------------------

    public partial class MKey
    {
        public const string USER_OFFICIAL_GROUP_DATA = "USER_OFFICIAL_GROUP_DATA";
    }

    public partial class MemoryData
    {
        static public SysOfficialGroupData OfficialGroupData
        {
            get
            {
                SysOfficialGroupData groupData = MemoryData.Get<SysOfficialGroupData>(MKey.USER_OFFICIAL_GROUP_DATA);
                if(groupData == null)
                {
                    groupData = new SysOfficialGroupData();
                    MemoryData.Set (MKey.USER_OFFICIAL_GROUP_DATA, groupData);
                }
                return groupData;
            }
        }
    }

    #endregion -----------------------------------------------------------------------------------
}