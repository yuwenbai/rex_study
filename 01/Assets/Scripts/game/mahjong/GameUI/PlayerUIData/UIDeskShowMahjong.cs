using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{

    public class UIDeskShowMahjong : MonoBehaviour
    {

        public UIWidget Widget_Best = null;
        public UITable table_Best = null;
        public UIGrid grid_Single = null;
        public UIWidget widget_BG = null;
        public GameObject obj_Single = null;

        private int m_Width = 31;
        private int m_WInterval = 5;
        private int m_Height = 46;
        private int m_HInterval = 1;

        public void InitShow(int seatid, List<int> mahjongList)
        {
            this.Clear();
            seatid += 1;

            UIWidget uiw = grid_Single.GetComponent<UIWidget>();
            switch (seatid)
            {
                case 2:
                    Widget_Best.pivot = UIWidget.Pivot.Right;
                    grid_Single.pivot = UIWidget.Pivot.Right;
                    break; ;
                case 3:
                    Widget_Best.pivot = UIWidget.Pivot.TopRight;
                    grid_Single.pivot = UIWidget.Pivot.Right;
                    break;
                case 1:
                case 4:
                    Widget_Best.pivot = UIWidget.Pivot.Left;
                    grid_Single.pivot = UIWidget.Pivot.Left;
                    break;
                default:
                    break;
            }

            int count = mahjongList.Count;
            if (count > 9)
                count = 9;

            for (int i = 0; i < count; i++)
            {
                UIBestRecordItem item = GameTools.InstantiatePrefabAndReturnComp<UIBestRecordItem>(obj_Single, grid_Single.transform);
                item.IniCommonCard(mahjongList[i], EnumMjSpecialType.Null, -1);
                item.gameObject.SetActive(true);
            }

            int widNum = count > 2 ? 3 : count;
            Widget_Best.width = m_Width * widNum + (widNum - 1) * m_WInterval;

            int heiNum = count / 3f % 1 > 0 ? count / 3 + 1 : count / 3;
            Widget_Best.height = m_Height * heiNum + (heiNum - 1) * m_HInterval;

            switch (seatid)
            {
                case 2:
                    grid_Single.transform.localPosition = new Vector3(-m_Width, 0, 0);
                    break;
                case 3:
                    grid_Single.transform.localPosition = new Vector3(-m_Width, -Widget_Best.height / 2, 0);
                    break;
                default:
                    grid_Single.transform.localPosition = new Vector3(0, 0, 0);
                    break;
            }

            if (this.isActiveAndEnabled)
            {
                StartCoroutine(UpdateGrid());
            }
        }

        IEnumerator UpdateGrid()
        {
            grid_Single.gameObject.SetActive(false);
            yield return null;
            grid_Single.gameObject.SetActive(true);
            yield return null;
            grid_Single.repositionNow = true;
            //table_Best.repositionNow = true;
            yield return null;
            grid_Single.Reposition();
            //table_Best.Reposition();
        }


        public void ClearRecord()
        {
            grid_Single.transform.DestroyChildren();
            Destroy(this.gameObject);
        }


        public void Clear()
        {
            grid_Single.transform.DestroyChildren();
        }

    }


}
