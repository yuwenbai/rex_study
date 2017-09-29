/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class SysArea
    {
        public int id;
        public string name;
        public int level;
        public int parent;
        public List<SysArea> subArea;

        public SysArea(int id,string name,int level,int parent)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.parent = parent;
        }
    }

    public class SysAreaData
    {
        //全部地区列表
        private List<SysArea> _areaList;
        public List<SysArea> AreaList
        {
            set
            {
                _areaList = value;
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysData_AreaList_Update, _areaList);
            }
            get
            {
                return _areaList;
            }
        }
        public Dictionary<int, SysArea> AreaMap;


        public void AreaListInterpreter(List<SysArea> list)
        {
            List<SysArea> result = new List<SysArea>();
            Dictionary<int, SysArea> map = new Dictionary<int, SysArea>();
            for(int i = 0; i < list.Count; ++i)
            {
                map.Add(list[i].id, list[i]);
            }

            for (int i = 0; i<list.Count; ++i)
            {
                if(list[i].level == 0)
                {
                    result.Add(list[i]);
                }
                else
                {
                    if(map.ContainsKey(list[i].parent))
                    {
                        if (map[list[i].parent].subArea == null)
                            map[list[i].parent].subArea = new List<SysArea>();
                        map[list[i].parent].subArea.Add(list[i]);
                    }
                }
            }
            this.AreaList = result;
            this.AreaMap = map;
        }
    }
}