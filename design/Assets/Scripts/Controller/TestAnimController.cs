using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimData
{
    public string eventName;
    public float mTime;
}
public class TestAnimController : MonoBehaviour {

    // Use this for initialization
    private Animator mAnimator = null;
    private RuntimeAnimatorController mRuntimeAnimatorController = null;
    private AnimationClip[] mAnimationClip = null;
    private Dictionary<string, AnimData> mDicAnimData;
    void Start () {
        mAnimator = GetComponent<Animator>();
        mRuntimeAnimatorController = mAnimator.runtimeAnimatorController;
        mAnimationClip = mRuntimeAnimatorController.animationClips;

        mDicAnimData = new Dictionary<string, AnimData>();
        for (int i = 0; i < 5; ++i)
        {
            mDicAnimData["anim" + i] = new AnimData();
            mDicAnimData["anim" + i].mTime = i;
            mDicAnimData["anim" + i].eventName = "animevent" + i;
        }

        for (int i = 0; i < mAnimationClip.Length; ++i)
        {
            AnimationClip mclip = mAnimationClip[i];
            if (mclip.events.Length == 0)
            {
                switch (mclip.name)
                {
                    case "1":
                        AnimationEvent event1 = new AnimationEvent();
                        event1.time = mclip.length * 0.5f;
                        event1.functionName = "animevent0";
                        mclip.AddEvent(event1);
                        break;
                    case "2":
                        break;
                    default:
                        break;
                }
            }
        }
        mAnimator.Rebind();
    }
    /// <summary>  
    /// 注销对应事件  
    /// </summary>  
    void UnSubscription()
    {
        for (int i = 0; i < mAnimationClip.Length; i++)
        {
            mAnimationClip[i].events = default(AnimationEvent[]);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
