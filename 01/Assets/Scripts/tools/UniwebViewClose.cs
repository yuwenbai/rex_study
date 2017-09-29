using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UniwebViewClose : MonoBehaviour
    {
        public UIWidget Bg;
        public UIWidget btnClose;
        public GameObject btnCloseCollider;

        public UIWidget btnBack;
        public GameObject btnBackCollider;

        private void Awake()
        {
            UIEventListener.Get(btnCloseCollider).onClick = closeUniweb;
            UIEventListener.Get(btnBackCollider).onClick = backUniWeb;
        }

        void Start()
        {
            var v = Bg.GetComponent<UIWidget>();
            Transform tempTf = UITools.GetNearestPanel(this.gameObject).transform;
            v.updateAnchors = UIRect.AnchorUpdate.OnUpdate;

            v.leftAnchor.target = tempTf;
            v.leftAnchor.relative = 0;
            v.leftAnchor.absolute = 0;

            v.rightAnchor.target = tempTf;
            v.rightAnchor.relative = 1;
            v.rightAnchor.absolute = 0;

            v.bottomAnchor.target = tempTf;
            v.bottomAnchor.relative = 1;
            v.bottomAnchor.absolute = -100;

            v.topAnchor.target = tempTf;
            v.topAnchor.relative = 1;
            v.topAnchor.absolute = 0;

            v.ResetAnchors();
            StartCoroutine(UITools.WaitExcution(() =>
            {
                UpdateCollider(btnClose);
                UpdateCollider(btnBack);
            }, 1));
        }

        public void closeUniweb(GameObject go)
        {
#if UNITY_ANDROID || UNITY_IOS
            AndroidManager.Instance.HiddenWebView();
#endif
        }

        public void backUniWeb(GameObject go)
        {
#if UNITY_ANDROID || UNITY_IOS
            AndroidManager.Instance.BackWebView();
#endif
        }

        public void UpdateCollider(UIWidget taget)
        {
            taget.ResizeCollider();
            taget.GetComponent<BoxCollider>().size *= 1.4f;
        }

        public void OnEnable()
        {
            MemoryData.GameStateData.IsOpenWebView = true;
        }

        public void OnDisable()
        {
            MemoryData.GameStateData.IsOpenWebView = false;
        }
    }
}
