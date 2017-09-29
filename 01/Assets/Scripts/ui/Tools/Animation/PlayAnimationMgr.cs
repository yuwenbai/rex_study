/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationData
{
    //分组名称
    public string GroupName;

}

public class PlayAnimationMgr : MonoBehaviour {
    public List<PlayAnimationData> AnimationGroups = new List<PlayAnimationData>();

    private void Awake()
    {
        
    }
}
