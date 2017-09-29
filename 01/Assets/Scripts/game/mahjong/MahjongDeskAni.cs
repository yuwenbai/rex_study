using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using projectQ;

public class MahjongDeskAni : MonoBehaviour
{
    public delegate void CreatMahJongFunction(E_CreateMahJongState state, Transform targetobj);
    public delegate void AcFunction();

    private bool m_IsCreat;
    public float limit = 0.5f;
    public Transform targetParent;
    public List<Transform> creatTrans;
    private List<GameObject> m_RoteNodes = new List<GameObject>();
    private bool m_IsInit = false;

    public void StopAllCont()
    {
        this.StopAllCoroutines();

        for (int i = 0; i < m_RoteNodes.Count; i++)
        {
            TweenPosition tp = m_RoteNodes[i].transform.GetComponent<TweenPosition>();
            if (tp != null)
            {
                tp.enabled = false;
                DestroyImmediate(tp);
            }
            Transform point = creatTrans[i].transform.FindChild("Point");
            if (point != null)
            {
                point.gameObject.SetActive(true);
                TweenPosition post = point.GetComponent<TweenPosition>();
                if (post != null)
                {
                    post.enabled = false;
                    DestroyImmediate(post);
                }
            }

            m_RoteNodes[i].transform.localPosition = m_LandPosition[i];
            //creatTrans[i].gameObject.SetActive(false);
        }

        ClearDice();
    }

    #region 码牌动画

    private Vector3[] m_LandPosition = new Vector3[]
    {
        new Vector3(3.45f,10.33f,-3.103f),
        new Vector3(3.134f,10.33f,3.44f),
        new Vector3(-3.414f,10.33f,3.132f),
        new Vector3(-3.083f,10.33f,-3.452f)
    };

    private Vector3[] m_CreatPosition = new Vector3[]
    {
        //new Vector3(-6.5f,0.395f,-6.12f),
        //new Vector3(-6.563f, 0.395f, -6.2f),
        //new Vector3(-6.563f, 0.395f, -6.2f),
        //new Vector3(-6.52f, 0.395f, -6.2f)
        new Vector3(-6.51f,0.395f,-6.13f),
        new Vector3(-6.57f, 0.395f, -6.2f),
        new Vector3(-6.57f, 0.395f, -6.2f),
        new Vector3(-6.525f, 0.395f, -6.195f)
    };

    public void Init()
    {
        if (m_IsInit)
            return;
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = targetParent.FindChild(string.Concat("land", i + 1)).gameObject;
            m_RoteNodes.Add(obj);

            GameObject child = new GameObject();
            child.name = "Point";
            child.transform.eulerAngles = new Vector3(90, -90 * (i + 1), 0);
            child.transform.localPosition = new Vector3(-6.51f, 0.42f, -0.33f);
            child.transform.parent = obj.transform;
            child.gameObject.SetActive(false);
            creatTrans.Add(child.transform);
        }
        m_IsInit = true;
    }

    private int m_CurWallIndex = -1;
    private AcFunction m_Fun = null;

    public void StartCreat(AcFunction func)
    {
        if (m_IsCreat)
            return;

        if (!m_IsInit)
            Init();
        m_Fun = func;
        m_IsCreat = true;
        for (int i = 0; i < m_RoteNodes.Count; i++)
        {
            //creatTrans[i].position = new Vector3(0, 0.4f, -6.4f);
            Vector3 lasePos = m_CreatPosition[i];
            creatTrans[i].transform.localPosition = new Vector3(lasePos.x, lasePos.y - 1, lasePos.z);//new Vector3(-6.563f, -0.4f, -0.33f);
            creatTrans[i].gameObject.SetActive(true);
            //StartCoroutine(StartCreat2(m_RoteNodes[i], i));
            //StartCoroutine(CallBack(func));

            Sequence moveSequence = DOTween.Sequence();
            Transform trans = m_RoteNodes[i].transform;
            Vector3 pos = pos = m_LandPosition[i];
            pos = new Vector3(pos.x, pos.y - 0.85f, pos.z);

            moveSequence.Append(trans.DOLocalMove(pos, UpTime)).SetEase(Ease.Linear);

            Transform pointtrans = creatTrans[i];
            moveSequence.Append(pointtrans.DOLocalMove(new Vector3(-7.2f, 0.395f, -6.2f), 0.05f)).SetEase(Ease.Linear);
            pos = m_CreatPosition[i];
            moveSequence.Append(pointtrans.DOLocalMove(pos, CreDic)).SetEase(Ease.Linear);

            pos = m_LandPosition[i];
            moveSequence.Append(trans.DOLocalMove(pos, UpTime)).SetEase(Ease.Linear);
            moveSequence.AppendCallback(() =>
            {
                AniCallBack();
                FuncCallBack();
            });
        }
       // m_CurWallIndex = TimeTick.Instance.SetAction(FuncCallBack, UpTime);//(UpTime * 2 + CreDic)
    }

    private void AniCallBack()
    {
        m_IsCreat = false;
    }

    private void FuncCallBack()
    {
        if (m_Fun != null)
            m_Fun();
    }

    private float UpDown = 0.5f;
    private float UpTime = 0.7f;
    private float CreTime = 0.7f;
    private float CreDic = 0.3f;

    private IEnumerator CallBack(AcFunction func)
    {
        yield return new WaitForSeconds(UpTime + CreTime + UpTime);

        if (func != null)
            func();
    }

    private IEnumerator StartCreat2(GameObject target, int index)
    {
        TweenPosition post1 = target.AddComponent<TweenPosition>();
        var pos = m_LandPosition[index];
        pos = new Vector3(pos.x, pos.y - 0.85f, pos.z);
        post1.from = pos;
        post1.to = new Vector3(pos.x, pos.y - 0.85f, pos.z);
        post1.duration = UpDown;

        yield return new WaitForSeconds(UpTime);

        Destroy(post1);
        target.transform.FindChild("Point").gameObject.SetActive(true);
        TweenPosition post = target.transform.FindChild("Point").gameObject.AddComponent<TweenPosition>();
        post.from = new Vector3(-7.2f, 0.395f, -6.2f);
        post.to = m_CreatPosition[index];//new Vector3(-6.563f, 00.395f, -6.2f);
        post.duration = CreDic;

        yield return new WaitForSeconds(CreTime);
        Destroy(post);
        TweenPosition post2 = target.AddComponent<TweenPosition>();
        pos = m_LandPosition[index];
        post2.from = new Vector3(pos.x, pos.y - 0.75f, pos.z);
        post2.to = pos;
        post2.duration = UpDown;

        yield return new WaitForSeconds(UpTime);
        Destroy(post2);
        m_IsCreat = false;
    }


    private IEnumerator StartCreat(GameObject target, int index)
    {
        TweenRotation rota = target.AddComponent<TweenRotation>();
        rota.from = new Vector3(0, -index * 90 + 90, 0);
        rota.to = new Vector3(-5, -index * 90 + 90, 0);
        rota.duration = 0.8f;

        yield return new WaitForSeconds(1.1f);
        Destroy(rota);
        target.transform.FindChild("Point").gameObject.SetActive(true);
        TweenPosition post = target.transform.FindChild("Point").gameObject.AddComponent<TweenPosition>();
        post.from = new Vector3(-6.51f, 0.42f, -13f);
        post.to = new Vector3(-6.51f, 0.42f, -6.13f);
        post.duration = 1.8f;

        yield return new WaitForSeconds(2.1f);
        Destroy(post);
        TweenRotation rota2 = target.AddComponent<TweenRotation>();
        rota2.from = new Vector3(-5, -index * 90 + 90, 0);
        rota2.to = new Vector3(0, -index * 90 + 90, 0);
        rota2.duration = 0.8f;

        yield return new WaitForSeconds(0.8f);
        Destroy(rota2);
        m_IsCreat = false;
    }

    #endregion

    #region 骰子相关
    private Vector3[] m_MahjongDice = new Vector3[] {
            new Vector3(0,0,90),
            new Vector3(-90,0,0),
            new Vector3(0,0,180),
            new Vector3(0,0,-90),
            new Vector3(90,0,0),
            new Vector3(0,0,0)
        };

    private Vector3[] m_DiceEndPos = new Vector3[]
    {
            new Vector3(0,0,0.253f),//0.07f
            new Vector3(0,0,0.253f),
            new Vector3(0.327f,0,0.012f)
    };

    private Transform[] m_DiceList = new Transform[3];
    private Transform[] m_DiceTargetList = new Transform[3];
    private Animation[] m_AnimationList = new Animation[6];
    private AnimationState[] m_DictAnStaList = new AnimationState[6];
    private List<int> m_TargetPos = new List<int>();
    //存放筛子的容器
    private GameObject m_DiceParent;

    private IEnumerator m_Ani;
    private IEnumerator m_Des;

    public void BeforhandDice(Transform parent, int number = 2)
    {
        int num = 0;
        for (int i = 0; i < m_DiceTargetList.Length; i++)
        {
            if (m_DiceTargetList[i] != null)
                num++;
        }

        //if (num == number)
        //    return;
        //else
        //{
        for (int i = 0; i < m_DiceTargetList.Length; i++)
        {
            if (m_DiceTargetList[i] != null)
                Destroy(m_DiceTargetList[i].gameObject);
        }
        //}

        for (int i = 0; i < number; i++)
        {
            string pathName = GameConst.path_MahjongShaZiPrefab;
            Transform obj = CreatDoce(parent, pathName, m_DiceEndPos[2 - i]);
            m_DiceTargetList[i] = obj;
        }
    }

    public void ClearDice()
    {
        if (m_Ani != null)
            StopCoroutine(m_Ani);
        if (m_Des != null)
            StopCoroutine(m_Des);

        if (m_CurWallIndex >= 0)
        {
            TimeTick.Instance.RemoveAction(m_CurWallIndex);
            m_CurWallIndex = -1;
        }


        if (m_DiceTargetList != null)
        {
            for (int i = 0; i < m_DiceTargetList.Length; i++)
            {
                if (m_DiceTargetList[i] != null)
                    Destroy(m_DiceTargetList[i].gameObject);
            }
        }

    }



    private Transform CreatDoce(Transform parent, string path, Vector3 pos)
    {
        InitDiceParent(parent);

        Transform obj = GameTools.InstantiatePrefab(path);
        obj.SetParent(m_DiceParent.transform, false);
        Vector3 vec = pos;
        Destroy(obj.transform.GetComponent<Animation>());
        Destroy(obj.GetChild(0).GetComponent<Animation>());
        obj.transform.localPosition = vec;
        return obj;
    }

    private void InitDiceParent(Transform parent)
    {
        if (m_DiceParent == null)
        {
            m_DiceParent = new GameObject();
            m_DiceParent.transform.SetParent(parent, false);
            m_DiceParent.transform.localPosition = new Vector3(0, 0, 0.03f);
        }
    }

    //扔色子
    public void StartRollTheDice(Transform parent, object vars, AcFunction ac)
    {
        List<int> rollList = vars as List<int>;

        InitDiceParent(parent);

        //两个动画
        string zhuanName = "shaizizhuan_ani";
        string shaiziName = "shaizi_ani";

        for (int i = 0; i < m_DiceTargetList.Length; i++)
        {
            if (i > rollList.Count && m_DiceTargetList[i] != null)
                Destroy(m_DiceTargetList[i]);
        }

        for (int i = 0; i < rollList.Count; i++)
        {
            string pathName = GameConst.path_MahjongShaZiPrefab;
            if (i < 3)
                pathName = string.Concat(pathName, string.Format("_{0}", 2 - i));
            Transform go = GameTools.InstantiatePrefab(pathName);
            go.SetParent(m_DiceParent.transform, false);

            //获取并记录脚本
            m_AnimationList[i * 2] = go.transform.GetComponent<Animation>();
            if (i < 3)
                m_DictAnStaList[i * 2] = m_AnimationList[i * 2][string.Concat(zhuanName, string.Format("_{0}", 2 - i))];
            else
                m_DictAnStaList[i * 2] = m_AnimationList[i * 2][zhuanName];
            m_AnimationList[i * 2 + 1] = go.transform.GetChild(0).GetComponent<Animation>();
            if (i == 0)
                m_DictAnStaList[i * 2 + 1] = m_AnimationList[i * 2 + 1][string.Concat(shaiziName, string.Format("_{0}", 2 - i))];
            else
                m_DictAnStaList[i * 2 + 1] = m_AnimationList[i * 2 + 1][shaiziName];

            m_DiceList[i] = go;

            if (m_DiceTargetList[i] == null)
            {
                pathName = GameConst.path_MahjongShaZiPrefab;
                Transform obj = CreatDoce(parent, pathName, m_DiceEndPos[2 - i]);
                obj.gameObject.SetActive(false);
                m_DiceTargetList[i] = obj;
            }
            else
            {
                m_DiceTargetList[i].gameObject.SetActive(false);
            }
        }

        m_Ani = RollTheDice();
        StartCoroutine(m_Ani);
        m_Des = RollDelete(ac);
        StartCoroutine(m_Des);
        SetDicePosition(rollList);
    }

    public void SetDicePosition(List<int> posList)
    {
        m_TargetPos = posList;
    }

    private float zhuanTime = 1;

    private IEnumerator RollDelete(AcFunction ac)
    {
        yield return new WaitForSeconds(2.5f);

        m_TargetPos = new List<int>();
        for (int i = 0; i < m_DiceList.Length; i++)
        {
            if (m_DiceList[i] != null)
            {
                Destroy(m_DiceList[i].gameObject);
                Destroy(m_DiceTargetList[i].gameObject);
            }
        }

        if (ac != null)
            ac();
    }

    //扔骰子表现
    private IEnumerator RollTheDice()
    {
        //动画帧速
        float frame = 1.5f;
        float limit = 0.3f;

        //调整动画速度
        while (frame > 0)
        {
            zhuanTime -= Time.deltaTime;

            if (m_TargetPos.Count > 0)
            {
                if (frame >= 0.3f && zhuanTime <= 0)
                    frame -= 2.0f * Time.deltaTime;

                for (int i = 0; i < 3; i++)
                {
                    if (m_AnimationList[i * 2] != null)
                    {
                        m_DictAnStaList[i * 2].speed = frame;
                        m_DictAnStaList[i * 2 + 1].speed = frame;

                        if (frame <= limit)
                        {
                            m_DictAnStaList[i * 2].wrapMode = WrapMode.Once;
                            m_DictAnStaList[i * 2 + 1].wrapMode = WrapMode.Once;

                            //动画结束后
                            if (frame < 0.3f)
                            {
                                int num = m_TargetPos[i] - 1;
                                Destroy(m_AnimationList[i * 2]);
                                Destroy(m_AnimationList[i * 2 + 1]);
                                Vector3 vec = m_MahjongDice[num];
                                vec.y = Random.Range(-180, 180);
                                m_DiceTargetList[i].transform.eulerAngles = vec;
                                m_DiceTargetList[i].transform.localPosition = m_DiceList[i].transform.localPosition;
                                m_DiceTargetList[i].transform.gameObject.SetActive(true);
                                m_DiceList[i].transform.gameObject.SetActive(false);

                                frame = 0;
                                zhuanTime = 1;
                            }
                            //else
                            //{
                            //    frame = 0.3f;
                            //}
                        }
                    }
                }
            }

            yield return null;
        }
    }
    #endregion

}

public enum E_CreateMahJongState
{
    None,
    Start,
    Show,
    End,
}
