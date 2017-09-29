/**
 * @Author lyb
 *  创建一个tween动画的运动轨迹
 *
 */

using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewAnimationCurve", menuName = "ScriptableData/NewAnimationCurve")]
public class AnimationCurveAsset : ScriptableObject 
{	
	public AnimationCurve Curve;
}