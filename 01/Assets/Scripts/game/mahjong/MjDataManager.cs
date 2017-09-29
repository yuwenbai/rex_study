/**
 * @Author Xin.Wang
 *
 *
 */
using System.Collections;
using System.Collections.Generic;
using MahjongPlayType;

public class MjChangeThreeData
{
    public int seatID;
    public List<int> getCodeList;
}


public class MjScore
{
    public int score;
    public int seatID;
    //for balance
    public bool isDianPao;
    public bool isChengBao;

    private void SetData(int seatID, int score)
    {
        this.seatID = seatID;
        this.score = score;
    }

    public MjScore(int seatID, int score)
    {
        SetData(seatID, score);
    }

    public MjScore(int seatID, int score, bool isDianpao, bool isChengbao)
    {
        SetData(seatID, score);
        this.isDianPao = isDianpao;
        this.isChengBao = isChengbao;
    }
}



//麻将单局结算 
public class MjDetaildedScore
{
    public int score = -1;
    public int scoreType = -1;              //分的类型，从XML读取字符串
    public List<int> seatID = null;

    //new
    public int huType = -1;
    public List<int> huPaiTypeList = new List<int>();
    public List<MjDetaildedSpecial> huSpecialList = new List<MjDetaildedSpecial>();

    public MjDetaildedScore(int score, int scoreType, List<int> seatID, int huType, List<int> paiType, List<MjDetaildedSpecial> specialList)
    {
        this.score = score;
        this.scoreType = scoreType;
        this.seatID = seatID;

        this.huType = huType;
        this.huPaiTypeList = paiType;
        this.huSpecialList = specialList;
    }

}

//结算时 特殊分数类型
public class MjDetaildedSpecial
{
    public int specialType = -1;
    public int value = -1;
}


public class MjPai
{
    public int mjCode;
    public int mjCodeType;                  //对应 EnumMjCodeType 或者 EnumMjSpecialType
    public int seatID;                      //对应碰或者直杠的座位号

    public EnumMjCodeType codeType;         //必有 每个牌都有自己的本身type
    public EnumMjSpecialType specialType = EnumMjSpecialType.Null;

    public MjPai(int mjCode, int mjCodeType)
    {
        this.mjCode = mjCode;
        this.mjCodeType = mjCodeType;

        this.codeType = (EnumMjCodeType)mjCodeType;
    }


    public EnumMjSpecialType SetListSpecialType(int specialType)
    {
        this.specialType = (EnumMjSpecialType)specialType;
        return this.specialType;
    }

}

public class MjPlayTimes
{
    public int MjType;
    public int WinBouts;
    public int TotalBouts;
    public MjPlayTimes(int mjType, int winBouts, int totalBouts)
    {
        this.MjType = mjType;
        this.WinBouts = winBouts;
        this.TotalBouts = totalBouts;
    }
}


//最佳牌型 
public class BestMjRecord
{
    public int mjType;                      //玩法类型
    public int oddsCount;                   //番数
    public List<MjPai> mjList = new List<MjPai>();              //牌的列表（手牌+碰杠吃听）
    public List<MjPai> specialList;         //特殊牌列表
    public List<int> scoreChange;           //一一对应的每个玩家的分数记录
    public List<int> paiType;               //胡牌的类型

    public int selfSeatID = -1;             //当前局玩家自己的座位号
    public List<string> playerNameList;     //一一对应的玩家名称


    public EnumMjSpecialType specailType = EnumMjSpecialType.Null;                      //本局特殊玩法类型
    public List<int> specialCardID = new List<int>();                                  //本局特殊玩法牌ID
    //根据这两个参数  初始化牌 
    //调用方法 CardHelper.GetHunSpriteName((int)(specailType), mjType);

    public BestMjRecord(int mjType, int oddsCount, List<MjPai> mjList, List<MjPai> specialList, List<int> scoreList, List<int> paiType)
    {
        this.mjType = mjType;
        this.oddsCount = oddsCount;
        this.mjList = mjList;
        this.specialList = specialList;
        this.scoreChange = scoreList;
        this.paiType = paiType;
        SetMjlistType();
    }


    public void SetMjlistType()
    {
        for (int i = 0; i < specialList.Count; i++)
        {
            specialCardID.Add(specialList[i].mjCode);
            if (specailType == EnumMjSpecialType.Null)
            {
                specailType = (EnumMjSpecialType)specialList[i].mjCodeType;
            }
        }


        for (int i = 0; i < mjList.Count; i++)
        {
            if (specialList != null && specialList.Count > 0)
            {
                for (int j = 0; j < specialList.Count; j++)
                {
                    if (mjList[i].mjCode == specialList[j].mjCode)
                    {
                        specailType = mjList[i].SetListSpecialType(specialList[j].mjCodeType);
                    }
                }
            }
        }
    }

    public BestMjRecord()
    {

    }
}

public class MjBureauDetialInfo
{
    public MjBureauInfo bureauInfo = null;              //对局基本信息
    public BestMjRecord bestRecord = null;              //最佳战绩
    public List<int> dianPaoCount = null;               //点炮次数
    public List<int> zimoCount = null;                  //自摸次数
    public int maxOddCount = -1;                        //最大番数
    public List<int> maxOddSeat = null;                 //最大番数的玩家座位号

    public MjBureauDetialInfo(MjBureauInfo bureauInfo, BestMjRecord bestRecord,
        List<int> dianPaoCount, List<int> zimoCount, List<int> maxOddSeat, int maxOddCount)
    {
        this.bureauInfo = bureauInfo;
        this.bestRecord = bestRecord;
        this.dianPaoCount = dianPaoCount;
        this.zimoCount = zimoCount;
        this.maxOddCount = maxOddCount;
        this.maxOddSeat = maxOddSeat;
    }
}

public class MjBureauInfo
{
    public int nBureauCount;                    //第几局
    public List<MjScore> detailList;            //每一局的积分信息
    public List<int> nWinSeatID;                //赢家座位号    
    public bool isGaming;                       //是否还在进行中

    public MjBureauInfo(int count, List<MjScore> detail, List<int> winSeat)
    {
        this.nBureauCount = count;
        this.detailList = detail;
        this.nWinSeatID = winSeat;
    }

}


/// <summary>
///玩家称号
/// </summary>
public class MjTitleInfo
{
    public int score;
    public List<int> titleList;
    public int seatID;

    public MjTitleInfo(int score, List<int> title, int seatID)
    {
        this.score = score;
        this.titleList = title;
        this.seatID = seatID;
    }
}


public struct MjPaiKou
{
    public bool canPeng;
    public bool canGang;
    public bool canTing;
    public bool canHu;
    public bool canZiMo;
    public bool canChi;
    public bool canMingLou;
    public bool canTingGang;
    public bool canPiao;
    public bool canCiHu;            //次胡
}


public class MjTingInfo
{
    public int tingCode;
    public List<int> huCode = new List<int>();    //当前没张听的牌胡哪些
    public List<int> huCodeNum = new List<int>(); //当前每张胡的牌剩多少
    public List<int> someNum = new List<int>(); //没涨胡的牌番数

    public MjTingInfo(int code, List<int> huCode, List<int> huCodeNum, List<int> someNum)
    {
        this.tingCode = code;
        this.huCode = huCode;
        this.huCodeNum = huCodeNum;
        this.someNum = someNum;
    }

    public void RefreshCount(int mjCode, int loseNum)
    {
        if (huCode != null && huCode.Contains(mjCode))
        {
            int index = huCode.IndexOf(mjCode);
            huCodeNum[index] -= loseNum;
        }
    }

}

public class MjTingInfoModel
{
    public int mjCode;
    public int restCount;
    public int maxOdd;

    public MjTingInfoModel(int cardID, int odds, int rest)
    {
        this.mjCode = cardID;
        this.restCount = rest;
        this.maxOdd = odds;
    }

}


public class MjActionCodeChi
{
    public int chiCode = -1;
    public int chiIndex = -1;

    public int selfCode1 = -1;
    public int selfCodeIndex1 = -1;
    public int selfCode2 = -1;
    public int selfCodeIndex2 = -1;

    public List<int> chiList = new List<int>();
}

public class MjReconnedCheck
{
    public EnumMjSpecialCheck checkType = EnumMjSpecialCheck.Null;
    public EnumMjSelectSubType subType = EnumMjSelectSubType.MjSelectSubType_NoOperation;
    public List<int> checkNeedValue = new List<int>();

    public MjReconnedCheck(int checkType, int subType, List<int> checkValue)
    {
        this.checkType = (EnumMjSpecialCheck)checkType;
        this.subType = (EnumMjSelectSubType)subType;
        this.checkNeedValue = checkValue;
    }

}

public class MjChangeScore
{
    public List<MjScore> socreList = null;
    public int showType = -1;

    public MjChangeScore(List<MjScore> scoreList, int showType)
    {
        this.socreList = scoreList;
        this.showType = showType;
    }
}

/// <summary>
/// 表演功能尸体类
/// </summary>
public class MjPerformanceData
{
    public int curPerformanceSeatID = 0;                    //当前正在表演的座位号
    public bool isPutPerformance = false;                   //当前是否是出牌表演 
    public List<int> seatID = new List<int>();              //每个人的座位号 
    public List<int> perforValue = new List<int>();         //每个人的状态
    public bool isHunDiao = false;

    public MjPerformanceData(List<int> seat, List<int> value, bool isPut, int curSeatID)
    {
        if (seat != null && value != null
            && seat.Count == value.Count)
        {
            seatID = seat;
            perforValue = value;
            curPerformanceSeatID = curSeatID;
            isPutPerformance = isPut;
        }
    }

    public MjPerformanceData(int seat, int value)
    {
        seatID.Add(seat);
        perforValue.Add(value);
    }
}

/// <summary>
/// 玩家选牌数据类
/// </summary>
public class MjPaiChooseData
{
    public class PaiItemData
    {
        public int paiID;
        public bool curChooseState = false;
    }

    public EnumChooseType chooseStateType = EnumChooseType.Single;          //默认单选
    public MjGamePaiChooseType chooseType = MjGamePaiChooseType.Null;       //玩法类型
    public List<PaiItemData> paiList = new List<PaiItemData>();
}




#region 新的特殊玩法的重连结构

//麻将重连的时候需要检查的特殊玩法类型 
public enum EnumMjSpecialCheck
{
    Null,
    MjBaseCheckType_XiaPao = 1,                     //下跑
    MjBaseCheckType_Lack = 2,                       //定缺
    MjBaseCheckType_ChangeThree = 3,                //换三张
    MjBaseCheckType_BuyHorse = 4,			        //买马
    MJBaseCheckType_XiaPaoZi = 5,                   //下炮子
    MjBaseCheckType_XiaYu = 6,                      //下鱼
    MjBaseCheckType_ZhaMa = 7,                      //扎马
    MjBaseCheckType_ZhuaNiao = 8,                   //抓鸟
    MjBaseCheckType_PiaoHu = 9,                     //飘胡
    MjBaseCheckType_PiaoGang = 10,                  //飘杠
    MjBaseCheckType_PiaoJin = 11,                   //飘金
    MjBaseCheckType_PiaoSu = 12,                    //飘素
    MjBaseCheckType_PaoKou = 13,                    //跑扣
    MjBaseCheckType_PaoTou = 14,                    //跑头
    MjBaseCheckType_XiaPiao = 15,                   //下漂
    MjBaseCheckType_XiaMa = 16,                     //下码
    MjBaseCheckType_XiaBang = 17,                        //下绑
    MjBaseCheckType_MingDa = 18,                    //明打
    MjBaseCheckType_XiaYuNingXia = 19,                //宁夏下鱼子


    MjBaseCheckType_KanNaoMo,                      //坎牌 闹庄 末留
    MjBaseCheckType_LiangYi,                        //亮一张
    MjBaseCheckType_LiangSiDaYi,                    //亮四打一
    MjBaseCheckType_SiDunSiDing,                    //死蹲死顶
    MjBaseCheckType_ChuMenDuan,                     //出门断
    MjBaseCheckType_XinXiaPiao,                     //新下跑
}


public enum EnumMjSelectSubType
{
    MjSelectSubType_NoOperation = 0,             //未操作
    MjSelectSubType_WAIT_NoSelect = 1,          // 未选择等待
    MjSelectSubType_WAIT_Select = 2,            // 已选择等待
    MjSelectSubType_RESULT_Select = 3,	        // 已确定结果
}


public class MjReconnedGameSpecial
{
    public MjReconnedInfoPao paoInfo = null;
    public MjReconnedInfoPaozi paoziInfo = null;
    public MjReconnedInfoQue queInfo = null;
    public MjReconnedInfoChangeThree changeThreeInfo = null;
    public MjReconnedInfoXiayu xiayuInfo = null;
}



/// <summary>
/// 公共类
/// </summary>
public class MjReconnedParseCommmon
{
    public int seatID;          //座位号
    public bool chooseState;    //是否已经选择

    public MjReconnedParseCommmon(int seatID, bool chooseState)
    {
        this.seatID = seatID;
        this.chooseState = chooseState;
    }
}

public class MjReconnedInfoBase
{
    public List<MjReconnedCellBase> cellList = new List<MjReconnedCellBase>();
    private MjReconnedCellBase selfCell = null;


    public MjReconnedCellBase GetSelfCell(int selfSeatID)
    {
        if (selfCell == null)
        {
            for (int i = 0; i < cellList.Count; i++)
            {
                if (cellList[i].commonData != null && cellList[i].commonData.seatID == selfSeatID)
                {
                    selfCell = cellList[i];
                    break;
                }
            }
        }

        return selfCell;
    }

}


public class MjReconnedCellBase
{
    public MjReconnedParseCommmon commonData = null;            //通用数据
}


#region 下跑
public class MjReconnedParsePao
{
    public int defaultValue;        //默认下多少
    public int chooseValue;         //选择下多少

    public MjReconnedParsePao(int defValue, int choValue)
    {
        this.defaultValue = defValue;
        this.chooseValue = choValue;
    }
}

public class MjReconnedCellPao : MjReconnedCellBase
{
    public MjReconnedParsePao parseData = null;                 //跑与炮使用数据

}

public class MjReconnedInfoPao : MjReconnedInfoBase
{
    public EnumMjSelectSubType subState = EnumMjSelectSubType.MjSelectSubType_NoOperation;     //当前状态
    public List<int> paoValues = new List<int>();                       //可以下多少


    public MjReconnedInfoPao(int subType, List<int> paoValues)
    {
        subState = (EnumMjSelectSubType)subType;
        if (paoValues != null)
        {
            this.paoValues = paoValues;
        }
    }

}


#endregion

#region 下炮

public class MjReconnedCellPaozi : MjReconnedCellBase
{
    public MjReconnedParsePao parseData = null;                 //跑与炮使用数据
}

public class MjReconnedInfoPaozi : MjReconnedInfoBase
{
    public EnumMjSelectSubType subState = EnumMjSelectSubType.MjSelectSubType_NoOperation;     //当前状态
    public List<int> paoziValues = new List<int>();                      //可以下多少

    public MjReconnedInfoPaozi(int subType, List<int> paoValues)
    {
        subState = (EnumMjSelectSubType)subType;
        if (paoValues != null)
        {
            this.paoziValues = paoValues;
        }
    }
}


#endregion

#region 定缺
public class MjReconnedParseQue
{
    public int defaultType;                             //默认定缺类型
    public int chooseType;                              //选择类型

    public MjReconnedParseQue(int defValue, int choValue)
    {
        this.defaultType = defValue;
        this.chooseType = choValue;
    }
}

public class MjReconnedCellQue : MjReconnedCellBase
{
    public MjReconnedParseQue parseData = null;
}

public class MjReconnedInfoQue : MjReconnedInfoBase
{
    public EnumMjSelectSubType subState = EnumMjSelectSubType.MjSelectSubType_NoOperation;     //当前状态

    public MjReconnedInfoQue(int subType)
    {
        subState = (EnumMjSelectSubType)subType;
    }
}

#endregion

#region 换三张
public class MjReconnedParseChangeThree
{
    public List<int> defaultList = new List<int>();     //推荐的三张

    public MjReconnedParseChangeThree(List<int> defList)
    {
        if (defList != null)
        {
            defaultList = defList;
        }
    }
}

public class MjReconnedCellChangeThree : MjReconnedCellBase
{
    public MjReconnedParseChangeThree parseData = null;
}


public class MjReconnedInfoChangeThree : MjReconnedInfoBase
{
    public EnumMjSelectSubType subState = EnumMjSelectSubType.MjSelectSubType_NoOperation;     //当前状态
    public EnumClockType clockType;                                               //转换方式

    public MjReconnedInfoChangeThree(int subType, int clockType)
    {
        subState = (EnumMjSelectSubType)subType;
        this.clockType = (EnumClockType)clockType;
    }

}

#endregion

#region 下258鱼

public class MjReconnedCellXiayu : MjReconnedCellBase
{
    public MjReconnedParsePao parseData = null;                 //下跑 下炮子 下鱼使用的数据
}

public class MjReconnedInfoXiayu : MjReconnedInfoBase
{
    public EnumMjSelectSubType subState = EnumMjSelectSubType.MjSelectSubType_NoOperation;     //当前状态
    public List<int> yuValues = new List<int>();                       //可以下多少

    public MjReconnedInfoXiayu(int subType, List<int> canValues)
    {
        subState = (EnumMjSelectSubType)subType;
        if (canValues != null)
        {
            this.yuValues = canValues;
        }
    }
}


#endregion


#endregion


public enum EnumMjOpenMaType
{
    Null,

    ZhaMa = 1,                          //扎马
    YiMaQuanZhong,                      //一码全中
    MaiMa,                              //买马
    ZhuaNiao,                           ///抓鸟
    ZhuaMa,                             //抓马
    JiangMa,                            //奖马
    BaoZhaMa,                           //爆炸马
}


public enum EnumMjIconTipType
{
    Tip_Que_Wan = 1,                        //定缺 万
    Tip_Que_Tiao = 2,                       //定缺 条
    Tip_Que_Tong = 3,                       //定缺 筒
    Tip_Dealer,                            //庄家
    Tip_Nao,                                //闹庄
    Tip_Dun,                                //死蹲
    Tip_Ding,                               //死顶
}



public enum EnumMjBeforeDispatch
{
    Null,
    ISXiaPao = 1,                           //当前局是否有下跑
    ISGangDaiPao = 2,                       //当前局是否有杠带跑
    ISGaozhuang = 3,                        //当前局是否有高庄
    ISlLiSi = 4,                            //当前局是否有立四
    ISChangeFlower = 5,                     //当前是否需要补花
    ISMingLouCancel = 6,                          //当前局是否可以明楼(废弃)
    ISLiangLiu = 7,                         //当前局是否有量六
    ISXiaPaoZi = 8,                         //是否有下炮子
    ISFangMao = 9,                          //是否放毛
    ISDuoHu = 10,                            //是否多胡
    ISCheckTing = 11,                        //是否默认查听
    ISXiaYu = 12,                           //是否有下鱼
    IsNoSortAfterDeal = 13,                  //发牌后是否不进行默认排序
    IsDaiCaiShen = 14,                        //是否带财神
    IsMingLouLiangTing = 15,                         //明楼是否是亮听不明牌
    IsCloseAnGang,                                 //暗杠是否全部扣死(false 亮一个 true 全扣死

    IsLiangSiDaYi                           //self 亮四打一 
}


public enum EnumMjNewDeskResult
{
    MjError_Success = 0,
    MjError_FangKaNotEnough = 1,    // 桌卡不够
    MjError_ExistRoom = 2       // 已经有房间
}


public enum EnumMjOpAction
{
    Null,
    MjOp_PutMj = 1,
    MjOp_Peng = 2,
    MjOp_Gang = 3,
    MjOp_Ting = 4,
    MjOp_HuPai = 5,
    MjOp_Pass = 6,
    MjOp_Chi = 7,
    MjOp_Mao = 8,
    MjOp_Du = 9,
    MjOp_Minglou = 10,
    MjOp_TingGang = 11,     //听杠


    MjOp_Cancel,    //self
    MjOp_Zimo,      //self
    MjOp_Guafeng,   //self 刮风
    MjOp_Xiayu,     //self 下雨
    MjOp_ChangeFlower,    //self 补花
    MjOp_ChangeMao,    //self 补毛
    MjOp_Biaoyan,       //self 表演

    MjOp_HuPiaoGenPiao,     //self 胡漂 跟漂
    MjOp_CiHu,              //self 次胡
    MjOp_PiCi,              //self 皮次
    MjOp_Minglao,           //self 明捞
    MjOp_MinglouMu,         //self 明楼（木）
    MjOp_AnlouShou,         //self 暗搂（手）
    MjOp_AnlouMu,           //self 暗楼 (木）
    MjOp_NiuPaiBuHua,       //扭牌补花
    MJOp_SiJiaMaiMa,        //四家买马
    MjOp_Minglv,            //self 明缕
}


public enum EnumMjOpActionTingType
{
    Tip = 1,                    //单纯标志查听

    Normal = 2,                 //普通查听
    Minglou = 3,                //明楼查听
}

public enum EnumMjHuType
{
    Null,
    MjHuType_Gun = 1,               //点炮
    MjHuType_Self = 2,              //自摸
    MjHuType_HaiDiLao = 3,          //海底捞月
    MjHuType_QiangGangHu = 4,       //抢杠胡
    MjHuType_GangShangHua = 5,      //杠上开花 
    MjHuType_GunMany = 6,           //一炮多响
    MjHuType_HuJiaoZhuanYi = 7,      //呼叫转移

    MjHuType_PiCi = 8,              //皮次
    MjHuType_CiHu = 9,              //次胡
}

public enum EnumMjScroeChangeType
{
    Null = 1,
    ScoreChangeFollow = 2,          //跟庄
    ScoreChangeTuishui,                  //退税


    ScoreChangeTongbu,              //积分同步
}

public enum EnumMjBuyhorseStateType
{
    BuyHorseNull = 1,               //没中
    BuyHorseWin = 2,               //买中赢家(买中： )
    BuyHorseLose = 3,               //买中输家
}



//麻将的手牌类型
public enum EnumMjCodeType
{
    Code_Null,
    Code_Hands = 1,
    Code_Gang_An = 2,
    Code_Gang_Zhi = 3,
    Code_Gang_Bu = 4,
    Code_Peng = 5,
    Code_Chi = 6,
    Code_Mao = 7,
    Code_Niu = 8,
}

/// <summary>
/// 麻将controller的摆放类型
/// </summary>
public enum EnumMjControlerShowType
{
    Null = 0,
    ShowTypeLeft = 1,
    ShowTypeMidle = 2,
    ShowTypeRight = 3
}


public enum EnumMjDeskAction
{
    MjRoom_Ready = 1,
    MjRoom_Quit = 2,
    MjRoom_Close = 3,
    MjRoom_Trust = 4,           // 托管
    MjRoom_UnTrust = 5,         // 取消托管
    MjRoom_Agree = 6,           // 同意
    MjRoom_DisAgree = 7,        // 不同意
    MjRoom_AskClose = 8,        // 请求解散
    MjRoom_Continue = 9,	    // 继续游戏
}

public enum EnumMjGetType
{
    MjGetType_Common = 1,
    MjGetType_Gang = 2
}

//麻将换三张的换牌方式
public enum EnumClockType
{
    ClockType_OppoSite = 1,
    ClockType_ClockWise = 2,
    ClockType_CounterClockWise = 3
}

public enum EnumConfirmType
{
    ConfirmType_WAN = 1,        //万
    ConfirmType_TIAO = 2,        //条
    ConfirmType_TONG = 3		    //筒
}


//for UI
//手牌的排列组合方式
public enum EnumMjListCardType
{
    //Normal = 1,         //默认
    Number = 1,         //按照数量
    Combine = 2         //按照组合优先级
}


public enum EnumMjHuaType
{
    Null = 0,
    Wan = 1,
    Tiao = 2,
    Tong = 3,
    Feng = 4,
    Hua = 5
}

public enum EnumMjStandingType
{
    Close,
    Standing,
    Open,
    Back,
}


//麻将标志类型
public enum MjBiaoType
{
    Null = 0,
    Mo = 1,
    Liang,
    Lai,
    Jin,
    Hun,
    Ci,
    Cai,
    Mao,
    Kan,
    Liu,
    Nao,
    Moliu,
    Hua,
    Niu,
    Gui,

    Tip = 16,
}

public enum MjHandAnimType
{
    Null = 0,
    Put,
    ChiPengGang,
    Flower,
    Hu,
    FlipPai,
    KouPai,
    TuiPai
}

enum MjGameState
{
    MjGameState_Idle = 0,                   //空闲状态
    MjGameState_BeforeGameStart = 1,        //游戏开始前
    MjGameState_BeforeDispatch = 2,         //发牌前
    MjGameState_BeforeOpWait = 3,           //前等待操作
    MjGameState_StandingCards = 4,          //立牌
    MjGameState_OpWait = 5,                 //后等待操作
    MjGameState_GameStart = 6,              //游戏开始
    MjGameState_GameIng = 7,                //游戏中
    MjGameState_Settlement = 8,             //结算
    MjGameState_SettlementShow = 9,         //结算展示
    MjGameState_GameIngIdle = 10,		    //游戏中空闲状态
}


public enum MjGamePlayerType
{
    PlayerFour,                         //四人模式
    PlayerThree,                        //三人模式
    PlayerTwo,                          //两人模式
}

public enum MjGamePaiChooseType
{
    Null,
    Nachi,                              //杠后拿吃
    LiangYiZhang,                       //发四亮一
}


//麻将音效
public enum EnumMjMusicType
{
    WanStart = 1000,            //一万
    TongStart = 1009,           //一筒
    TiaoStart = 1018,           //一条

    FengDong = 1027,            //东
    FengNan = 1028,              //南
    FengXi = 1029,             //西
    FengBei = 1030,             //北
    FengZhong = 1031,
    FengFa = 1032,
    FengBai = 1033,

    ActionChi = 1034,
    ActionPeng = 1035,
    ActionGang = 1036,
    ActionTing = 1037,
    ActionHu = 1038,
    ActionZimo = 1039,
    ActionPiCi = 1041,          //皮次（次胡
    ActionCiHu = 1200,          //杠次
    ActionGuafeng = 1046,
    ActionXiaYu = 1047,
    ActionMinglouShou = 1061,   //明搂
    ActionMinglouMu = 1052,     //明楼（木
    ActionMinglao = 1062,
    ActionAnLouMu = 1301,       //暗楼（木
    ActionAnLouShou = 1302,     //暗搂（手
    ActionMinglv = 1303,     //明缕

    StateGenZhuang = 1040,
    StateThink = 1042,
    StateLianLiu = 1044,
    StateYiTiaoLong = 1045,


    HuCiHu = 1041,
    HuGangShangKaiHua = 1043,
}



namespace projectQ
{
    //麻将数据DataModel
    public partial class MahongDataModel
    {
        #region BoardData(局信息，Gameover才清理)
        public bool isInMahjongScene = false;
        public bool isReconned = false;

        public bool gameStartNum = false;

        public MjGamePlayerType playerType = MjGamePlayerType.PlayerFour;
        public List<int> playerUISeat = new List<int>();

        //目前暂时只支持查看自己回放
        public CurUserData curUserData
        {
            get
            {
                if (_curUserData == null)
                    _curUserData = new CurUserData();
                return _curUserData;
                //if (FakeReplayManager.Instance.ReplayState)
                //{
                //    if (_curViewUserData == null)
                //        _curViewUserData = new CurUserData();
                //    return _curViewUserData;
                // }
                //else
                //{
                //    if (_curUserData == null)
                //        _curUserData = new CurUserData();
                //    return _curUserData;
                //}
            }
        }

        private CurUserData _curViewUserData = null;
        private CurUserData _curUserData = null;

        public class CurUserData
        {
            //桌号
            private int _selfDeskID = 0;
            public int selfDeskID
            {
                get { return _selfDeskID; }
                set { _selfDeskID = value; }
            }

            //座位号
            private int _selfSeatID = 0;
            public int selfSeatID
            {
                get { return _selfSeatID; }
                set
                {
                    _selfSeatID = value;
                    //fire
                    EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSetSlefSeatID);
                }
            }
            //当前用户id
            private ulong _selfUserID;
            public ulong selfUserID
            {
                get { return _selfUserID; }
                set { _selfUserID = value; }
            }
        }

        public class GameProcessCloseDesk
        {
            public int startSeatID = -1;
            public int[] seatState = new int[5] { 0, 0, 0, 0, 0 };
            //0 choose 1 true 2 false

            public void SetSeatState(int seatID, int state)
            {
                seatState[seatID] = state;
            }

            public int GetSeatState(int seatID)
            {
                return seatState[seatID];
            }
        }
        //解散房间

        public GameProcessCloseDesk processClose = null;

        /// <summary>
        /// 核心流程 过程中不清理
        /// </summary>
        public partial class CoreProcessData
        {
            public Dictionary<string, MahjongPorcessBase> processDic = new Dictionary<string, MahjongPorcessBase>();
        }

        private CoreProcessData _coreData = null;
        public CoreProcessData CoreData
        {
            get
            {
                if (_coreData == null)
                {
                    _coreData = new CoreProcessData();
                }
                return _coreData;
            }
        }

        #endregion

        #region ProcessData(每轮信息，每次Gamestart清理)

        #region 类定义

        public class GameProcessQue
        {
            public EnumMjHuaType defaultQueType = EnumMjHuaType.Null;                   //服务器默认缺类型
            public EnumMjHuaType[] queResultArray = new EnumMjHuaType[4];               //每个人的定缺结果(座位号--)

            public void SetQueType(int uiSeatID, int queType)
            {
                queResultArray[uiSeatID] = (EnumMjHuaType)queType;
            }

            public EnumMjSpecialType CheckProcessQue(int mjCode, int uiSeatID)
            {
                bool isQue = false;
                EnumMjHuaType curType = CardHelper.GetHuaTypeByID(mjCode);
                if (curType == queResultArray[uiSeatID])
                {
                    isQue = true;
                }
                return isQue ? EnumMjSpecialType.Que : EnumMjSpecialType.Null;
            }

        }

        public class GameProcessChangeThree
        {
            public EnumClockType changeClock = EnumClockType.ClockType_OppoSite;    //当前的换牌方式
            public List<int> defaultList = new List<int>();                         //服务器默认换的类型
            public List<int> chooseList = new List<int>();                          //客户端选择
            public List<int> getList = new List<int>();                             //换得的三张牌 
        }

        public class GameProcessXiaPao
        {
            public int defaultValue = -1;                                           //默认的下跑数
            public List<int> choosableValues = new List<int>();                     //可选的下跑数

            public int[] choosenValues = new int[4];                                      //每个人的下跑结果
        }


        public class GameProcessRulerResult
        {
            public int rulerResult = ConstDefine.MJ_PK_NULL;
            public List<int> gangList = null;
            public List<MjTingInfo> tingList = null;
            public int seatID = -1;
            public List<int> ciList = null;

        }

        public class GameProcessActionCode
        {
            public int pengCode = -1;
            public int huCode = -1;
            public List<int> gangList = new List<int>();
            public List<int> ciList = new List<int>();
            public List<MjActionCodeChi> chiCodeList = new List<MjActionCodeChi>();
            public Dictionary<int, MjTingInfo> tingInfoDic = new Dictionary<int, MjTingInfo>();

            public MjActionCodeChi curCodeChi = null;
        }


        #endregion


        //断线重连
        public class GameProcessReconned
        {
            //base info
            public long userID;
            public MjDeskInfo deskInfo = null;
            public List<MjPlayerInfo> playerInfoList = new List<MjPlayerInfo>();
            public int gameState = 0;
            //before dispacher
            public List<bool> beforeDispatchValues = new List<bool>();

            //game start
            public int dealerSeat;
            public List<int> rollNums = new List<int>();
            public int getMjSeat = -1;
            public int getOffset = -1;
            public int allCount = -1;
            public int curRestCount = -1;
            public List<List<int>> maoGroup = null;

            //standing
            public List<MjStandingPlateData> standingData = new List<MjStandingPlateData>();

            //special
            public List<MjReconnedCheck> checkSpecial = new List<MjReconnedCheck>();
            public MjReconnedGameSpecial specailCheck = new MjReconnedGameSpecial();

            public int kannaomoState = 0;
            public int xuanPiaoState = 0;

            //打出去
            public List<int>[] putCardArray = new List<int>[5];
            //胡牌
            public List<int>[] huCardArray = new List<int>[5];
            //补花
            public List<int>[] flowerArray = new List<int>[5];
            //独立牌区
            public List<int>[] independentArray = new List<int>[5];
            //手牌 连带碰杠
            public List<MjPai>[] handCardArray = new List<MjPai>[5];
            //手牌中的最后一张牌
            public int lastHandCard = -1;
            //单纯的手牌
            public List<int>[] downListArray = new List<int>[5];
            //换三张玩法里面需要扣出三张
            public void ChangeThreeLose(int seatID, List<int> loseThree)
            {
                List<int> dowList = this.downListArray[seatID];
                if (loseThree != null && dowList != null)
                {
                    for (int i = 0; i < loseThree.Count; i++)
                    {
                        int index = dowList.IndexOf(loseThree[i]);
                        if (index < 0)
                        {
                            dowList.RemoveAt(0);
                        }
                        else
                        {
                            dowList.RemoveAt(index);
                        }

                    }
                }
            }
            //需要被翻过来的听的index
            public int[] putCloseIndexArray = new int[5] { -1, -1, -1, -1, -1 };
            public bool hasFlyOne = false;
            public List<int> minglouTingList = new List<int>();


            //playOprate
            public bool showBaoci = false;

            //wait data
            public int waitType = -1;
            public int waitSeatID = -1;
            public int waitTime = -1;
            public int waitLastPutSeatID = -1;

            //ruler info
            public bool canFangMao = false;
            public int rulerResult = -1;
            public List<MjTingInfo> tingInfo = new List<MjTingInfo>();
            public List<int> chiList = new List<int>();
            public List<int> gangList = new List<int>();
            public int nMjCode = -1;
            public List<int> ciList = new List<int>();

            //ting or hu info
            public List<int> tingSeatID = new List<int>();
            public MjTingInfo confirmTingInfo = null;
            public MjPerformanceData performData = null;

            //close room
            public bool isAskClose = false;
            public int closeTime = 0;
            public int startAskSeatID = 0;
            public List<bool> isAnswer = new List<bool>();
            public List<bool> isAgree = new List<bool>();
        }

        public partial class GameProcessData
        {
            public int dealerSeatID = -1;                   //庄家座位号 
            public int getMjSeatID = -1;                    //开始拿牌的座位号
            public int getOffset = -1;                      //剩余几墩
            public List<int> rollNums = new List<int>();    //发牌色子
            public int curRestMjCount;                      //当前剩余麻将总数 
            private int _curAmount = -1;
            public int curMjAmount                          //麻将的总数
            {
                get
                {
                    return _curAmount;
                }
                set { _curAmount = curRestMjCount = value; }
            }


            public List<List<int>> maoGroup = null;         //当局游戏放毛的组
            public bool chooseLimit = false;                //选择放毛限制 只能选择不同的牌(临时 回头服务器通知)
            public void SetGameStartInfo(int dealerSeatID, int getMjSeatID, int getOffset, List<int> rollNums, List<List<int>> maoGroup)
            {
                this.dealerSeatID = dealerSeatID;
                this.getMjSeatID = getMjSeatID;
                this.getOffset = getOffset;
                this.rollNums = rollNums;

                this.maoGroup = maoGroup;
                if (maoGroup != null && maoGroup.Count > 0)
                {
                    chooseLimit = maoGroup.Count > 1;
                }
            }

            public int lastPutSeatID = -1;
            public int lastPutCode = -1;


            //ruler result
            public GameProcessRulerResult processRulerResult = null;
            public void IniProcessRulerResult()
            {
                processRulerResult = new GameProcessRulerResult();
            }


            //CardAction data(牌口数据)
            public GameProcessActionCode processActionCode = null;

            public void IniProcessActionCode()
            {
                processActionCode = new GameProcessActionCode();
            }

            public MjTingInfo curTingInfo = null;                               //听口 用来做听状态监测


            //special data
            //EnumMjBeforeDispatch
            public Dictionary<int, bool> stateBeforeDispatch = new Dictionary<int, bool>();
            private bool hasFlyOne = false;                                      //是否飞一
            public bool HasFlyOne
            {
                get { return hasFlyOne; }
                set
                {
                    if (value)
                    {
                        //fire
                    }
                    hasFlyOne = value;
                }
            }


            public void SetStateInfo(List<bool> checkValues)
            {
                for (int i = 0; i < checkValues.Count; i++)
                {
                    int curType = i + 1;
                    bool value = checkValues[i];
                    if (!stateBeforeDispatch.ContainsKey(curType))
                    {
                        stateBeforeDispatch.Add(curType, value);
                    }
                    else
                    {
                        stateBeforeDispatch[curType] = value;
                    }
                }
            }

            public bool CheckStateInfo(EnumMjBeforeDispatch type, bool needHulue = true)
            {
                if (type == EnumMjBeforeDispatch.ISCheckTing && needHulue)
                {
                    return true;
                }

                if (type == EnumMjBeforeDispatch.IsLiangSiDaYi)
                {
                    return MjDataManager.Instance.LSDY_CheckPlayOprate();
                }


                int curType = (int)type;
                if (stateBeforeDispatch.ContainsKey(curType))
                {
                    return stateBeforeDispatch[curType];
                }

                return false;
            }

            #region Special(定缺)
            public GameProcessQue processQue = null;

            public void IniProcessQue()
            {
                if (processQue == null)
                {
                    processQue = new GameProcessQue();
                }
            }

            #endregion

            #region Special(换三张)
            public GameProcessChangeThree processChangeThree = null;

            #endregion

            #region Special(下跑)
            public GameProcessXiaPao processXiaPao = null;

            #endregion

            #region 断线重连
            public GameProcessReconned processReconned = null;

            #endregion

            public Dictionary<string, MahjongPlayOprateBase> playOprateDic = new Dictionary<string, MahjongPlayOprateBase>();
        }


        private GameProcessData _processData = null;
        public GameProcessData ProcessData
        {
            get
            {
                if (_processData == null)
                {
                    _processData = new GameProcessData();
                }
                return _processData;
            }
        }

        public void ClearProcessData()
        {
            _processData = null;
        }

        #endregion

    }



    public partial class MjPlayOprateType
    {

    }

    public partial class MjProcessOprateType
    { }

    public partial class MjDataManager : SingletonTamplate<MjDataManager>
    {
        private MahongDataModel _mjDataModel = null;
        public MahongDataModel MjData
        {
            get
            {
                if (_mjDataModel == null)
                {
                    _mjDataModel = new MahongDataModel();
                }
                return _mjDataModel;
            }
        }

        /// <summary>
        /// 回放的标志
        /// </summary>
        public bool GameStateHuiFang
        {
            get
            {
                return FakeReplayManager.Instance.ReplayState;
            }
        }


        public void ClearLastGame()
        {
            MjData.ClearProcessData();
        }

        public void ClearAllData()
        {
            _mjDataModel = null;
        }

        /// <summary>
        /// 检测当前是否有玩法数据类型存在 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckPlayDataOprateContain(string key)
        {
            return MjData.ProcessData.playOprateDic.ContainsKey(key);
        }



        #region 游戏开局管理
        public int SetPlayerEnterDesk(MjDeskInfo joinDeskInfo, ulong userID)
        {
            MjData.curUserData.selfUserID = userID;
            MjData.curUserData.selfDeskID = joinDeskInfo.deskID;

            return MjData.curUserData.selfDeskID;
        }

        //设置游戏开始时的信息
        public void SetProcessGameStart(int delaerID, int getMjSeat, int getOffset, List<int> rollNum, int amount, int deskID, List<List<int>> maoGroup)
        {
            if (deskID == MjData.curUserData.selfDeskID)
            {
                MjData.ProcessData.SetGameStartInfo(delaerID, getMjSeat, getOffset, rollNum, maoGroup);
                this.SetAllCount(amount);
            }

        }

        //设置游戏开始前的一些状态标识
        public void SetProcessStateInfo(List<bool> checkValue)
        {
            MjData.ProcessData.SetStateInfo(checkValue);
        }


        public void SetAllCount(int count)
        {
            MjData.ProcessData.curMjAmount = count;
        }

        public void SetCurCount(int curCount)
        {
            MjData.ProcessData.curRestMjCount = curCount;
        }

        public void CutDownCurRestCount(int count)
        {
            if (MjData.ProcessData.curRestMjCount >= count)
            {
                MjData.ProcessData.curRestMjCount -= count;
            }
        }

        public int GetIndependentCount()
        {
            int dependentCount = 0;
            if (MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISlLiSi))
            {
                dependentCount = 4;
            }

            if (MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.ISLiangLiu))
            {
                dependentCount = 6;
            }

            if (MjData.ProcessData.CheckStateInfo(EnumMjBeforeDispatch.IsLiangSiDaYi))
            {
                dependentCount = 4;
            }


            return dependentCount;
        }


        //设置缺门状态
        public void SetQueType(int uiSeatID, int queType)
        {
            MjData.ProcessData.IniProcessQue();
            MjData.ProcessData.processQue.SetQueType(uiSeatID, queType);
        }

        public EnumMjHuaType GetQueType(int uiSeatID)
        {
            EnumMjHuaType queType = EnumMjHuaType.Null;
            if (MjData.ProcessData.processQue != null)
            {
                queType = MjData.ProcessData.processQue.queResultArray[uiSeatID];
            }
            return queType;
        }

        #endregion


        #region 牌口管理
        public void SetRulerResult(int seatID, int rulerResult, List<int> gangList, List<MjTingInfo> tingInfo, List<int> ciList)
        {
            MjData.ProcessData.IniProcessRulerResult();
            MjData.ProcessData.processRulerResult.seatID = seatID;
            MjData.ProcessData.processRulerResult.rulerResult = rulerResult;
            MjData.ProcessData.processRulerResult.gangList = gangList;
            MjData.ProcessData.processRulerResult.tingList = tingInfo;
            MjData.ProcessData.processRulerResult.ciList = ciList;
        }


        public MjPaiKou CheckPaikou(int ruler)
        {
            MjData.ProcessData.IniProcessActionCode();
            MjPaiKou paikouStruct = this.GetPaikouStruct(ruler);
            return paikouStruct;
        }

        public void ClearPaikou()
        {
            MjData.ProcessData.IniProcessActionCode();
        }


        public void SetPaikouPeng(int mjCode)
        {
            MjData.ProcessData.processActionCode.pengCode = mjCode;
        }

        public int GetPaikouPeng()
        {
            return MjData.ProcessData.processActionCode.pengCode;
        }

        public void SetPaiKouHu(int mjCode)
        {
            MjData.ProcessData.processActionCode.huCode = mjCode;
        }

        public int GetPaikouHu()
        {
            return MjData.ProcessData.processActionCode.huCode;
        }

        public void SetPaikouGang(List<int> mjCode)
        {
            MjData.ProcessData.processActionCode.gangList = mjCode;
        }

        public void SetPaikouCiList(List<int> ciList)
        {
            MjData.ProcessData.processActionCode.ciList = ciList;
        }

        public List<int> GetPaikouGangList()
        {
            return MjData.ProcessData.processActionCode.gangList;
        }

        public List<int> GetPaikouCiList()
        {
            return MjData.ProcessData.processActionCode.ciList;
        }

        public int GetPaikouGang(int index)
        {
            return MjData.ProcessData.processActionCode.gangList[index];
        }

        public int GetPaikouCi(int index)
        {
            return MjData.ProcessData.processActionCode.ciList[index];
        }

        public void SetPaikouTing(List<MjTingInfo> tingList)
        {
            if (tingList != null && tingList.Count > 0)
            {
                for (int i = 0; i < tingList.Count; i++)
                {
                    MjData.ProcessData.processActionCode.tingInfoDic.Add(tingList[i].tingCode, tingList[i]);
                }
            }
        }

        public List<int> GetPaikouTingIDList()
        {
            List<int> tingList = new List<int>();
            if (MjData.ProcessData.processActionCode != null && MjData.ProcessData.processActionCode.tingInfoDic != null
                && MjData.ProcessData.processActionCode.tingInfoDic.Count > 0)
            {
                foreach (KeyValuePair<int, MjTingInfo> item in MjData.ProcessData.processActionCode.tingInfoDic)
                {
                    MjTingInfo info = item.Value;
                    tingList.Add(info.tingCode);
                }
            }

            return tingList;
        }

        public MjTingInfo GetPaikouTing(int mjCode)
        {
            if (MjData.ProcessData.processActionCode.tingInfoDic.ContainsKey(mjCode))
            {
                return MjData.ProcessData.processActionCode.tingInfoDic[mjCode];
            }
            return null;
        }

        public bool CheckPaikouTing(int mjCode)
        {
            return MjData.ProcessData.processActionCode.tingInfoDic.ContainsKey(mjCode);
        }


        public void SetPaikouTingConfirm(MjTingInfo info, bool showState = true)
        {
            MjData.ProcessData.curTingInfo = info;
            if (showState)
            {
                EventDispatcher.FireEvent(GEnum.NamedEvent.EMjControlSetTingRest);
            }
        }


        public void SortTingInfoModel(List<MjTingInfoModel> infoList, bool sortNormal)
        {
            if (infoList != null && infoList.Count > 0)
            {
                if (sortNormal)
                {
                    infoList.Sort(SortTingInfoSimple);
                }
                else
                {
                    infoList.Sort(SortTingInfoCombine);
                }
            }

        }

        private int SortTingInfoSimple(MjTingInfoModel left, MjTingInfoModel right)
        {
            return left.mjCode.CompareTo(right.mjCode);
        }


        private int SortTingInfoCombine(MjTingInfoModel left, MjTingInfoModel right)
        {
            int returnNum = 0;
            returnNum = -left.restCount.CompareTo(right.restCount);
            if (returnNum == 0)
            {
                returnNum = -left.maxOdd.CompareTo(right.maxOdd);
                if (returnNum == 0)
                {
                    returnNum = left.mjCode.CompareTo(right.mjCode);
                }
            }

            return returnNum;
        }


        //public void SetPaikouTingConfirm(int mjCode)
        //{
        //    MjTingInfo info = this.GetPaikouTing(mjCode);
        //    MjData.ProcessData.curTingInfo = info;
        //}


        //public bool CheckPaikouTingCard(int mjCode)
        //{
        //    bool isCountain = false;
        //    MjTingInfo confirmInfo = MjData.ProcessData.curTingInfo;
        //    if (confirmInfo != null)
        //    {
        //        if (confirmInfo.huCode.Contains(mjCode))
        //        {
        //            isCountain = true;
        //        }
        //    }

        //    return isCountain;
        //}

        //public int CutDownPaikouTing(int mjCode, int num)
        //{
        //    int curRest = 0;
        //    MjTingInfo confirmInfo = MjData.ProcessData.curTingInfo;
        //    if (confirmInfo != null)
        //    {
        //        for (int i = 0; i < confirmInfo.huCode.Count; i++)
        //        {
        //            if (confirmInfo.huCode[i] == mjCode)
        //            {
        //                confirmInfo.huCodeNum[i] -= num;
        //                if (confirmInfo.huCodeNum[i] < 0)
        //                {
        //                    confirmInfo.huCodeNum[i] = 0;
        //                }
        //                curRest = confirmInfo.huCodeNum[i];
        //                break;
        //            }
        //        }
        //    }
        //    return curRest;
        //}


        #region paikou chi

        public void SetPaikouChi(int code, List<int> handList)
        {
            CardTypeStartEnd startEnd = GetStartEnd(code);
            int checkNum1 = -1;
            int checkNum2 = -1;
            bool isOk = false;
            int chiIndex = 0;

            //check 吃的牌在最左边
            checkNum1 = code + 1;
            checkNum2 = code + 2;
            MjActionCodeChi leftChiAction = new MjActionCodeChi();
            if (checkNum2 <= startEnd.endID)
            {
                //边界合理
                leftChiAction.chiCode = code;
                isOk = CheckChiCodeContain(checkNum1, checkNum2, leftChiAction, handList);
                //bool isOk = containNum1 && containNum2;
                if (isOk)
                {
                    leftChiAction.chiIndex = chiIndex;
                    chiIndex++;
                    MjData.ProcessData.processActionCode.chiCodeList.Add(leftChiAction);
                }
            }

            //check 吃的牌在中间

            checkNum1 = code - 1;
            checkNum2 = code + 1;
            MjActionCodeChi midChiAction = new MjActionCodeChi();
            if (checkNum1 >= startEnd.startID && checkNum2 <= startEnd.endID)
            {
                //边界合理
                midChiAction.chiCode = code;
                isOk = CheckChiCodeContain(checkNum1, checkNum2, midChiAction, handList);
                //bool isOk = containNum1 && containNum2;
                if (isOk)
                {
                    leftChiAction.chiIndex = chiIndex;
                    chiIndex++;
                    MjData.ProcessData.processActionCode.chiCodeList.Add(midChiAction);
                }
            }

            //check 吃的牌在右边
            checkNum1 = code - 2;
            checkNum2 = code - 1;
            MjActionCodeChi rightChiAction = new MjActionCodeChi();
            if (checkNum1 >= startEnd.startID)
            {
                //边界合理
                rightChiAction.chiCode = code;
                isOk = CheckChiCodeContain(checkNum1, checkNum2, rightChiAction, handList);
                //bool isOk = containNum1 && containNum2;
                if (isOk)
                {
                    leftChiAction.chiIndex = chiIndex;
                    chiIndex++;
                    MjData.ProcessData.processActionCode.chiCodeList.Add(rightChiAction);
                }
            }
        }


        public List<MjActionCodeChi> GetPaikouChi()
        {
            return MjData.ProcessData.processActionCode.chiCodeList;
        }

        public void SetCurPaikouChi(MjActionCodeChi codeChi)
        {
            MjData.ProcessData.processActionCode.curCodeChi = codeChi;
        }

        public MjActionCodeChi GetCurPaikouChi()
        {
            MjActionCodeChi codeChi = MjData.ProcessData.processActionCode.curCodeChi;
            if (codeChi != null)
            {
                int loseCard1 = codeChi.selfCode1;
                int loseCard2 = codeChi.selfCode2;

                this.SetPlayerHandLose(MjData.curUserData.selfSeatID, loseCard1);
                this.SetPlayerHandLose(MjData.curUserData.selfSeatID, loseCard2);
            }

            return codeChi;
        }

        private class CardTypeStartEnd
        {
            public int startID;
            public int endID;
        }

        private CardTypeStartEnd GetStartEnd(int code)
        {
            CardTypeStartEnd startEnd = new CardTypeStartEnd();
            if (code <= 7)
            {
                //东西南北中发白
                startEnd.startID = 1;
                startEnd.endID = 7;
            }
            else if (code <= 16)
            {
                //万
                startEnd.startID = 8;
                startEnd.endID = 16;
            }
            else if (code <= 25)
            {
                //条
                startEnd.startID = 17;
                startEnd.endID = 25;
            }
            else
            {
                //筒
                startEnd.startID = 26;
                startEnd.endID = 34;
            }
            return startEnd;
        }

        private bool CheckChiCodeContain(int checkNum1, int checkNum2, MjActionCodeChi chiAction, List<int> handList)
        {
            bool checked1 = false;
            bool checked2 = false;
            int selfSeatID = MjData.curUserData.selfSeatID;
            // List<int> mjList = MjData.ProcessData.processPlayer[selfSeatID].list_Hand;
            List<int> mjList = handList;
            for (int i = 0; i < mjList.Count; i++)
            {
                if (mjList[i] == checkNum1)
                {
                    chiAction.selfCode1 = checkNum1;
                    chiAction.selfCodeIndex1 = i;
                    checked1 = true;
                }
                if (mjList[i] == checkNum2)
                {
                    chiAction.selfCode2 = checkNum2;
                    chiAction.selfCodeIndex2 = i;
                    checked2 = true;
                }
            }

            bool isOk = checked1 && checked2;
            if (isOk)
            {
                List<int> chiList = new List<int>();
                chiList.Add(chiAction.chiCode);
                chiList.Add(checkNum1);
                chiList.Add(checkNum2);
                chiList.Sort();
                chiAction.chiList = chiList;
            }
            return isOk;
        }


        #endregion

        public MjPaiKou GetPaikouStruct(int mjRulerResult)
        {
            MjPaiKou paikou = new MjPaiKou();
            paikou.canPeng = (mjRulerResult & ConstDefine.MJ_PK_PENG) == ConstDefine.MJ_PK_PENG;
            paikou.canGang = (mjRulerResult & ConstDefine.MJ_PK_GANG) == ConstDefine.MJ_PK_GANG;
            paikou.canTing = (mjRulerResult & ConstDefine.MJ_PK_TING) == ConstDefine.MJ_PK_TING;
            paikou.canHu = (mjRulerResult & ConstDefine.MJ_PK_HUPAI) == ConstDefine.MJ_PK_HUPAI;
            paikou.canChi = (mjRulerResult & ConstDefine.MJ_PK_CHI) == ConstDefine.MJ_PK_CHI;
            paikou.canMingLou = (mjRulerResult & ConstDefine.Mj_PK_MINGLOU) == ConstDefine.Mj_PK_MINGLOU;
            paikou.canTingGang = (mjRulerResult & ConstDefine.Mj_PK_TINGGANG) == ConstDefine.Mj_PK_TINGGANG;
            paikou.canPiao = (mjRulerResult & ConstDefine.Mj_PK_XUANPIAO) == ConstDefine.Mj_PK_XUANPIAO;
            paikou.canCiHu = (mjRulerResult & ConstDefine.Mj_Pk_CIHU) == ConstDefine.Mj_Pk_CIHU;
            return paikou;
        }

        #endregion

        #region 其他管理

        #region 解散牌桌
        public int SetCloseStart(int seatID)
        {
            if (MjData.processClose == null)
            {
                MjData.processClose = new MahongDataModel.GameProcessCloseDesk();
            }

            MjData.processClose.startSeatID = seatID;
            MjData.processClose.SetSeatState(seatID, 1);
            return seatID;
        }

        public void SetCloseState(int seatID, int state)
        {
            if (MjData.processClose != null)
            {
                MjData.processClose.SetSeatState(seatID, state);
            }
            else
            {
                SetCloseStart(seatID);
            }

        }

        public int GetCloseState(int seatID)
        {
            if (MjData.processClose != null)
            {
                return MjData.processClose.GetSeatState(seatID);
            }
            return -1;
        }

        public void SetCloseEnd()
        {
            MjData.processClose = null;
        }

        #endregion

        #region 断线重连

        public void SetReconnedData(MahongDataModel.GameProcessReconned processData)
        {
            ClearLastGame();
            MahjongLogicNew.Instance.ClearUp();
            MjData.ProcessData.processReconned = processData;

            MjDeskInfo deskInfo = processData.deskInfo;
            MemoryData.DeskData.AddOrUpdateDeskInfo(deskInfo.deskID, deskInfo);
            MjData.curUserData.selfDeskID = deskInfo.deskID;
            List<MjPlayerInfo> playerInfoList = processData.playerInfoList;
            SetPlayerInfosByDeskID(playerInfoList, deskInfo.deskID);
            if (playerInfoList != null && playerInfoList.Count > 0)
            {
                for (int i = 0; i < playerInfoList.Count; i++)
                {
                    if (playerInfoList[i].userID == MemoryData.UserID)
                    {
                        MjData.curUserData.selfSeatID = playerInfoList[i].seatID;
                        MjData.curUserData.selfUserID = (ulong)playerInfoList[i].userID;
                    }
                }
            }

            SetProcessStateInfo(processData.beforeDispatchValues);
        }

        #endregion

        #region 战绩管理
        public bool CheckResultNeedShow()
        {
            int deskID = MjData.curUserData.selfDeskID;
            GameResult result = MemoryData.ResultData.get(deskID);
            int curCount = 0;
            if (result != null)
            {
                curCount = result.bureauDetailList.Count;
            }
            bool isShow = curCount > 0;

            return isShow;
        }


        public void SetGameOverResultData(bool isShow, int gameSub, int gameType, int oddsCount, int showType,
            List<MjBureauDetialInfo> bureauList, List<MjTitleInfo> titleInfoList, int createTime, long ownerID, List<GameResultCostData> resultCostList, int resultType)
        {
            //回放不记录战绩信息
            if (FakeReplayManager.Instance.ReplayState)
                return;
            int selfSeatID = MjData.curUserData.selfSeatID;
            int deskID = MjData.curUserData.selfDeskID;
            MjPlayerInfo[] playerInfo = GetAllPlayerInfoByDeskID(deskID);
            GameResult gameResult = new GameResult(deskID, gameType, gameSub, oddsCount, showType, bureauList, titleInfoList, selfSeatID);

            List<GameResultPlayer> playerList = new List<GameResultPlayer>();
            for (int i = 0; i < playerInfo.Length; i++)
            {
                if (playerInfo[i] == null)
                {
                    continue;
                }
                PlayerDataModel playerModel = MemoryData.PlayerData.get(playerInfo[i].userID);

                DebugPro.Log(DebugPro.EnumLog.HeadUrl, "MahjongLogicNew__LogicMjGameOverNotify", playerModel.PlayerDataBase.HeadURL);
                GameResultPlayer playerItem = new GameResultPlayer(playerInfo[i].userID, playerModel.PlayerDataBase.Name, playerModel.PlayerDataBase.HeadURL);
                playerItem.SetResultData(playerInfo[i].seatID, playerInfo[i].score, playerInfo[i].winBouts);
                playerList.Add(playerItem);
            }
            gameResult.SetPlayerInfo(playerList, MemoryData.UserID);
            gameResult.maxBouts = MemoryData.DeskData.GetOneDeskInfo(deskID).rounds;
            gameResult.recordTime = createTime;
            gameResult.ownerUserID = ownerID;

            MemoryData.ResultData.CheckAndUpdateData(gameResult);
            MemoryData.ResultData.SetResultCostData(deskID, resultCostList, resultType);


            EventDispatcher.FireEvent(GEnum.NamedEvent.EMjGameOverNotify, isShow);
        }


        #endregion

        #endregion

        #region 玩家管理
        /// <summary>
        /// 通过桌子ID设置Player信息
        /// </summary>
        /// <param name="playerInfoList"></param>
        /// <param name="roomID"></param>
        public void SetPlayerInfosByDeskID(List<MjPlayerInfo> playerInfoList, int deskID)
        {
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(deskID);

            if (deskInfo == null)
            {
                return;
            }

            if (playerInfoList != null && playerInfoList.Count > 0)
            {
                for (int i = 0; i < playerInfoList.Count; i++)
                {
                    deskInfo.SetPlayerInfo(playerInfoList[i].userID);
                }
            }
        }



        /// <summary>
        /// 通过桌子ID和座位ID获取Player
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public MjPlayerInfo GetPlayerInfoByDeskIDAndSeatID(int deskID, int seatID)
        {
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(deskID);
            if (deskInfo == null)
            {
                return null;
            }
            long userID = deskInfo.GetPlayerIdBySeatId(seatID);
            return MemoryData.PlayerData.get(userID).playerDataMj;
        }


        public MjPlayerInfo GetCurDeskPlayerInfoBySeatID(int seatID)
        {
            int deskID = MjData.curUserData.selfDeskID;

            return GetPlayerInfoByDeskIDAndSeatID(deskID, seatID);
        }


        /// <summary>
        /// 通过桌子ID获取所有Player
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public MjPlayerInfo[] GetAllPlayerInfoByDeskID(int deskID)
        {
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(deskID);
            if (deskInfo == null)
            {
                return null;
            }

            long[] playerIdList = deskInfo.GetAllPlayerID();
            MjPlayerInfo[] infos = new MjPlayerInfo[4];
            for (int i = 0; i < playerIdList.Length; i++)
            {
                infos[i] = MemoryData.PlayerData.get(playerIdList[i]).playerDataMj;
            }

            return infos;
        }
        /// <summary>
        /// 根据当前桌号拿到userid
        /// </summary>
        /// <param name="seatId"></param>
        /// <returns></returns>
        public long GetCurrentUserIDBySeatID(int seatId)
        {
            int deskID = MjDataManager.Instance.MjData.curUserData.selfDeskID;
            return GetPlayerIDByDeskID(deskID, seatId);
        }
        /// <summary>
        /// 通过桌子ID和座位号获取userID
        /// </summary>
        /// <param name="deskID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public long GetPlayerIDByDeskID(int deskID, int seatID)
        {
            long returnID = -1;
            long[] userIDs = GetAllPlayerIDByDeskID(deskID);
            if (userIDs != null)
            {
                for (int i = 0; i < userIDs.Length; i++)
                {
                    PlayerDataModel model = MemoryData.PlayerData.CheckModel(userIDs[i]);
                    if (model != null)
                    {
                        int curSeatID = model.playerDataMj.seatID;
                        if (curSeatID == seatID)
                        {
                            returnID = userIDs[i];
                            break;
                        }
                    }
                }
            }

            return returnID;
        }


        /// <summary>
        /// 通过桌子号 获取所有玩家UserID
        /// </summary>
        /// <param name="deskID"></param>
        /// <returns></returns>
        public long[] GetAllPlayerIDByDeskID(int deskID)
        {
            MjDeskInfo deskInfo = MemoryData.DeskData.GetOneDeskInfo(deskID);
            if (deskInfo == null)
            {
                return null;
            }
            return deskInfo.GetAllPlayerID();
        }


        /// <summary>
        /// 获取当前桌所有玩家的UI座位号
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllUiSeatCurDesk(bool removeSelf)
        {
            SetCurDeskPlayer();

            List<int> returnList = new List<int>(MjData.playerUISeat);
            if (removeSelf)
            {
                returnList.Remove(0);
            }

            return returnList;
        }

        //change
        public List<int> GetAllSeatIDCurDesk(bool removeSelf)
        {
            List<int> seatList = new List<int>() { 1, 2, 3, 4 };

            if (removeSelf)
            {
                int seatSelf = MjData.curUserData.selfSeatID;
                seatList.Remove(seatSelf);
            }

            return seatList;
        }

        /// <summary>
        /// 获取当前桌上玩家数量
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCountCurDesk()
        {
            SetCurDeskPlayer();

            return MjData.playerUISeat.Count;
        }


        private void SetCurDeskPlayer()
        {
            if (MjData.playerUISeat == null || MjData.playerUISeat.Count < 2)
            {
                int deskID = MjData.curUserData.selfDeskID;
                int selfSeatID = MjData.curUserData.selfSeatID;
                MjPlayerInfo[] players = GetAllPlayerInfoByDeskID(deskID);
                List<int> uiSeatID = new List<int>();
                if (players != null && players.Length > 0)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i] == null)
                        {
                            continue;
                        }

                        int curSeat = players[i].seatID;
                        int curUISeat = CardHelper.GetMJUIPosByServerPos(curSeat, selfSeatID);
                        uiSeatID.Add(curUISeat);
                    }
                }
                MjData.playerUISeat = uiSeatID;
            }
        }


        #endregion

    }

}


