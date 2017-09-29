/**
 * @Author lyb
 * ScrollView 滑动到最低端的时候发送事件出去
 *
 */

using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class ScrollViewControl : MonoBehaviour
    {
        UIScrollView scrollView;

        void OnEnable()
        {
            Init();
            //getInfo();
            scrollView.onDragFinished += OnDragFinished;
        }

        void OnDisable()
        {
            scrollView.onDragFinished -= OnDragFinished;
        }

        void Start() { }

        void Init()
        {
            scrollView = transform.GetComponent<UIScrollView>();
        }

        void OnDragFinished()
        {
            scrollView.UpdateScrollbars(true);
            Vector3 constraint = scrollView.panel.CalculateConstrainOffset(scrollView.bounds.min, scrollView.bounds.min);
            if (constraint.y <= 1f)
            {
                C2CDragFinishDelegate();
            }
            flushRank();
        }

        void C2CDragFinishDelegate()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysUI_ScrollView_DragFinish);

            QLoger.LOG(" 创建 +++++++++++++++++++++++++++++ ");
        }

        void flushRank()
        {
            //grid.GetComponent<UIGrid>().Reposition();
            //grid.GetComponent<UIGrid>().repositionNow = true;
            //NGUITools.SetDirty(grid);
        }
    }
}