/**
* @Author lyb
*
*
*/

using UnityEngine;

namespace projectQ
{
    public class Animator_Hand : MonoBehaviour
    {
        /// <summary>
        /// 手臂模型
        /// </summary>
        public GameObject Model_Hand;
        /// <summary>
        /// 手臂绑点
        /// </summary>
        public Transform Model_HandPoint;
        /// <summary>
        /// 插牌手臂绑点
        /// </summary>
        public Transform Model_HandChaPoint;

        /// <summary>
        /// 当前控制的手的座位号
        /// </summary>
        private int _HandIndex;
        public int HandIndex
        {
            get { return _HandIndex; }
        }

        /// <summary>
        /// 当前是男的还是女的
        /// </summary>
        public SexEnum _AnimSex;
        public SexEnum AnimSex
        {
            get { return _AnimSex; }
            set { _AnimSex = value; }
        }

        private Animator A_nimator;        

        public void Start()
        {
            A_nimator = Model_Hand.GetComponent<Animator>();
        }

        #region lyb----------------方法----------------------------------
        
        /// <summary>
        /// 动画初始化
        /// p0 p1 不同的动画代表不同的含义，每步动画中做详细解释
        /// index 当前控制的是哪个座位
        /// </summary>
        public void Animator_Init(AnimatorEnum animEnum, Vector3 p0, Vector3 p1, int index, int extend)
        {
            //A_nimator.CrossFade("Idle", 0);

            _HandIndex = index;

            switch (animEnum)
            {
                case AnimatorEnum.Anim_chipenggang:
                    Animator_ChiPengGang chipenggang = InitAnimData<Animator_ChiPengGang>();
                    chipenggang.Init_Animator_ChiPengGang(p0, p1, gameObject, extend);
                    break;
                case AnimatorEnum.Anim_chupai:
                    Animator_ChuPai chupai = InitAnimData<Animator_ChuPai>();
                    chupai.Init_Animator_ChuPai(p0, p1, gameObject, extend);
                    break;
                case AnimatorEnum.Anim_koupai:
                    Animator_KouPai koupai = InitAnimData<Animator_KouPai>();
                    koupai.Init_Animator_KouPai(p0, p1, gameObject, extend);
                    break;
                case AnimatorEnum.Anim_tuipai:
                    break;
                case AnimatorEnum.Anim_chapai:
                    Animator_ChaPai chapai = InitAnimData<Animator_ChaPai>();
                    chapai.Init_Animator_ChaPai(p0, p1, gameObject, extend);
                    break;
                case AnimatorEnum.Anim_dianji:
                    break;
                case AnimatorEnum.Anim_zhuapai:
                    break;
                case AnimatorEnum.Anim_zhengli:
                    break;
            }
        }

        public T InitAnimData<T>() where T : BaseAnimator
        {
            T data = Model_Hand.GetComponent<T>();
            if (data == null)
            {
                data = Model_Hand.AddComponent<T>();
            }

            return data;
        }

        #endregion ------------------------------------------------------
    }
}