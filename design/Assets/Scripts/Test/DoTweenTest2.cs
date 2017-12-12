using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class DoTweenTest2 : MonoBehaviour
{
    Vector2 centerPos;
    Image image;
    // Use this for initialization
    void Start()
    {
        centerPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        image = transform.GetComponent<Image>();
    }



    void OnGUI()
    {
        if (GUILayout.Button("move to word pos(100,100)"))
        {
            //DoMove的坐标系是左下角为准,移动到100,100位置
            Tweener tweener = image.rectTransform.DOMove(new Vector2(100, 100), 10f);
            tweener.OnComplete(() =>
            {
                Debug.Log("rextest 111111");
            });
        }
        if (GUILayout.Button("move to anchor pos(100,100)"))
        {
            //image.rectTransform.DOMove(new Vector3(Screen.width * 0.5f + 100, Screen.height * 0.5f + 100, 0), 1f);
            DOTween.KillAll(true);
        }
        if (GUILayout.Button("kill false"))
        {//每点击一次，在原始缩放基础上放大（2，2）
            //当前sacle(1,1,1)1秒内添加到(3,3,1)
            //image.rectTransform.DOBlendableScaleBy(new Vector2(2, 2), 1f);
            ////          image.rectTransform.DOScale (new Vector2(2,2),1f);
            DOTween.KillAll(false);
        }
        if (GUILayout.Button("scale to (2,2,1)"))
        {

            image.rectTransform.DOScale(new Vector3(2, 2, 1), 1f);

        }
        if (GUILayout.Button("rotate 180 degree"))
        {
            //旋转到180度
            image.rectTransform.DORotate(new Vector3(0, 0, 180), 1f);
        }

        if (GUILayout.Button("test tweener event"))
        {
            Tweener tweener = image.rectTransform.DOMove(new Vector3(Screen.width * 0.5f + 300, Screen.height * 0.5f - 100, 0), 1f);
            tweener.OnPlay(OnPlay);
            tweener.OnComplete(OnComplete);
            //          tweener.OnComplete (delegate() {
            //              Debug.Log("tween animation 结束");
            //          });
        }
    }

    void OnComplete()
    {
        Debug.Log("tween animation 结束");
    }

    void OnPlay()
    {
        Debug.Log("tween animation 开始");
    }
}