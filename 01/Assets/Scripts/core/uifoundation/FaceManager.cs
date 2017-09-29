using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace projectQ {
    public class FaceManager : SingletonTamplate<FaceManager> {

        public FaceManager() {
            m_buffer = new Dictionary<string, UIFace>();
        }

        private IDictionary<string, UIFace> m_buffer;

        public IDictionary<string, UIFace> BufferFace {
            get { return m_buffer; }
        }

        public void addFace(UIFace face) {
            if (m_buffer.ContainsKey(face.faceName)) {
                QLoger.ERROR("缓冲区中包含有同名的Face,不能添加同名的Face");
            } else {
                m_buffer.Add(face.faceName, face);
            }
        }

        public UIFace getFace(string name) {
            UIFace f = null;
            if (m_buffer.TryGetValue(name, out f)) {
                return f;
            }
            return null;
        }

        public void deleteFace(string name) {
            if (m_buffer.ContainsKey(name)) {
                m_buffer.Remove(name);
            }
        }

        public void clearFace() {
            m_buffer.Clear();
        }

        public override string ToString() {
            return string.Format("[FaceManager] {0}", CommonTools.ReflactionObject(this));
        }
    }
}