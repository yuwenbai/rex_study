/**
 * @Author lyb
 *
 *
 */

using UnityEngine;

namespace projectQ
{
    [RequireComponent(typeof(Animation))]
    public class AnimationCtrl : MonoBehaviour
    {
        /// <summary>
        /// 最小时间
        /// </summary>
        private int RandomTimeMin = 0;
        /// <summary>
        /// 最大时间
        /// </summary>
        private int RandomTimeMax = 300;

        private bool isPlay;
        private float waitTime;

        private Animation animation;

        void Start()
        {
            animation = gameObject.GetComponent<Animation>();

            if (gameObject.GetComponent<AnimationRandom>() != null)
            {
                RandomTimeMin = gameObject.GetComponent<AnimationRandom>().RandomTimeMin;
                RandomTimeMax = gameObject.GetComponent<AnimationRandom>().RandomTimeMax;
            }
        }

        void Update()
        {
            if (isPlay)
            {
                waitTime -= Time.deltaTime;

                if (waitTime <= 0)
                {
                    isPlay = false;

                    animation.Play();
                }
            }
        }

        /// <summary>
        /// 动画播放完毕 。。 随机选取一个时间再次播放该特效
        /// </summary>
        public void AnimationFinish()
        {
            animation.Stop();

            waitTime = PlayTime_Random();

            isPlay = true;
        }

        /// <summary>
        /// 生成设置范围内的下次播放特效的随机数
        /// </summary>
        private float PlayTime_Random()
        {
            int num = Random.Range(RandomTimeMin, RandomTimeMax);

            float time = num * 0.01f;

            return time;
        }
    }
}
