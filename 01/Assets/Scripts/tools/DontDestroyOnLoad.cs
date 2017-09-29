using UnityEngine;
using System.Collections;
namespace projectQ {
    public class DontDestroyOnLoad : MonoBehaviour {
        // Use this for initialization
        void Start() {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }
}
