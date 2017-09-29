using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace projectQ
{
    public abstract class BaseXmlBuild
    {
        public Dictionary<string , string> XmlBuildDic = new Dictionary<string, string>();

        public static Dictionary<string, System.Type> XmlTypeMaper = new Dictionary<string, System.Type>();

        public static void Init()
        {
            XmlTypeMaper.Add("AddSheet", typeof(AddSheet));
            XmlTypeMaper.Add("AreaCode", typeof(AreaCode));
            XmlTypeMaper.Add("CompareSheet", typeof(CompareSheet));
            XmlTypeMaper.Add("DetailSheet", typeof(DetailSheet));
            XmlTypeMaper.Add("DetailShow", typeof(DetailShow));
            XmlTypeMaper.Add("FunctionIcon", typeof(FunctionIcon));
            XmlTypeMaper.Add("LinkConf", typeof(LinkConf));
            XmlTypeMaper.Add("MusicSound", typeof(MusicSound));
            XmlTypeMaper.Add("MusicVoice", typeof(MusicVoice));
            XmlTypeMaper.Add("OnGameConfig", typeof(OnGameConfig));
            XmlTypeMaper.Add("PlayRule", typeof(PlayRule));
            XmlTypeMaper.Add("PrefabConfig", typeof(PrefabConfig));
            XmlTypeMaper.Add("RegionUnLock", typeof(RegionUnLock));
            XmlTypeMaper.Add("SelectRegion", typeof(SelectRegion));
            XmlTypeMaper.Add("ServerList", typeof(ServerList));
            XmlTypeMaper.Add("TextConfig", typeof(TextConfig));
            XmlTypeMaper.Add("Tips", typeof(Tips));
            XmlTypeMaper.Add("officialconfig", typeof(officialconfig));
            XmlTypeMaper.Add("shop", typeof(shop));
        }
    }

    public class AddSheet : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _biaozhun;
        public string biaozhun
        {
            get { return XmlBuildDic["biaozhun"]; }
            set { _biaozhun = value; }
        }

        private string _xueliumajiang;
        public string xueliumajiang
        {
            get { return XmlBuildDic["xueliumajiang"]; }
            set { _xueliumajiang = value; }
        }

        private string _xuezhanmajiang;
        public string xuezhanmajiang
        {
            get { return XmlBuildDic["xuezhanmajiang"]; }
            set { _xuezhanmajiang = value; }
        }

        private string _pingdingshanmajiang;
        public string pingdingshanmajiang
        {
            get { return XmlBuildDic["pingdingshanmajiang"]; }
            set { _pingdingshanmajiang = value; }
        }

        private string _jinanmajiang;
        public string jinanmajiang
        {
            get { return XmlBuildDic["jinanmajiang"]; }
            set { _jinanmajiang = value; }
        }

        private string _shandongtuidaohu;
        public string shandongtuidaohu
        {
            get { return XmlBuildDic["shandongtuidaohu"]; }
            set { _shandongtuidaohu = value; }
        }

        private string _sichuanxueliumajiang;
        public string sichuanxueliumajiang
        {
            get { return XmlBuildDic["sichuanxueliumajiang"]; }
            set { _sichuanxueliumajiang = value; }
        }

        private string _sichuanxuezhanmajiang;
        public string sichuanxuezhanmajiang
        {
            get { return XmlBuildDic["sichuanxuezhanmajiang"]; }
            set { _sichuanxuezhanmajiang = value; }
        }

        private string _jiaozuomajiang;
        public string jiaozuomajiang
        {
            get { return XmlBuildDic["jiaozuomajiang"]; }
            set { _jiaozuomajiang = value; }
        }

    }

    public class AreaCode : BaseXmlBuild
    {
        private string _AreaName;
        public string AreaName
        {
            get { return XmlBuildDic["AreaName"]; }
            set { _AreaName = value; }
        }

        private string _AreaID;
        public string AreaID
        {
            get { return XmlBuildDic["AreaID"]; }
            set { _AreaID = value; }
        }

    }

    public class CompareSheet : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Type;
        public string Type
        {
            get { return XmlBuildDic["Type"]; }
            set { _Type = value; }
        }

        private string _MjTypeName;
        public string MjTypeName
        {
            get { return XmlBuildDic["MjTypeName"]; }
            set { _MjTypeName = value; }
        }

    }

    public class DetailSheet : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _biaozhun;
        public string biaozhun
        {
            get { return XmlBuildDic["biaozhun"]; }
            set { _biaozhun = value; }
        }

        private string _other;
        public string other
        {
            get { return XmlBuildDic["other"]; }
            set { _other = value; }
        }

    }

    public class DetailShow : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Name;
        public string Name
        {
            get { return XmlBuildDic["Name"]; }
            set { _Name = value; }
        }

        private string _zhengzhoumajiang;
        public string zhengzhoumajiang
        {
            get { return XmlBuildDic["zhengzhoumajiang"]; }
            set { _zhengzhoumajiang = value; }
        }

        private string _luoyanggangci;
        public string luoyanggangci
        {
            get { return XmlBuildDic["luoyanggangci"]; }
            set { _luoyanggangci = value; }
        }

        private string _xinxiangmajiang;
        public string xinxiangmajiang
        {
            get { return XmlBuildDic["xinxiangmajiang"]; }
            set { _xinxiangmajiang = value; }
        }

        private string _kaifengmajiang;
        public string kaifengmajiang
        {
            get { return XmlBuildDic["kaifengmajiang"]; }
            set { _kaifengmajiang = value; }
        }

        private string _jiaozuomajiang;
        public string jiaozuomajiang
        {
            get { return XmlBuildDic["jiaozuomajiang"]; }
            set { _jiaozuomajiang = value; }
        }

        private string _xueliumajiang;
        public string xueliumajiang
        {
            get { return XmlBuildDic["xueliumajiang"]; }
            set { _xueliumajiang = value; }
        }

        private string _xuezhanmajiang;
        public string xuezhanmajiang
        {
            get { return XmlBuildDic["xuezhanmajiang"]; }
            set { _xuezhanmajiang = value; }
        }

        private string _hongzhongmajiang;
        public string hongzhongmajiang
        {
            get { return XmlBuildDic["hongzhongmajiang"]; }
            set { _hongzhongmajiang = value; }
        }

        private string _jinanmajiang;
        public string jinanmajiang
        {
            get { return XmlBuildDic["jinanmajiang"]; }
            set { _jinanmajiang = value; }
        }

        private string _qingdaomajiang;
        public string qingdaomajiang
        {
            get { return XmlBuildDic["qingdaomajiang"]; }
            set { _qingdaomajiang = value; }
        }

        private string _shandongtuidaohu;
        public string shandongtuidaohu
        {
            get { return XmlBuildDic["shandongtuidaohu"]; }
            set { _shandongtuidaohu = value; }
        }

        private string _luohemajiang;
        public string luohemajiang
        {
            get { return XmlBuildDic["luohemajiang"]; }
            set { _luohemajiang = value; }
        }

        private string _pingdingshanmajiang;
        public string pingdingshanmajiang
        {
            get { return XmlBuildDic["pingdingshanmajiang"]; }
            set { _pingdingshanmajiang = value; }
        }

        private string _weihaimajiang;
        public string weihaimajiang
        {
            get { return XmlBuildDic["weihaimajiang"]; }
            set { _weihaimajiang = value; }
        }

        private string _baojimajiang;
        public string baojimajiang
        {
            get { return XmlBuildDic["baojimajiang"]; }
            set { _baojimajiang = value; }
        }

        private string _xinyangshigongzui;
        public string xinyangshigongzui
        {
            get { return XmlBuildDic["xinyangshigongzui"]; }
            set { _xinyangshigongzui = value; }
        }

        private string _xinyangmantangpao;
        public string xinyangmantangpao
        {
            get { return XmlBuildDic["xinyangmantangpao"]; }
            set { _xinyangmantangpao = value; }
        }

        private string _xinyangbandaoying;
        public string xinyangbandaoying
        {
            get { return XmlBuildDic["xinyangbandaoying"]; }
            set { _xinyangbandaoying = value; }
        }

        private string _henantuidaohu;
        public string henantuidaohu
        {
            get { return XmlBuildDic["henantuidaohu"]; }
            set { _henantuidaohu = value; }
        }

        private string _liuxingzhuanzhuanmajiang;
        public string liuxingzhuanzhuanmajiang
        {
            get { return XmlBuildDic["liuxingzhuanzhuanmajiang"]; }
            set { _liuxingzhuanzhuanmajiang = value; }
        }

        private string _anyangmajiang;
        public string anyangmajiang
        {
            get { return XmlBuildDic["anyangmajiang"]; }
            set { _anyangmajiang = value; }
        }

        private string _zhoukoumajiang;
        public string zhoukoumajiang
        {
            get { return XmlBuildDic["zhoukoumajiang"]; }
            set { _zhoukoumajiang = value; }
        }

        private string _sanmenxiamajiang;
        public string sanmenxiamajiang
        {
            get { return XmlBuildDic["sanmenxiamajiang"]; }
            set { _sanmenxiamajiang = value; }
        }

        private string _puyangmajiang;
        public string puyangmajiang
        {
            get { return XmlBuildDic["puyangmajiang"]; }
            set { _puyangmajiang = value; }
        }

        private string _zhumadianmajiang;
        public string zhumadianmajiang
        {
            get { return XmlBuildDic["zhumadianmajiang"]; }
            set { _zhumadianmajiang = value; }
        }

        private string _nanyangmajiang;
        public string nanyangmajiang
        {
            get { return XmlBuildDic["nanyangmajiang"]; }
            set { _nanyangmajiang = value; }
        }

        private string _xuchangmajiang;
        public string xuchangmajiang
        {
            get { return XmlBuildDic["xuchangmajiang"]; }
            set { _xuchangmajiang = value; }
        }

        private string _jiyuanmajiang;
        public string jiyuanmajiang
        {
            get { return XmlBuildDic["jiyuanmajiang"]; }
            set { _jiyuanmajiang = value; }
        }

        private string _zhoukoudanhunmajiang;
        public string zhoukoudanhunmajiang
        {
            get { return XmlBuildDic["zhoukoudanhunmajiang"]; }
            set { _zhoukoudanhunmajiang = value; }
        }

        private string _zhoukouyingbanmajiang;
        public string zhoukouyingbanmajiang
        {
            get { return XmlBuildDic["zhoukouyingbanmajiang"]; }
            set { _zhoukouyingbanmajiang = value; }
        }

        private string _shangqiumajiang;
        public string shangqiumajiang
        {
            get { return XmlBuildDic["shangqiumajiang"]; }
            set { _shangqiumajiang = value; }
        }

        private string _henanhuashuimajiang;
        public string henanhuashuimajiang
        {
            get { return XmlBuildDic["henanhuashuimajiang"]; }
            set { _henanhuashuimajiang = value; }
        }

        private string _henanzhuanzhuanmajiang;
        public string henanzhuanzhuanmajiang
        {
            get { return XmlBuildDic["henanzhuanzhuanmajiang"]; }
            set { _henanzhuanzhuanmajiang = value; }
        }

        private string _weifangmajiang;
        public string weifangmajiang
        {
            get { return XmlBuildDic["weifangmajiang"]; }
            set { _weifangmajiang = value; }
        }

        private string _hezemajiang;
        public string hezemajiang
        {
            get { return XmlBuildDic["hezemajiang"]; }
            set { _hezemajiang = value; }
        }

        private string _dezhoumajiang;
        public string dezhoumajiang
        {
            get { return XmlBuildDic["dezhoumajiang"]; }
            set { _dezhoumajiang = value; }
        }

        private string _yantaimajiang;
        public string yantaimajiang
        {
            get { return XmlBuildDic["yantaimajiang"]; }
            set { _yantaimajiang = value; }
        }

        private string _linyimajiang;
        public string linyimajiang
        {
            get { return XmlBuildDic["linyimajiang"]; }
            set { _linyimajiang = value; }
        }

        private string _xiaopinghu;
        public string xiaopinghu
        {
            get { return XmlBuildDic["xiaopinghu"]; }
            set { _xiaopinghu = value; }
        }

        private string _sichuanpinghu;
        public string sichuanpinghu
        {
            get { return XmlBuildDic["sichuanpinghu"]; }
            set { _sichuanpinghu = value; }
        }

        private string _jiningmajiang;
        public string jiningmajiang
        {
            get { return XmlBuildDic["jiningmajiang"]; }
            set { _jiningmajiang = value; }
        }

        private string _sichuanxueliumajiang;
        public string sichuanxueliumajiang
        {
            get { return XmlBuildDic["sichuanxueliumajiang"]; }
            set { _sichuanxueliumajiang = value; }
        }

        private string _sichuanxuezhanmajiang;
        public string sichuanxuezhanmajiang
        {
            get { return XmlBuildDic["sichuanxuezhanmajiang"]; }
            set { _sichuanxuezhanmajiang = value; }
        }

        private string _zibomajiang;
        public string zibomajiang
        {
            get { return XmlBuildDic["zibomajiang"]; }
            set { _zibomajiang = value; }
        }

        private string _taianmajiang;
        public string taianmajiang
        {
            get { return XmlBuildDic["taianmajiang"]; }
            set { _taianmajiang = value; }
        }

        private string _zhuabenniao;
        public string zhuabenniao
        {
            get { return XmlBuildDic["zhuabenniao"]; }
            set { _zhuabenniao = value; }
        }

        private string _rizhaomajiang;
        public string rizhaomajiang
        {
            get { return XmlBuildDic["rizhaomajiang"]; }
            set { _rizhaomajiang = value; }
        }

        private string _laiwumajiang;
        public string laiwumajiang
        {
            get { return XmlBuildDic["laiwumajiang"]; }
            set { _laiwumajiang = value; }
        }

        private string _liuxingzhuabenniao;
        public string liuxingzhuabenniao
        {
            get { return XmlBuildDic["liuxingzhuabenniao"]; }
            set { _liuxingzhuabenniao = value; }
        }

        private string _dongyingmajiang;
        public string dongyingmajiang
        {
            get { return XmlBuildDic["dongyingmajiang"]; }
            set { _dongyingmajiang = value; }
        }

        private string _pingdumajiang;
        public string pingdumajiang
        {
            get { return XmlBuildDic["pingdumajiang"]; }
            set { _pingdumajiang = value; }
        }

        private string _binzhoumajiang;
        public string binzhoumajiang
        {
            get { return XmlBuildDic["binzhoumajiang"]; }
            set { _binzhoumajiang = value; }
        }

        private string _liaochengmajiang;
        public string liaochengmajiang
        {
            get { return XmlBuildDic["liaochengmajiang"]; }
            set { _liaochengmajiang = value; }
        }

        private string _hudapai;
        public string hudapai
        {
            get { return XmlBuildDic["hudapai"]; }
            set { _hudapai = value; }
        }

        private string _dahongzhong;
        public string dahongzhong
        {
            get { return XmlBuildDic["dahongzhong"]; }
            set { _dahongzhong = value; }
        }

        private string _zuopaituidaohu;
        public string zuopaituidaohu
        {
            get { return XmlBuildDic["zuopaituidaohu"]; }
            set { _zuopaituidaohu = value; }
        }

        private string _guangdongtuidaohu;
        public string guangdongtuidaohu
        {
            get { return XmlBuildDic["guangdongtuidaohu"]; }
            set { _guangdongtuidaohu = value; }
        }

        private string _jihu;
        public string jihu
        {
            get { return XmlBuildDic["jihu"]; }
            set { _jihu = value; }
        }

        private string _baidajihu;
        public string baidajihu
        {
            get { return XmlBuildDic["baidajihu"]; }
            set { _baidajihu = value; }
        }

        private string _jianhuajipinghu;
        public string jianhuajipinghu
        {
            get { return XmlBuildDic["jianhuajipinghu"]; }
            set { _jianhuajipinghu = value; }
        }

    }

    public class FunctionIcon : BaseXmlBuild
    {
        private string _FunctionID;
        public string FunctionID
        {
            get { return XmlBuildDic["FunctionID"]; }
            set { _FunctionID = value; }
        }

        private string _FunctionName;
        public string FunctionName
        {
            get { return XmlBuildDic["FunctionName"]; }
            set { _FunctionName = value; }
        }

        private string _IsShow;
        public string IsShow
        {
            get { return XmlBuildDic["IsShow"]; }
            set { _IsShow = value; }
        }

        private string _IsIOSShow;
        public string IsIOSShow
        {
            get { return XmlBuildDic["IsIOSShow"]; }
            set { _IsIOSShow = value; }
        }

        private string _IsIosJudgeShow;
        public string IsIosJudgeShow
        {
            get { return XmlBuildDic["IsIosJudgeShow"]; }
            set { _IsIosJudgeShow = value; }
        }

    }

    public class LinkConf : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Name;
        public string Name
        {
            get { return XmlBuildDic["Name"]; }
            set { _Name = value; }
        }

        private string _Key;
        public string Key
        {
            get { return XmlBuildDic["Key"]; }
            set { _Key = value; }
        }

        private string _Type;
        public string Type
        {
            get { return XmlBuildDic["Type"]; }
            set { _Type = value; }
        }

        private string _WX_Type;
        public string WX_Type
        {
            get { return XmlBuildDic["WX_Type"]; }
            set { _WX_Type = value; }
        }

        private string _WX_ShareType;
        public string WX_ShareType
        {
            get { return XmlBuildDic["WX_ShareType"]; }
            set { _WX_ShareType = value; }
        }

        private string _Url;
        public string Url
        {
            get { return XmlBuildDic["Url"]; }
            set { _Url = value; }
        }

        private string _Icon;
        public string Icon
        {
            get { return XmlBuildDic["Icon"]; }
            set { _Icon = value; }
        }

        private string _Title;
        public string Title
        {
            get { return XmlBuildDic["Title"]; }
            set { _Title = value; }
        }

        private string _Desc;
        public string Desc
        {
            get { return XmlBuildDic["Desc"]; }
            set { _Desc = value; }
        }

        private string _ParaInfo;
        public string ParaInfo
        {
            get { return XmlBuildDic["ParaInfo"]; }
            set { _ParaInfo = value; }
        }

    }

    public class MusicSound : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Name;
        public string Name
        {
            get { return XmlBuildDic["Name"]; }
            set { _Name = value; }
        }

        private string _Explain;
        public string Explain
        {
            get { return XmlBuildDic["Explain"]; }
            set { _Explain = value; }
        }

        private string _Detail;
        public string Detail
        {
            get { return XmlBuildDic["Detail"]; }
            set { _Detail = value; }
        }

        private string _remark;
        public string remark
        {
            get { return XmlBuildDic["remark"]; }
            set { _remark = value; }
        }

    }

    public class MusicVoice : BaseXmlBuild
    {
        private string _SoundTypeID;
        public string SoundTypeID
        {
            get { return XmlBuildDic["SoundTypeID"]; }
            set { _SoundTypeID = value; }
        }

        private string _Explain;
        public string Explain
        {
            get { return XmlBuildDic["Explain"]; }
            set { _Explain = value; }
        }

        private string _Mandarin;
        public string Mandarin
        {
            get { return XmlBuildDic["Mandarin"]; }
            set { _Mandarin = value; }
        }

        private string _Localism_HeNan;
        public string Localism_HeNan
        {
            get { return XmlBuildDic["Localism_HeNan"]; }
            set { _Localism_HeNan = value; }
        }

        private string _Localism_ShanDong;
        public string Localism_ShanDong
        {
            get { return XmlBuildDic["Localism_ShanDong"]; }
            set { _Localism_ShanDong = value; }
        }

    }

    public class OnGameConfig : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Key;
        public string Key
        {
            get { return XmlBuildDic["Key"]; }
            set { _Key = value; }
        }

        private string _Type;
        public string Type
        {
            get { return XmlBuildDic["Type"]; }
            set { _Type = value; }
        }

        private string _Value;
        public string Value
        {
            get { return XmlBuildDic["Value"]; }
            set { _Value = value; }
        }

        private string _Desc;
        public string Desc
        {
            get { return XmlBuildDic["Desc"]; }
            set { _Desc = value; }
        }

    }

    public class PlayRule : BaseXmlBuild
    {
        private string _ConfigId;
        public string ConfigId
        {
            get { return XmlBuildDic["ConfigId"]; }
            set { _ConfigId = value; }
        }

        private string _RuleName;
        public string RuleName
        {
            get { return XmlBuildDic["RuleName"]; }
            set { _RuleName = value; }
        }

        private string _RuleBase;
        public string RuleBase
        {
            get { return XmlBuildDic["RuleBase"]; }
            set { _RuleBase = value; }
        }

        private string _RuleForeign;
        public string RuleForeign
        {
            get { return XmlBuildDic["RuleForeign"]; }
            set { _RuleForeign = value; }
        }

        private string _RuleSpecial;
        public string RuleSpecial
        {
            get { return XmlBuildDic["RuleSpecial"]; }
            set { _RuleSpecial = value; }
        }

        private string _RuleFinish;
        public string RuleFinish
        {
            get { return XmlBuildDic["RuleFinish"]; }
            set { _RuleFinish = value; }
        }

        private string _RuleDetail;
        public string RuleDetail
        {
            get { return XmlBuildDic["RuleDetail"]; }
            set { _RuleDetail = value; }
        }

    }

    public class PrefabConfig : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Path;
        public string Path
        {
            get { return XmlBuildDic["Path"]; }
            set { _Path = value; }
        }

        private string _CacheCount;
        public string CacheCount
        {
            get { return XmlBuildDic["CacheCount"]; }
            set { _CacheCount = value; }
        }

        private string _Description;
        public string Description
        {
            get { return XmlBuildDic["Description"]; }
            set { _Description = value; }
        }

    }

    public class RegionUnLock : BaseXmlBuild
    {
        private string _RegionID;
        public string RegionID
        {
            get { return XmlBuildDic["RegionID"]; }
            set { _RegionID = value; }
        }

        private string _RegionName;
        public string RegionName
        {
            get { return XmlBuildDic["RegionName"]; }
            set { _RegionName = value; }
        }

        private string _IsUnLock;
        public string IsUnLock
        {
            get { return XmlBuildDic["IsUnLock"]; }
            set { _IsUnLock = value; }
        }

        private string _RegionSelect;
        public string RegionSelect
        {
            get { return XmlBuildDic["RegionSelect"]; }
            set { _RegionSelect = value; }
        }

        private string _SelectPosition;
        public string SelectPosition
        {
            get { return XmlBuildDic["SelectPosition"]; }
            set { _SelectPosition = value; }
        }

    }

    public class SelectRegion : BaseXmlBuild
    {
        private string _RegionID;
        public string RegionID
        {
            get { return XmlBuildDic["RegionID"]; }
            set { _RegionID = value; }
        }

        private string _CityID;
        public string CityID
        {
            get { return XmlBuildDic["CityID"]; }
            set { _CityID = value; }
        }

        private string _RegionName;
        public string RegionName
        {
            get { return XmlBuildDic["RegionName"]; }
            set { _RegionName = value; }
        }

        private string _CityName;
        public string CityName
        {
            get { return XmlBuildDic["CityName"]; }
            set { _CityName = value; }
        }

    }

    public class ServerList : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _Ip;
        public string Ip
        {
            get { return XmlBuildDic["Ip"]; }
            set { _Ip = value; }
        }

        private string _Port;
        public string Port
        {
            get { return XmlBuildDic["Port"]; }
            set { _Port = value; }
        }

        private string _Name;
        public string Name
        {
            get { return XmlBuildDic["Name"]; }
            set { _Name = value; }
        }

        private string _sType;
        public string sType
        {
            get { return XmlBuildDic["sType"]; }
            set { _sType = value; }
        }

    }

    public class TextConfig : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _KEY;
        public string KEY
        {
            get { return XmlBuildDic["KEY"]; }
            set { _KEY = value; }
        }

        private string _TYPE;
        public string TYPE
        {
            get { return XmlBuildDic["TYPE"]; }
            set { _TYPE = value; }
        }

        private string _Value;
        public string Value
        {
            get { return XmlBuildDic["Value"]; }
            set { _Value = value; }
        }

        private string _remark;
        public string remark
        {
            get { return XmlBuildDic["remark"]; }
            set { _remark = value; }
        }

    }

    public class Tips : BaseXmlBuild
    {
        private string _tips_id;
        public string tips_id
        {
            get { return XmlBuildDic["tips_id"]; }
            set { _tips_id = value; }
        }

        private string _game_type;
        public string game_type
        {
            get { return XmlBuildDic["game_type"]; }
            set { _game_type = value; }
        }

        private string _tips_content;
        public string tips_content
        {
            get { return XmlBuildDic["tips_content"]; }
            set { _tips_content = value; }
        }

    }

    public class officialconfig : BaseXmlBuild
    {
        private string _ID;
        public string ID
        {
            get { return XmlBuildDic["ID"]; }
            set { _ID = value; }
        }

        private string _PlayID;
        public string PlayID
        {
            get { return XmlBuildDic["PlayID"]; }
            set { _PlayID = value; }
        }

        private string _PlayName;
        public string PlayName
        {
            get { return XmlBuildDic["PlayName"]; }
            set { _PlayName = value; }
        }

        private string _UrlRes;
        public string UrlRes
        {
            get { return XmlBuildDic["UrlRes"]; }
            set { _UrlRes = value; }
        }

    }

    public class shop : BaseXmlBuild
    {
        private string _GoodsID;
        public string GoodsID
        {
            get { return XmlBuildDic["GoodsID"]; }
            set { _GoodsID = value; }
        }

        private string _ItemID;
        public string ItemID
        {
            get { return XmlBuildDic["ItemID"]; }
            set { _ItemID = value; }
        }

        private string _ItemNum;
        public string ItemNum
        {
            get { return XmlBuildDic["ItemNum"]; }
            set { _ItemNum = value; }
        }

        private string _GoodsName;
        public string GoodsName
        {
            get { return XmlBuildDic["GoodsName"]; }
            set { _GoodsName = value; }
        }

        private string _MoneyType;
        public string MoneyType
        {
            get { return XmlBuildDic["MoneyType"]; }
            set { _MoneyType = value; }
        }

        private string _MoneyNum;
        public string MoneyNum
        {
            get { return XmlBuildDic["MoneyNum"]; }
            set { _MoneyNum = value; }
        }

        private string _Notes;
        public string Notes
        {
            get { return XmlBuildDic["Notes"]; }
            set { _Notes = value; }
        }

        private string _Sort;
        public string Sort
        {
            get { return XmlBuildDic["Sort"]; }
            set { _Sort = value; }
        }

        private string _IsrealGoods;
        public string IsrealGoods
        {
            get { return XmlBuildDic["IsrealGoods"]; }
            set { _IsrealGoods = value; }
        }

        private string _ImageRes;
        public string ImageRes
        {
            get { return XmlBuildDic["ImageRes"]; }
            set { _ImageRes = value; }
        }

        private string _ButtonDesc;
        public string ButtonDesc
        {
            get { return XmlBuildDic["ButtonDesc"]; }
            set { _ButtonDesc = value; }
        }

        private string _ShopID;
        public string ShopID
        {
            get { return XmlBuildDic["ShopID"]; }
            set { _ShopID = value; }
        }

        private string _Discount;
        public string Discount
        {
            get { return XmlBuildDic["Discount"]; }
            set { _Discount = value; }
        }

        private string _BuyInfo;
        public string BuyInfo
        {
            get { return XmlBuildDic["BuyInfo"]; }
            set { _BuyInfo = value; }
        }

    }

}