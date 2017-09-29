

using System;
/**
* @Author JEFF
*
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace projectQ
{
    public class QSceneManager : BaseManager
    {
        private SceneBase m_State;

        /// <summary>
        /// 取得当前场景Name
        /// </summary>
        /// <returns></returns>
        public string GetCurrSceneName()
        {
            return m_State.StateName;
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="State">所需要的代码</param>
        /// <param name="LoadSceneName">场景名称</param>
        public void SetScene(SceneBase State, string LoadSceneName)
        {
            //加载场景
            _R.Instance.StartCoroutine(LoadScene(State,LoadSceneName));
        }

        // 加载场景
        private IEnumerator LoadScene(SceneBase State, string LoadSceneName)
        {
             if (m_State != null)
                m_State.StateEnd();

            //设置场景
            m_State = State;

            SceneManager.LoadScene("Empty");
            yield return null;
            if(!string.IsNullOrEmpty(LoadSceneName))
            {
                SceneManager.LoadScene(LoadSceneName);
                yield return null;
            }
            // 通知新的场景
            if (m_State != null)
            {
                m_State.StateBegin();
            }
        }

        public override void Init()
        {
        }
        public override void Dispose()
        {
        }
    }

    public class SceneBase
    {
        private string m_StateName = "SceneBase";
        public string StateName
        {
            get { return m_StateName; }
            set { m_StateName = value; }
        }

        public virtual void StateBegin()
        {
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysScene_Open, StateName);
        }

        public virtual void StateEnd()
        {
            WindowUIManager.Instance.CloseAll();
            EventDispatcher.FireSysEvent(GEnum.NamedEvent.SysScene_Close, StateName);
        }

    }
}
