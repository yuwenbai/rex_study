/**
 * @Author Xin.Wang
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace projectQ
{
    public class MjBiaoUVControl : MonoBehaviour
    {
        public MeshFilter _mesh;
        public MeshRenderer _renderer = null;

        private GameObject _selfObj = null;
        public GameObject SelfObj
        {
            get
            {
                if (_selfObj == null)
                {
                    _selfObj = gameObject;
                }
                return _selfObj;
            }
        }

        public int idx = -1;

        private Vector2[] uv = null;

        public void IniUV(int index, bool isTip = false)
        {
            if (idx == index)
            {
                return;
            }

            idx = index;
            updateUV(index - 1);
        }

        void updateUV(int myIdx)
        {
            if (uv == null)
            {
                uv = new Vector2[_mesh.mesh.uv.Length];
                System.Array.Copy(_mesh.mesh.uv, uv, _mesh.mesh.uv.Length);
            }

            Vector2 zuoshangBase = Vector2.up;

            float offX = 0.25f * (myIdx % 4);
            float offY = 0.25f * (myIdx / 4);

            Vector2 newZuoshang = zuoshangBase;
            newZuoshang.x += offX;
            newZuoshang.y -= offY;

            var n_uv = new Vector2[uv.Length];
            System.Array.Copy(uv, n_uv, n_uv.Length);

            n_uv[0].x = newZuoshang.x;
            n_uv[0].y = newZuoshang.y - 0.25f;

            n_uv[1].x = newZuoshang.x + 0.25f;
            n_uv[1].y = newZuoshang.y;

            n_uv[2].x = newZuoshang.x + 0.25f;
            n_uv[2].y = newZuoshang.y - 0.25f;

            n_uv[3] = newZuoshang;

            _mesh.mesh.uv = n_uv;
        }

    }

}

