using UnityEngine;
using System.Collections;
namespace projectQ {
    public abstract class UIFace : MonoBehaviour {

        public string faceName;

        virtual public void init() {
            if (string.IsNullOrEmpty(faceName)) {
                faceName = this.gameObject.name;
            }
        }

        virtual public void open() {
            UITools.SetActive(this.gameObject);
            FaceManager.Instance.addFace(this);
        }

        virtual public void hidden() {
            UITools.SetActive(this.gameObject, false);
        }

        virtual public void show() {
            UITools.SetActive(this.gameObject);
        }

        virtual public void close() {
            FaceManager.Instance.deleteFace(this.faceName);
            GameObject.Destroy(this.gameObject);
        }
    }

}