/**
 * @Author YQC
 *
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using projectQ.chectKey;
using System.IO;

namespace projectQ
{
    public class UICheatKeyModel : UIModelBase
    {
        public UICheatKey UI
        {
            get { return _ui as UICheatKey; }
        }
        public const string PaiSpriteNamePre = "mj_icon_";
        public const string PaiBackSpriteName = "mj_icon_Back";
        
        public List<CheatKeyPai> PaiList = null;

        #region override
        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[] {
                GEnum.NamedEvent.SysUI_CheatKeySelectDataUpdata
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            switch(msgEnum)
            {
                
                case GEnum.NamedEvent.SysUI_CheatKeySelectDataUpdata:
                    UI.OnShow();
                    break;
            }
        }
        #endregion

        private void Awake()
        {
            //CheatKeyManager.Instance.ReadCheatData();
        }
    }



    namespace chectKey
    {
        public class CkData
        {
            public string name;
            private List<int> _paiList;
            public List<int> PaiList
            {
                private set
                {
                    _paiList = value;
                }
                get
                {
                    return _paiList;
                }
            }
            public int MaxPaiNum;

            public CkData(string name,int num)
            {
                this.name = name;
                this.MaxPaiNum = num;

                InitPaiList();
            }
            public List<int> GetList()
            {
                List<int> result = new List<int>();
                if(this.PaiList != null && this.PaiList.Count > 0)
                {
                    for (int i = 0; i < this.PaiList.Count; i++)
                    {
                        if(PaiList[i] > 0)
                        {
                            result.Add(PaiList[i]);
                        }
                    }
                }
                return result;
            }

            public void InitPaiList()
            {
                this._paiList = new List<int>(this.MaxPaiNum);
                for (int i = 0; i < this.MaxPaiNum; i++)
                {
                    this._paiList.Add(-1);
                }
            }

            public bool AddPai(int id = -1)
            {
                if(id != -1)
                {
                    for (int i = 0; i < this.PaiList.Count; i++)
                    {
                        if(PaiList[i] < 0)
                        {
                            PaiList[i] = id;
                            return true;
                        }
                    }
                }

                if (MaxPaiNum == 0)
                {
                    this.PaiList.Add(id);
                    return true;
                }
                else if (this.PaiList.Count < MaxPaiNum)
                {
                    this.PaiList.Add(id);
                    return true;
                }
                

                return false;
            }

            public bool UpdatePai(int index,int id)
            {
                if(PaiList == null)
                {
                    InitPaiList();
                }
                if(PaiList.Count > index)
                {
                    PaiList[index] = id;
                    return true;
                }
                return false;
            }

            public void DeletePai(int index)
            {
                if(PaiList != null && PaiList.Count > index)
                {
                    if(PaiList.Count > this.MaxPaiNum)
                    {
                        PaiList.RemoveAt(index);
                    }
                    else
                    {
                        PaiList[index] = -1;
                    }
                }
            }
            /// <summary>
            /// 删除最后一个
            /// </summary>
            public int DeletePaiLast()
            {
                if(PaiList != null && PaiList.Count > 0)
                {
                    if (this.MaxPaiNum == 0)
                    {
                        PaiList.RemoveAt(PaiList.Count - 1);
                    }
                    else
                    {
                        for (int i = PaiList.Count - 1; i >= 0; i--)
                        {
                            if(PaiList[i] > 0)
                            {
                                DeletePai(i);
                                return i;
                            }
                        }
                    }
                }
                return -1;
            }


            /// <summary>
            /// 取得下一个空牌位置
            /// </summary>
            /// <returns></returns>
            public int GetLastEmtryPai()
            {
                if(this.PaiList != null || this.PaiList.Count > 0)
                {
                    for (int i = 0; i < this.PaiList.Count; i++)
                    {
                        if(this.PaiList[i] < 0)
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }

            public bool IsEmtry()
            {
                if(this.PaiList != null && this.PaiList.Count > 0)
                {
                    for (int i = 0; i < this.PaiList.Count; i++)
                    {
                        if(this.PaiList[i] > 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public bool IsFull()
            {
                if (this.MaxPaiNum == 0) return true;
                if (this.PaiList == null || this.PaiList.Count != MaxPaiNum) return false;

                for (int i = 0; i < this.PaiList.Count; i++)
                {
                    if(this.PaiList[i] == -1)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [Serializable, XmlRoot("CheatKeyData")]
        public class CheatKeyData
        {
            public string Name;
            public string _content;
            public string Content
            {
                set
                {
                    _content = value;
                    if (_content != null)
                    {
                        _content = _content.Trim();
                    }
                    StringToList();
                }
                get { return _content; }
            }
            [XmlIgnore]
            private List<CkData> _contentList;
            [XmlIgnore]
            public List<CkData> ContentList
            {
                get { return _contentList; }
            }
            public void ListToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if(this.ContentList != null && this.ContentList.Count > 0)
                {
                    for (int i = 0; i < this.ContentList.Count; i++)
                    {
                        CkData data = this.ContentList[i];
                        for (int j = 0; j < data.PaiList.Count; j++)
                        {
                            if(data.PaiList[j] > 0)
                            {
                                sb.Append(data.PaiList[j]).Append(',');
                            }
                        }
                        if(sb[sb.Length -1 ] == ',')
                        {
                            sb.Length = sb.Length - 1;
                        }

                        if(i < this.ContentList.Count - 1)
                        {
                            sb.Append(';');
                        }
                    }
                }
                this._content = sb.ToString();
            }
            private void StringToList()
            {
                InitList();
                if (_content != null)
                {
                    var yi = _content.Split(';');
                    for (int i = 0; i < yi.Length; i++)
                    {
                        CkData cktemp = null;
                        if (_contentList.Count > i)
                        {
                            cktemp = _contentList[i];
                        }
                        else
                        {
                            break;
                            //cktemp = new CkData("临时" + i, 0);
                            //_contentList.Add(cktemp);
                        }

                        var pais = yi[i].Split(',');
                        for (int j = 0; j < pais.Length; j++)
                        {
                            int temp = -1;
                            if (int.TryParse(pais[j], out temp))
                            {
                                cktemp.AddPai(temp);
                            }
                        }
                        
                    }
                }
            }
            private void InitList()
            {
                _contentList = new List<CkData>();
                string[] nams = new string[] {
                    "东","南","西","北","手牌1","手牌2","手牌3","手牌4"
                };
                int[] nums = new int[] { 14,13,13,13,0,0,0,0};
                for (int i = 0; i < nams.Length; i++)
                {
                    var cd = new CkData(nams[i], nums[i]);
                    _contentList.Add(cd);
                }
            }

            public bool IsFull()
            {
                for (int i = 0; i < this.ContentList.Count; i++)
                {
                    if (!this.ContentList[i].IsFull()) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 用于数据保存类
        /// </summary>
        [Serializable, XmlRoot("CheatKeyDataList")]
        public class CheatKeyDataList
        {
            public List<CheatKeyData> AllData;
            private string defaultName;
            public string DefaultName
            {
                set {
                    this.defaultName = value;
                }
                get {
                    bool flag = false;
                    if (AllData != null && AllData.Count > 0)
                    {
                        for (int i = 0; i < AllData.Count; i++)
                        {
                            if (AllData[i].Name == defaultName)
                            {
                                flag = true;
                                break;
                            }
                        }

                        if (flag)
                        {
                            return defaultName;
                        }
                        else
                        {
                            return AllData[0].Name;
                        }
                    }
                    return "default";
                }
            }

            public CheatKeyDataList()
            {
                AllData = new List<CheatKeyData>();
            }
        }

        /// <summary>
        /// 牌
        /// </summary>
        public class CheatKeyPai
        {
            private int _key;

            public int Key
            {
                set
                {
                    _key = value;
                    SpriteName = UICheatKeyModel.PaiSpriteNamePre + _key;
                }
                get { return _key; }
            }
            public string SpriteName;
            public int MaxNum = 4;
            public int CurrNum = 4;

            public CheatKeyPai(int key)
            {
                this.Key = key;
            }

            public void ResetNum()
            {
                this.CurrNum = this.MaxNum;
            }
        }

        /// <summary>
        /// 测试码管理器
        /// </summary>
        public class CheatKeyManager : SingletonTamplate<CheatKeyManager>
        {
            /// <summary>
            /// 测试码本地保存的key
            /// </summary>
            public const string CheatKeyDataKey = "CheatKeyDataKey";

            /// <summary>
            /// 麻将牌数量
            /// </summary>
            public const int PaiMaxCount = 42;

            /// <summary>
            /// 作弊码数据 全部
            /// </summary>
            //public Dictionary<string, CheatKeyData> CheatDataListAll = new Dictionary<string, CheatKeyData>();
            public List<CheatKeyData> CheatDataListAll = new List<CheatKeyData>();

            public Action SendCallBack = null;

            #region 牌数据操作
            /// <summary>
            /// 初始化牌数据
            /// </summary>
            private List<CheatKeyPai> GetInitPaiData()
            {
                List<CheatKeyPai> result = new List<CheatKeyPai>();
                for (int i = 1; i <= PaiMaxCount; i++)
                {
                    result.Add(new CheatKeyPai(i));
                }
                return result;
            }

            /// <summary>
            /// 重置麻将牌数据
            /// </summary>
            public void ResetPaiData(ref List<CheatKeyPai> paiList)
            {
                if(paiList == null)
                {
                    paiList = GetInitPaiData();
                }
                else
                {
                    for (int i = 0; i < paiList.Count; i++)
                    {
                        paiList[i].ResetNum();
                    }
                }


                var data = this.GetCheatKeyData(CurrCheatName);
                if (data != null && data.ContentList != null)
                {
                    for (int i = 0; i < data.ContentList.Count; i++)
                    {
                        var ckData = data.ContentList[i];
                        if (ckData != null && ckData.PaiList != null && ckData.PaiList.Count > 0)
                        {
                            for (int j = 0; j < ckData.PaiList.Count; j++)
                            {
                                var pai = ckData.PaiList[j];
                                if (pai > 0 && pai < PaiMaxCount)
                                {
                                    paiList[pai - 1].CurrNum--;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region 作弊数据操作
            /// <summary>
            /// 读取本地数据
            /// </summary>
            public void ReadCheatData()
            {
                CheatDataMapAll_Clear();
                CheatKeyDataList CheatData = PlayerPrefsTools.GetObject<CheatKeyDataList>(CheatKeyDataKey);
                if (CheatData != null)
                {
                    for (int i = 0; i < CheatData.AllData.Count; ++i)
                    {
                        var item = CheatData.AllData[i];
                        CheatDataMapAll_Add(item);
                    }
                    SelectCheatName(CheatData.DefaultName);
                }
                else
                {
                    DefaultData();
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_CheatKeyDataUpdata);
            }

            /// <summary>
            /// 保存数据到本地
            /// </summary>
            public void SaveCheatData()
            {
                CheatKeyDataList CheatData = new CheatKeyDataList();
                CheatData.DefaultName = CurrCheatName;
                for (int i = 0; i < CheatDataListAll.Count; i++)
                {
                    CheatDataListAll[i].ListToString();
                    CheatData.AllData.Add(CheatDataListAll[i]);
                }
                PlayerPrefsTools.SetObject<CheatKeyDataList>(CheatKeyDataKey, CheatData, true);
                if (SendCallBack != null)
                {
                    SendCallBack();
                }
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_CheatKeyDataUpdata);
            }

            /// <summary>
            /// 默认数据
            /// </summary>
            private void DefaultData()
            {
                string defaultData = "10,10,10,11,11,11,14,14,14,15,15,16,16,16;10,10,10,11,11,11,14,14,14,26,26,16,16;10,10,10,11,11,11,14,14,14,26,26,16,16;10,10,10,11,11,11,14,14,14,26,26,16,16;10,11,14,26,16,10,11,14,26,16,10,11,14,26,16;10,11,14,26,16,10,11,14,26,16,10,11,14,26,16;10,11,14,26,16,10,11,14,26,16,10,11,14,26,16;10,11,14,26,16,10,11,14,26,16,10,11,14,26,16";
                CreateUpdateData("default", defaultData);

            }

            /// <summary>
            /// 创建数据
            /// </summary>
            public void CreateUpdateData(string name, string value)
            {
                CheatKeyData data = new CheatKeyData();
                data.Name = name;
                data.Content = value;
                CheatDataMapAll_Add(data);

                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_CheatKeyDataUpdata);
            }

            public void DeleteData(CheatKeyData data)
            {
                if (data == null) return;
                if(CurrCheatData == data)
                {
                    CurrCheatData = null;
                }
                CheatDataListAll.Remove(data);
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_CheatKeyDataUpdata);
            }

            /// <summary>
            /// 根据测试码名字取得数据
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public CheatKeyData GetCheatKeyData(string name = null)
            {
                if(name == null)
                {
                    name = CurrCheatName;
                }
                if (CheatDataListAllFind(name) == -1)
                {
                    CreateUpdateData(name, "");
                }
                return CheatDataListAll[CheatDataListAllFind(name)];
            }
            #endregion

            #region CheatDataMapAll操作

            public int CheatDataListAllFind(string name)
            {
                for (int i=0; i< CheatDataListAll.Count; ++i)
                {
                    if (CheatDataListAll[i].Name == name)
                        return i;
                }
                return -1;
            }
            public void CheatDataMapAll_Add(CheatKeyData value)
            {
                int index = CheatDataListAllFind(value.Name);

                if(index != -1)
                {
                    CheatDataListAll[index] = value;
                }
                else
                {
                    CheatDataListAll.Add(value);
                }
            }

            public void CheatDataMapAll_Clear()
            {
                CheatDataListAll.Clear();
            }
            #endregion

            #region 设置当前数据
            public string CurrCheatName
            {
                get {
                    return CurrCheatData.Name;
                }
            }
            private CheatKeyData _currCheatData;
            public CheatKeyData CurrCheatData
            {
                set { _currCheatData = value; }
                get {
                    if(_currCheatData == null)
                    {
                        if(this.CheatDataListAll != null && this.CheatDataListAll.Count > 0)
                        {
                             _currCheatData = this.CheatDataListAll[0];
                        }
                    }
                    return _currCheatData;
                }
            }
            public CkData CurrSelectCkData = null;
            public int CurrSelectPaiIndex = -1;

            public void SelectCheatName(string name)
            {
                CurrCheatData = GetCheatKeyData(name);
                SelectShouPai(null, -1);
            }
            public void SelectShouPai(CkData data,int index = -1)
            {
                CurrSelectCkData = data;
                CurrSelectPaiIndex = index;
                EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_CheatKeySelectDataUpdata);
            }

            /// <summary>
            /// 选中牌 有没有操作
            /// </summary>
            /// <param name="id"></param>
            public bool SelectPai(int id)
            {
                bool flag = false;
                if(CurrSelectCkData != null)
                {
                    if(CurrSelectPaiIndex < 0)
                    {
                        flag = CurrSelectCkData.AddPai(id);
                    }
                    else
                    {
                        flag = CurrSelectCkData.UpdatePai(CurrSelectPaiIndex,id);
                    }
                }
                if(flag)
                {
                    if(CurrSelectCkData.MaxPaiNum == 0)
                    {
                        if(CurrSelectPaiIndex >= 0 && CurrSelectPaiIndex < CurrSelectCkData.PaiList.Count - 1)
                        {
                            CurrSelectPaiIndex++;
                        }
                        else
                        {
                            int index = CurrSelectCkData.GetLastEmtryPai();
                            CurrSelectPaiIndex = index == -1 ? CurrSelectCkData.PaiList.Count - 1 : index;
                        }
                    }
                    else
                    {
                        if (CurrSelectPaiIndex < CurrSelectCkData.MaxPaiNum - 1)
                        {
                            CurrSelectPaiIndex++;
                        }
                    }

                }
                return flag;
            }

            public void DeleteCurrSelectPai(CkData data, int index = -1)
            {
                if(data != CurrSelectCkData)
                {
                    CurrSelectPaiIndex = index;
                }

                SelectShouPai(data, CurrSelectPaiIndex);
                if (CurrSelectPaiIndex < 0)
                {
                    CurrSelectPaiIndex = data.DeletePaiLast();
                }
                else
                {
                    data.DeletePai(CurrSelectPaiIndex);
                    if (CurrSelectPaiIndex >= 0) CurrSelectPaiIndex--; 
                }
            }

            public void CreateCurrSelectPai(CkData data)
            {
                SelectShouPai(data, -1);
                data.AddPai();
            }
            #endregion

            #region 导入导出
            public string Import(string filePath)
            {
                FileInfo fi = new FileInfo(filePath);
                string error = null;
                if(fi.Exists)
                {
                    string content;
                    using (var fis = fi.OpenText())
                    {
                         content = fis.ReadToEnd();
                    }
                    ImportContent(content);
                }
                else
                {
                    error = "路径错误未找到文件";
                }
                return error;
            }

            private void ImportContent(string content)
            {
                string[] keys = content.Split('\r','\n');
                if(keys.Length == 1)
                {
                    keys = content.Split('\n');
                }
                for (int i = 0; i < keys.Length; i++)
                {
                    string[] item = keys[i].Split('|');
                    if(item.Length == 2)
                    {
                        CreateUpdateData(item[0],item[1]);
                    }
                }
                SaveCheatData();
            }

            public void Export(string filePath)
            {
                if(this.CheatDataListAll != null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < CheatDataListAll.Count; i++)
                    {
                        var item = CheatDataListAll[i];
                        sb.Append(item.Name).Append('|').Append(item.Content);
                        sb.AppendLine();
                    }
                    foreach(var item in this.CheatDataListAll)
                    {
                       
                    }
                    FileInfo fi = new FileInfo(filePath);
                    if(fi.Exists)
                    {
                        fi.Delete();
                    }
                    using (var fis = fi.CreateText())
                    {
                        fis.Write(sb.ToString());
                    }
                }
            }

            #endregion

            #region 发送
            public void SendCode(string name)
            {
                SelectCheatName(name);
                var data = GetCheatKeyData(name);
                try
                {
                    ModelNetWorker.Instance.MjTestReq(MjDataManager.Instance.MjData.curUserData.selfDeskID,
                        data.ContentList[0].GetList()
                        , data.ContentList[1].GetList()
                        , data.ContentList[2].GetList()
                        , data.ContentList[3].GetList()
                        , data.ContentList[4].GetList()
                        , data.ContentList[5].GetList()
                        , data.ContentList[6].GetList()
                        , data.ContentList[7].GetList());
                    SaveCheatData();
                }
                catch
                {
                    QLoger.ERROR("阿萨德");
                }
            }
            #endregion


        }
    }
}