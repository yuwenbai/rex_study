/****************************************************
 *  Copyright(C) 2015 智幻点击
 *	版权所有。
 *	作者:xiyongjian
 *	创建时间：11/05/2015
 *	文件名：  UITools.cs @ hc
 *	文件功能描述: A UI Helper Class
 *  创建标识：jeff.13/01/2016
 *	创建描述：is tools class , which is replace NGUITools , just for warpping some function useing for farther work.
 *
 *  修改标识：增加加载本地加载单图方法
 *  修改描述：修改路径为封装路径
 *
 *
 *
 *****************************************************/
using System.Text;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace projectQ {
    public class UITools {
        #region game object operator

        

        static public void ResetTransfrom(Transform trans) {
            if (trans != null) {
                trans.localPosition = Vector3.zero;
                trans.localRotation = Quaternion.identity;
                trans.localScale = Vector3.one;
            }
        }

        public static GameObject CloneObject(GameObject cloneTarget,
                                             GameObject parent,
                                             string name = null) {
            return CloneObject(cloneTarget,
                               parent,
                               name == null ? cloneTarget.name : name
                               , Vector3.zero,
                               Vector3.one,
                               Quaternion.identity);
        }

        public static GameObject CloneObject(GameObject cloneTarget,
                                             GameObject parent,
                                             string name,
                                             Vector3 localpz,
                                             Vector3 localsc,
                                             Quaternion localrt) {

            GameObject go = GameObject.Instantiate(cloneTarget) as GameObject;
            if (go != null) {
                Transform t = go.transform;

                if (parent != null) {
                    t.parent = parent.transform;
                    go.layer = parent.layer;
                }

                if (name != null) {
                    t.name = name;
                }
                t.localPosition = localpz;
                t.localRotation = localrt;
                t.localScale = localsc;
            } else {
                QLoger.ERROR("不能加载对象 {0} with name {1} ",
                               cloneTarget.name, name);
            }
            return go;
        }
        #endregion

        #region Tools
        /// <summary>
        /// 向上查找UIPanel
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static UIPanel GetNearestPanel(GameObject go) {
            UIPanel panel = go.GetComponent<UIPanel>();
            if (panel != null) {
                return panel;
            } else if (go.transform.parent != null) {
                return GetNearestPanel(go.transform.parent.gameObject);
            }
            return null;
        }

        /// <summary>
        /// 向上查找组件
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetNearestPanel<T>(GameObject go) where T : MonoBehaviour
        {
            T component = go.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            else if (go.transform.parent != null)
            {
                return GetNearestPanel<T>(go.transform.parent.gameObject);
            }
            return null;
        }

        /// <summary>
        /// 向下查找UIPanel
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static List<UIPanel> GetChildPanel(Transform tf)
        {
            List<UIPanel> uiPanelList = new List<UIPanel>();
            UIPanel panel = tf.GetComponent<UIPanel>();
            if(panel != null)
            {
                uiPanelList.Add(panel);
            }
            for(int i=0; i<tf.childCount; ++i)
            {
                List<UIPanel> tempList = GetChildPanel(tf.GetChild(i));
                if(tempList != null && tempList.Count > 0)
                {
                    uiPanelList.AddRange(tempList);
                }
            }
            return uiPanelList;
        }

        /// <summary>
        /// 创建子
        /// </summary>
        public static void CreateChild<T>(Transform parentTf,GameObject clone,List<T> dataList, System.Action<GameObject,T> func, bool isDestryAll = false)
        {
            CreateChild<T>(parentTf, clone, dataList, func, null, isDestryAll);
        }

        public static void CreateChild<T>(Transform parentTf, GameObject clone, List<T> dataList, System.Action<GameObject, int, T> func, bool isDestryAll = false)
        {
            CreateChild<T>(parentTf, clone, dataList, null, func, isDestryAll);
        }

        /// <summary>
        /// 创建子
        /// </summary>
        public static void CreateChild<T>(Transform parentTf, GameObject clone, List<T> dataList, System.Action<GameObject, T> func, System.Action<GameObject,int, T> func2, bool isDestryAll = false)
        {
            if (clone == null)
                clone = parentTf.GetChild(0).gameObject;

            if (isDestryAll)
            {
                parentTf.DestroyChildren();
            }

            for (int i = 0; i < parentTf.childCount || i < dataList.Count; ++i)
            {
                if (i >= dataList.Count)
                {
                    parentTf.GetChild(i).gameObject.SetActive(false);
                    continue;
                }
                GameObject go;
                if (i >= parentTf.childCount)
                {
                    go = UITools.CloneObject(clone, parentTf.gameObject);
                }
                else
                {
                    go = parentTf.GetChild(i).gameObject;
                }
                go.SetActive(true);
                if(func != null)
                {
                    func(go, dataList[i]);
                }
                if(func2 != null)
                {
                    func2(go, i, dataList[i]);
                }
            }
        }

        //创建子 无泛型
        public static void CreateChild(Transform parentTf, GameObject clone, int count ,System.Action<GameObject,int> func, bool isDestryAll = false)
        {
            if (clone == null)
            {
                return;
            }

            if (isDestryAll)
            {
                parentTf.DestroyChildren();
            }

            for (int i = 0; i < count; i++)
            {
                GameObject go = UITools.CloneObject(clone, parentTf.gameObject);
                go.SetActive(true);
                func(go, i);
            }
        }
        

        public static IEnumerator ResetScrollView(UIScrollView sv)
        {
            yield return null;
            sv.ResetPosition();
        }

        /*
        public static void SetAncher(UIWidget wigget, Transform ancher, UIAnchorPoint lp)
        {


            if (ancher == null) return;

            if (wigget != null)
            {
                wigget.leftAnchor.target = ancher;
                wigget.rightAnchor.target = ancher;
                wigget.topAnchor.target = ancher;
                wigget.bottomAnchor.target = ancher;

                //计算偏移量
                wigget.leftAnchor.absolute = 0;
                wigget.rightAnchor.absolute = 0;
                wigget.topAnchor.absolute = 0;
                wigget.bottomAnchor.absolute = 0;

                //计算锚点
                //Anchor.relative =  0 left , botton ,0.5 center  1 right , top 
                switch (lp)
                {
                    case UIAnchorPoint.Center:
                        {
                            wigget.leftAnchor.relative = 0.5f;
                            wigget.rightAnchor.relative = 0.5f;
                            wigget.topAnchor.relative = 0.5f;
                            wigget.bottomAnchor.relative = 0.5f;
                        }
                        break;

                    case UIAnchorPoint.CenterBottom:
                        {
                            wigget.leftAnchor.relative = 0.5f;
                            wigget.rightAnchor.relative = 0.5f;
                            wigget.topAnchor.relative = 0f;
                            wigget.bottomAnchor.relative = 0f;
                        }
                        break;
                    case UIAnchorPoint.CenterTop:
                        {
                            wigget.leftAnchor.relative = 0.5f;
                            wigget.rightAnchor.relative = 0.5f;
                            wigget.topAnchor.relative = 1f;
                            wigget.bottomAnchor.relative = 1f;
                        }
                        break;

                    case UIAnchorPoint.LeftBottom:
                        {
                            wigget.leftAnchor.relative = 0f;
                            wigget.rightAnchor.relative = 0f;
                            wigget.topAnchor.relative = 0f;
                            wigget.bottomAnchor.relative = 0f;
                        }
                        break;
                    case UIAnchorPoint.LeftCenter:
                        {
                            wigget.leftAnchor.relative = 0f;
                            wigget.rightAnchor.relative = 0f;
                            wigget.topAnchor.relative = 0.5f;
                            wigget.bottomAnchor.relative = 0.5f;
                        }
                        break;
                    case UIAnchorPoint.LeftTop:
                        {
                            wigget.leftAnchor.relative = 0f;
                            wigget.rightAnchor.relative = 0f;
                            wigget.topAnchor.relative = 1f;
                            wigget.bottomAnchor.relative = 1f;
                        }
                        break;

                    case UIAnchorPoint.RightBottom:
                        {
                            wigget.leftAnchor.relative = 1f;
                            wigget.rightAnchor.relative = 1f;
                            wigget.topAnchor.relative = 0f;
                            wigget.bottomAnchor.relative = 0f;
                        }
                        break;
                    case UIAnchorPoint.RightCenter:
                        {
                            wigget.leftAnchor.relative = 1f;
                            wigget.rightAnchor.relative = 1f;
                            wigget.topAnchor.relative = 0.5f;
                            wigget.bottomAnchor.relative = 0.5f;
                        }
                        break;
                    case UIAnchorPoint.RightTop:
                        {
                            wigget.leftAnchor.relative = 1f;
                            wigget.rightAnchor.relative = 1f;
                            wigget.topAnchor.relative = 1f;
                            wigget.bottomAnchor.relative = 1f;
                        }
                        break;
                }
            }
        }
        */


            /// <summary>
            /// Sets the active.
            /// </summary>
            /// <param name="go">Go.</param>
            /// <param name="state">If set to <c>true</c> state.</param>
        public static void SetActive(GameObject go, bool state = true) {
            NGUITools.SetActive(go, state, false);
        }

        public static void SetActive(MonoBehaviour mono, bool state = true) {
            SetActive(mono.gameObject, state);
        }
        public static void SetAlpha(GameObject go, bool state = true) {
            SetAlpha(go, state, true);
        }
        static public void SetAlpha(GameObject go, bool state, bool compatibilityMode) {
            if (go) {
                UIWidgetAlpha(go.transform, state);
            }
        }

        static void UIWidgetAlpha(Transform t, bool state) {
            UIWidget wiget = t.GetComponent<UIWidget>();
            if (wiget != null) {
                wiget.alpha = state ? 1 : 0;
            }
            for (int i = 0, imax = t.childCount; i < imax; ++i) {
                wiget = t.GetChild(i).GetComponent<UIWidget>();
                wiget.alpha = state ? 1 : 0;
            }
        }



        /// <summary>
        /// Makes the mask.
        /// </summary>
        /// <param name="go">Go.</param>
        /// <param name="depth">Depth.</param>
        public static UITexture MakeMask(GameObject go, float color = 0.5f, int depth = 0) {

            UITexture mask = NGUITools.AddWidget<UITexture>(go);
            mask.name = string.Format("{0}_mask", go.name);
            mask.mainTexture = Texture2D.whiteTexture;
            mask.color = new Color(0, 0, 0, color); //设置为纯黑色 附带半透明
            mask.depth = depth;
            mask.width = Screen.width * 5;
            mask.height = Screen.height * 5;
            NGUITools.AddWidgetCollider(mask.gameObject);
            return mask;
        }
        public static UIWidget ShowMask(GameObject go,UIWidget BaseMask = null,int depth = -99,float alpha = 0.68f)
        {
            if (BaseMask == null)
            {
                GameObject prefab = null;

				prefab = ResourcesDataLoader.Load<GameObject>("Shader/FrostedGlass");
				/*MARK By JEFF 不需要毛玻璃效果
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                prefab = ResourcesDataLoader.Load<GameObject>("Shader/FrostedGlass");
#else
                prefab = ResourcesDataLoader.Load<GameObject>("Shader/Glass1/Glass1");
#endif
			*/
                if (prefab != null)
                {
					BaseMask = UITools.CloneObject(prefab, go, "BaseMask").GetComponent<UISprite>();
                    BaseMask.updateAnchors = UIRect.AnchorUpdate.OnEnable;
                }                
            }

            Transform tempTf = UITools.GetNearestPanel(go).transform;
            BaseMask.transform.parent = go.transform;
            if (!BaseMask.enabled) BaseMask.enabled = true;
            BaseMask.depth = depth;
            BaseMask.leftAnchor.target = tempTf;
            BaseMask.leftAnchor.relative = 0;
            BaseMask.leftAnchor.absolute = -30;

            BaseMask.rightAnchor.target = tempTf;
            BaseMask.rightAnchor.relative = 1;
            BaseMask.rightAnchor.absolute = 30;

            BaseMask.bottomAnchor.target = tempTf;
            BaseMask.bottomAnchor.relative = 0;
            BaseMask.bottomAnchor.absolute = -30;

            BaseMask.topAnchor.target = tempTf;
            BaseMask.topAnchor.relative = 1;
            BaseMask.topAnchor.absolute = 30;
            BaseMask.ResetAnchors();
            BaseMask.alpha = alpha;

            return BaseMask;
        }



        public static void MakeEditorDialog(string msg, string title = "提示", string ok = "ok") {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog(title, msg, ok);
#endif
        }

        /// <summary>
        /// 等待制定帧后执行指定方法
        /// </summary>
        /// <returns>The excution.</returns>
        /// <param name="action">Action.</param>
        /// <param name="waitframe">Waitframe.</param>
        static public IEnumerator WaitExcution(System.Action action, int waitframe = 1) {
            while (waitframe-- > 0) {
                yield return new WaitForEndOfFrame();
            }
            action();
        }

        /// <summary>
        /// 等待制定时间后执行方法
        /// </summary>
        static public IEnumerator WaitExcution(System.Action action, float waitTime = 1)
        {
            yield return new WaitForSeconds(waitTime);
            action();
        }

        /// <summary>
        /// 等待制定时间后执行方法
        /// </summary>
        static public IEnumerator WaitExcution(System.Action<string> action, float waitTime = 1, string extendStr = "")
        {
            yield return new WaitForSeconds(waitTime);
            action(extendStr);
        }

        #endregion
    }
}