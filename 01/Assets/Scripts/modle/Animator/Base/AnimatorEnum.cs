/**
 * @Author lyb
 * 动画的枚举列举
 *
 */

namespace projectQ
{
    public enum SexEnum
    {
        /// <summary>
        /// 男
        /// </summary>
        Man = 0,
        /// <summary>
        /// 女
        /// </summary>
        Woman = 1,
    }

    public enum AnimatorEnum
    {
        /// <summary>
        /// 吃碰杠
        /// </summary>
        Anim_chipenggang,
        /// <summary>
        /// 手出牌
        /// </summary>
        Anim_chupai,
        /// <summary>
        /// 手扣牌
        /// </summary>
        Anim_koupai,
        /// <summary>
        /// 手推牌
        /// </summary>
        Anim_tuipai,
        /// <summary>
        /// 手插牌
        /// </summary>
        Anim_chapai,
        /// <summary>
        /// 手点击
        /// </summary>
        Anim_dianji,
        /// <summary>
        /// 抓牌
        /// </summary>
        Anim_zhuapai,
        /// <summary>
        /// 整理牌型
        /// </summary>
        Anim_zhengli,
    }

    /// <summary>
    /// 动画事件枚举
    /// </summary>
    public enum AnimatorEventEnum
    {
        /// <summary>
        /// 出牌 - 移动到p0点位置1
        /// </summary>
        Anim_chupai_move1,
        /// <summary>
        /// 出牌 - 打牌动画播放完毕
        /// </summary>
        Anim_chupai_motion1,
        /// <summary>
        /// 出牌 - 移动到p1点位置2
        /// </summary>
        Anim_chupai_move2,
        /// <summary>
        /// 出牌 - 手移动回初始位置
        /// </summary>
        Anim_chupai_moveInit,

        /// <summary>
        /// 吃碰杠 - 收到了要移动的消息
        /// </summary>
        Anim_chipenggang_moveBegin,
        /// <summary>
        /// 吃碰杠 - 移动到P0点位置
        /// </summary>
        Anim_chipenggang_move1,
        /// <summary>
        /// 吃碰杠 - 动画播放完毕
        /// </summary>
        Anim_chipenggang_motion1,
        /// <summary>
        /// 吃碰杠 - 移动到p1点位置2
        /// </summary>
        Anim_chipenggang_move2,
        /// <summary>
        /// 吃碰杠 - 手移动回初始位置
        /// </summary>
        Anim_chipenggang_moveInit,

        /// <summary>
        /// 扣牌 - 移动到p0点位置1
        /// </summary>
        Anim_koupai_move1,
        /// <summary>
        /// 扣牌 - 扣牌动画播放完毕
        /// </summary>
        Anim_koupai_motion1,
        /// <summary>
        /// 扣牌 - 移动到p1点位置2
        /// </summary>
        Anim_koupai_move2,
        /// <summary>
        /// 扣牌 - 手移动回初始位置
        /// </summary>
        Anim_koupai_moveInit,

        /// <summary>
        /// 插牌 - 移动到p0点位置1
        /// </summary>
        Anim_chapai_move1,
        /// <summary>
        /// 插牌 - 动画播放完毕
        /// </summary>
        Anim_chapai_motion1,
        /// <summary>
        /// 插牌 - 手往上抬指定高度1
        /// </summary>
        Anim_chapai_up1,
        /// <summary>
        /// 插牌 - 移动到p1上面
        /// </summary>
        Anim_chapai_up2,
        /// <summary>
        /// 插牌 - 移动到p1点位置2
        /// </summary>
        Anim_chapai_move2,
        /// <summary>
        /// 插牌 - 手移动回初始位置
        /// </summary>
        Anim_chapai_moveInit,
    }
}
