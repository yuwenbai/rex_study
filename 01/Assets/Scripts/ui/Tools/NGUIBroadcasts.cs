/**
 * @Author YQC
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace projectQ
{
    public class NGUIBroadcasts : MonoBehaviour {
        public GameObject Root;
        public int OneShowNum = 1;
        public float Speed = 1f;
        private Queue<NGUIBroadcastsData> waitQueue = new Queue<NGUIBroadcastsData>();
        private List<NGUIBroadcastsData> showList = new List<NGUIBroadcastsData>();
        private Queue<GameObject> Pool = new Queue<GameObject>();
        public UIScrollView scrollview;
        public GameObject Prefab;
        private NGUIBroadcastsData CurrData;

        

        public void Push(NGUIBroadcastsData item)
        {
            waitQueue.Enqueue(item);
            StartBroadcasts();
        }
        public void Clear()
        {
            waitQueue.Clear();
        }
        //控制跑马灯显隐
        private bool isShow = true;
        public void SetBroadcastShow(bool _show)
        {
            isShow = _show;
            Root.SetActive(isShow);
        }
        private void StartBroadcasts()
        {
            if (isShow == false) return;
            if (!Check()) return;

            //将待显示的放入显示队列
            if (showList.Count < OneShowNum)
            {
                //初始化位置 ScrollView_size / 2 + UILabel_Size / 2
                var instartDataItem = waitQueue.Dequeue();
                if (instartDataItem.LoopCount-- > 0)
                {
                    waitQueue.Enqueue(instartDataItem);
                }
                GameObject go = PopItem();
                ItemDataAssignment(instartDataItem, go);
            }
        }

        private void ItemDataAssignment(NGUIBroadcastsData data,GameObject go)
        {
            UILabel label = go.GetComponent<UILabel>();
            label.text = data.value;

            data.tf = go.transform;
            data.wiget = label;

            data.LeftRightBorder = GetLeftRightBorder(scrollview, data.wiget);
            //设置起始位置
            data.tf.localPosition = new Vector3(data.LeftRightBorder.y, 0, 0);

            //速度
            float tempSpeed = data.Speed;
            tempSpeed = Speed * (tempSpeed > 0 ? tempSpeed : 1f);
            CurrData = data;
            DOTween.Kill(data.tf, true);
            Tweener tweener = data.tf.DOLocalMoveX(data.LeftRightBorder.x, tempSpeed).SetEase(Ease.Linear).SetSpeedBased().OnComplete(TweenEnd).SetUpdate(UpdateType.Late);
            showList.Add(data);
        }

        private void TweenEnd()
        {
            int index = showList.IndexOf(CurrData);
            if (index >= 0)
            {
                PushItem(showList[index].tf.gameObject);
                showList.RemoveAt(index);
            }
            StartBroadcasts();
        }

        private GameObject PopItem()
        {
            GameObject go = null;
            if (Pool.Count > 0)
                go = Pool.Dequeue();

            if(go == null)
            {
                go = UITools.CloneObject(Prefab, scrollview.gameObject);
            }
            go.SetActive(true);
            return go;
        }
        private void PushItem(GameObject go)
        {
            go.SetActive(false);
            Pool.Enqueue(go);
        }

        private bool Check()
        {
            bool result = waitQueue.Count != 0 || showList.Count != 0;
                
            if (result != Root.activeSelf)
                Root.SetActive(result);

            return result;
        }

        /// <summary>
        /// 取得左右边界
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="wiget"></param>
        /// <returns>x 左边界 y右边界</returns>
        private Vector2 GetLeftRightBorder(UIScrollView sv, UIWidget wiget)
        {
            UIPanel panel = sv.panel;
            
            float right = (float)panel.GetViewSize().x / 2 + (float)wiget.width / 2;
            return new Vector2(-right, right);
        }

        #region 生命周期
        private void Awake()
        {
            Root.SetActive(false);
        }
        #endregion

    }

    [System.Serializable]
    public class NGUIBroadcastsData
    {
        public string value;
        public Transform tf;
        public UIWidget wiget;
        public float Speed;
        public Vector2 LeftRightBorder;
        //[Tooltip("循环次数")]
        public int LoopCount = 1;

        public NGUIBroadcastsData()
        {
        }
        public NGUIBroadcastsData(string value,float Speed, int loopCount = 1)
        {
            this.value = value;
            this.LoopCount = loopCount;
            this.Speed = Speed;
        }
      
    }
}
