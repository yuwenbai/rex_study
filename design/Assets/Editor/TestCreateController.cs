using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class TestCreateController : Editor {

    [MenuItem("Test/CreateController")]
    static void DOCreateAnimationAssets()
    {
        //创建一个controller
        AnimatorController mAnimatorController = AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/Animator/ani.controller");
        //mAnimatorController.layers

    }
    private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine mAnimatorStateMachine = layer.stateMachine;
        AnimationClip mClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        AnimatorState mState = mAnimatorStateMachine.AddState(mClip.name);

    }
}
