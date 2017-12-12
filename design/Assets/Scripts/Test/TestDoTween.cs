using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoTween : MonoBehaviour {
    public float aaa;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [ContextMenu("DoTweenAlpha")]
    void DoTweenAlpha()
    {
        //GetComponent<MeshRenderer>().material.color = Color.green;
        Debug.Log("DoTweenAlpha");
        //if (uiRect != null)
        {
            DOTween.To(x => GetComponent<MeshRenderer>().material.color = new Color(x,1,1,1), 1.0f, 0.0f, 5.0f).SetId("Tween");
        }
    }

    [ContextMenu("DoTweenKillCompleteAlpha")]
    void DoTweenKillCompleteAlpha()
    {
        Debug.Log("DoTweenKillCompleteAlpha");

        //if (uiRect != null)
        {
            DOTween.Kill("Tween", true);
        }
    }

    [ContextMenu("DoTweenKillAlpha")]
    void DoTweenKillAlpha()
    {
        Debug.Log("DoTweenKillAlpha");

        //if (uiRect != null)
        {
            DOTween.Kill("Tween", false);
        }
    }

    [ContextMenu("DoSequenceAlpha")]
    void DoSequenceAlpha()
    {
        Debug.Log("DoSequenceAlpha");

        //if (uiRect != null)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(x => aaa = x, 1.0f, 0.0f, 5.0f));
            sequence.SetId("Sequence");
        }
    }

    [ContextMenu("DoSequenceKillCompleteAlpha")]
    void DoSequenceKillCompleteAlpha()
    {
        Debug.Log("DoSequenceKillCompleteAlpha");

        //if (uiRect != null)
        {
            DOTween.Kill("Sequence", true);
        }
    }

    [ContextMenu("DoSequenceKillAlpha")]
    void DoSequenceKillAlpha()
    {
        Debug.Log("DoSequenceKillAlpha");

        //UIRect uiRect = m_uiRectAni;
        //if (uiRect != null)
        {
            DOTween.Kill("Sequence", false);
        }
    }

}
