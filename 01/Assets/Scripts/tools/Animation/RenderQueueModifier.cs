using UnityEngine;
using System.Collections;

namespace projectQ
{
    public class RenderQueueModifier : MonoBehaviour
    {
        public enum RenderType
        {
            FRONT,
            BACK
        }

        public UIWidget m_target = null;
        public RenderType m_type = RenderType.FRONT;
        private Renderer[] _renderers;
        private UITexture _texture;
        private int _lastQueue = 0;

        public UITexture light1;
        public UITexture light2;
        public Animation thisAni;

        //void Awake(){}

        void OnEnable()
        {
            GameDelegateCache.Instance.InvokeMethodEvent += new GameDelegateCache.OnAddPerInvokeMethodEvent(this.RenderQueueCalculateValue);
            GameDelegateCache.Instance.SetEffectStateEvent += new GameDelegateCache.OnSetEffectStateEvent(this.SetEffectState);
            if (thisAni == null)
                return;

            thisAni.Play();
            light1.gameObject.SetActive(true);
            light2.gameObject.SetActive(true);
            StartCoroutine(EnDisAbelEffect());
        }

        void OnDisable()
        {
            GameDelegateCache.Instance.InvokeMethodEvent -= new GameDelegateCache.OnAddPerInvokeMethodEvent(this.RenderQueueCalculateValue);
            GameDelegateCache.Instance.SetEffectStateEvent -= new GameDelegateCache.OnSetEffectStateEvent(this.SetEffectState);
            RefreshEffect();
        }

        private IEnumerator EnDisAbelEffect()
        {
            yield return new WaitForSeconds(2f);
            if(thisAni != null)
            {
                thisAni.Stop();
                thisAni.enabled = false;
                light1.gameObject.SetActive(false);
                light2.gameObject.SetActive(false);
            }
        }

        private void RefreshEffect()
        {
            if (thisAni == null)
                return;
            thisAni.enabled = true;
        }

        void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in _renderers)
            {
                rend.enabled = GameConfig.Instance.IsEffectOpen;

                if (rend.gameObject.GetComponent<AnimationCtrl>() == null)
                {
                    rend.gameObject.AddComponent<AnimationCtrl>();
                }
            }

            _texture = GetComponent<UITexture>();
        }

        /*
        void FixedUpdate()
        {

        }
        */

        void RenderQueueCalculateValue()
        {
            //TODO:该脚本会有效率问题，需要进行测试

            if (m_target == null || m_target.drawCall == null)
            {
                return;
            }

            int queue = m_target.drawCall.renderQueue;
            queue += m_type == RenderType.FRONT ? 1 : -1;
            if (_lastQueue != queue)
            {
                _lastQueue = queue;
                foreach (Renderer r in _renderers)
                {
                    r.material.renderQueue = _lastQueue;
                }

                if (_texture != null)
                {
                    if (_texture.material != null)
                    {
                        _texture.material.renderQueue = _lastQueue;
                    }
                }
            }
        }

        /// <summary>
        /// 控制特效开关
        /// </summary>
        void SetEffectState()
        {
            if (_renderers != null)
            {
                foreach (Renderer rend in _renderers)
                {
                    rend.enabled = GameConfig.Instance.IsEffectOpen;
                }
            }            
        }
    }
}