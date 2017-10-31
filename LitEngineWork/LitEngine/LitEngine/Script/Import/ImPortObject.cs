using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LitEngine
{
    using Loader;
    public class ImPortBase
    {
        protected LoaderManager mLManager;

        public string mName;
        public string mKey;
        public string mClassType;
        public int mIndex;

        public string mAssetsName;

        public Object mAssets = null;

        public Object mCreatObject;
        public ImPortBase mParent;

        protected System.Type mType;
        protected List<ImPortBase> mChild = null;
        public ImPortBase()
        {

        }

        virtual public void AddChild(ImPortBase _obj)
        {
            _obj.mParent = this;
            mChild.Add(_obj);
        }
        virtual public void ChoseResouse()
        {
            ChoseChild();
        }
        virtual public void ChoseChild()
        {
            if (mChild == null) return;
            foreach (ImPortBase tobj in mChild)
                tobj.ChoseResouse();
        }
        virtual public void CreatObject()
        {
            if (mChild == null) return;
            foreach (ImPortBase tobj in mChild)
                tobj.CreatObject();
        }
        virtual protected void LogErrorParentType()
        {
            DLog.LOG(DLogType.Error,"错误的父对象:" + mParent.mCreatObject.GetType() + " AssetsName:" + mParent.mAssetsName + " Type:" + mType);
        }

        virtual public void RemoveAssets()
        {
            mLManager.RemoveBundle(mAssetsName, false);
            foreach (ImPortBase tobj in mChild)
            {
                tobj.RemoveAssets();
            }
        }
        virtual public void AddAssetsToCreat(LoaderCreat _creat)
        {
            _creat.Add(mName, mAssetsName, mAssetsName, LoadCallBack, null);
            if (mChild == null) return;
            foreach (ImPortBase tobj in mChild)
            {
                tobj.AddAssetsToCreat(_creat);
            }
        }

        virtual protected void LoadCallBack(string ObjectKey, object _Res, object _Tar)
        {
            mAssets = (Object)_Res;
        }
    }
    public class ImPortMaterial : ImPortBase
    {

        public ImPortMaterial() : base()
        {
            mType = typeof(Material);
            mChild = new List<ImPortBase>();
        }
        override public void ChoseResouse()
        {
            if (mParent != null && mParent.mCreatObject != null)
            {
                System.Type ttype = mParent.mCreatObject.GetType();
                if (ttype.Equals(typeof(GameObject)))
                {
                    GameObject tparent = (GameObject)mParent.mCreatObject;
                    Renderer trender = tparent.transform.GetComponent<Renderer>();

                    if (!ScriptTool.IsNull(trender) && trender.sharedMaterials != null && trender.sharedMaterials.Length > 0)
                    {
                        Material[] tmats = trender.sharedMaterials;
                        tmats[mIndex] = (Material)mCreatObject;
                        ScriptTool.RestShaderOnEditer(tmats[mIndex]);
                        trender.sharedMaterials = tmats;
                    }
                }
                else
                    LogErrorParentType();

            }

            base.ChoseResouse();
        }
        override public void CreatObject()
        {
            if (mAssets == null)
                DLog.LOG(DLogType.Error,"Mat资源为NULL");
            bool isInstantiate = mAssetsName.Contains("Instantiate");
            if (isInstantiate)
                mCreatObject = Object.Instantiate(mAssets);
            else
                mCreatObject = mAssets;
            base.CreatObject();
        }

    }
    public class ImPortTexture : ImPortBase
    {
        public string mShaderKey;
        public ImPortTexture() : base()
        {
            mType = typeof(Texture);
        }
        override public void ChoseResouse()
        {
            // Debug.Log(mParent+"---"+ mParent.mCreatObject + "---" + mCreatObject + "---" + mAssetsName + "1");
            if (mParent != null && mParent.mCreatObject != null && mCreatObject != null)
            {
                // Debug.Log(mAssetsName + "2" );
                System.Type ttype = mParent.mCreatObject.GetType();
                if (ttype.Equals(typeof(Material)))
                {
                    Material tparent = (Material)mParent.mCreatObject;
                    tparent.SetTexture(mShaderKey, (Texture)mCreatObject);
                }
                else
                    LogErrorParentType();

            }
            base.ChoseResouse();
        }
        override public void CreatObject()
        {
            mCreatObject = mAssets;
            base.CreatObject();
        }

    }
    public class ImPortGameObject : ImPortBase
    {
        public static bool mShowParticle = true;


        public bool mIsStatic;
        public int mAssetsLoadType;
        //GameObject
        public Vector3 mLocalPostion;
        public Quaternion mLocalRotion;
        public Vector3 mLocalScal;
        public int mLightmapIndex;
        public Vector4 mLightmapScaleOffset;
        public Vector4 mRealtimeLightmapScaleOffset;
        public int mMatCount;

        public bool mNeedAddHideComp = true;
        public bool mAddedHideComp = false;

        public ImPortGameObject() : base()
        {
            mType = typeof(GameObject);
            mChild = new List<ImPortBase>();
            mLocalPostion = Vector3.zero;
            mLocalRotion = new Quaternion();
            mLocalScal = Vector3.zero;
            mLightmapScaleOffset = Vector4.zero;
            mRealtimeLightmapScaleOffset = Vector4.zero;
        }



        #region 计算关系
        override public void ChoseResouse()
        {
            if (mCreatObject == null) return;
            if (mParent != null && ((ImPortGameObject)mParent).mAddedHideComp)
            {
                mNeedAddHideComp = false;
                mAddedHideComp = true;
            }
            ChoseGameObject();//计算自身
            base.ChoseResouse();//计算子
        }
        void ChoseGameObject()
        {
            GameObject tobj = (GameObject)mCreatObject;
            Renderer trender = tobj.transform.GetComponent<Renderer>();
            if (!ScriptTool.IsNull(trender))
            {
                if (mMatCount > 0)
                    trender.materials = new Material[mMatCount];
                trender.lightmapIndex = mLightmapIndex;
                trender.lightmapScaleOffset = mLightmapScaleOffset;
                trender.realtimeLightmapScaleOffset = mRealtimeLightmapScaleOffset;
                trender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                if (mNeedAddHideComp && !mAddedHideComp)
                {
                    ParticleSystem tpartic = tobj.transform.GetComponent<ParticleSystem>();

                    if (!ScriptTool.IsNull(tpartic))
                    {
                        tpartic.Stop();
                        if (!trender.enabled)
                        {
                            trender.enabled = true;
                            //tpartic.maxParticles = 0;
                        }

                        ScriptOnCullDisable treadcomp = tobj.GetComponent<ScriptOnCullDisable>();
                        if (ScriptTool.IsNull(treadcomp))
                        {
                            ScriptOnCullDisable tcomp = tobj.AddComponent<ScriptOnCullDisable>();
                            tcomp.mParticsystem = tpartic;
                        }
                        mAddedHideComp = true;
                    }
                }
            }
            tobj.isStatic = mIsStatic;
        }
        #endregion
        override public void CreatObject()
        {
            CreatGameObject();
            base.CreatObject();
        }

        void CreatGameObject()
        {
            if (mAssetsLoadType == 1)
            {
                if (mAssets == null)
                {
                    DLog.LOG(DLogType.Error,"建立对象所需要的资源为NULL:" + mAssetsName);
                    mCreatObject = new GameObject(mName);
                }
                else
                {
                    mCreatObject = Object.Instantiate(mAssets);
                    mCreatObject.name = mName;

                }

            }
            else if (mAssetsLoadType == 3)
                mCreatObject = new GameObject(mName);
            else
            {
                if (mParent != null && mParent.mCreatObject != null)
                {
                    System.Type ttype = mParent.mCreatObject.GetType();
                    if (ttype.Equals(typeof(GameObject)))
                    {
                        GameObject tobj = (GameObject)mParent.mCreatObject;
                        Transform trans = tobj.transform.FindChild(mName);
                        if (trans != null)
                            mCreatObject = trans.gameObject;
                        else
                        {
                            string tlog = tobj.name;
                            ImPortGameObject tlastobj = (ImPortGameObject)mParent.mParent;
                            while (tlastobj != null)
                            {
                                tlog = tlastobj.mName + "/" + tlog;
                                tlastobj = (ImPortGameObject)tlastobj.mParent;
                            }
                            DLog.LOG(DLogType.Error,tlog + "未能在子节点下找到节点（" + mName + "）");
                            mCreatObject = new GameObject(mName)
                            {
                                name = mName
                            };
                        }

                    }
                    else
                        LogErrorParentType();

                }
                else
                    mCreatObject = null;
            }

            GameObject tself = (GameObject)mCreatObject;
            if (tself.name.Equals("effect") || tself.name.Equals("texiao"))//需要与美术协商
            {
                tself.SetActive(mShowParticle);
            }
            Renderer trender = tself.transform.GetComponent<Renderer>();
            if (mAssetsLoadType != 2)
            {
                tself.transform.localPosition = mLocalPostion;
                tself.transform.localRotation = mLocalRotion;
                tself.transform.localScale = mLocalScal;

                if (mParent != null && mParent.mCreatObject != null)
                {
                    System.Type ttype = mParent.mCreatObject.GetType();
                    if (ttype.Equals(typeof(GameObject)))
                    {
                        GameObject tparent = (GameObject)mParent.mCreatObject;
                        tself.transform.SetParent(tparent.transform, false);
                    }
                    else
                        LogErrorParentType();

                }
            }

        }

        override public void AddAssetsToCreat(LoaderCreat _creat)
        {
            if (mAssetsLoadType == 1)
                _creat.Add( mName, mAssetsName, mAssetsName, LoadCallBack, null);
            foreach (ImPortBase tobj in mChild)
            {
                tobj.AddAssetsToCreat(_creat);
            }
        }
    }
}

