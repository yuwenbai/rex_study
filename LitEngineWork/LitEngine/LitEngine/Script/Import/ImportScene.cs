
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace LitEngine
{
    using ReaderAndWriterTool;
    using Loader;
    public enum ObjectAssetsLoadType
    {
        ObjectAssetsType_Have = 1,
        ObjectAssetsType_ParentHave,
        ObjectAssetsType_NotHave,
        ObjectAssetsType_HaveModel,
        ObjectAssetsType_ParentHaveModel,
    }
    public class ImPortScene
    {
        protected string mAppname;
        protected LoaderManager mLManager;

        static string sSceneNameB = ".sc";
        static string sSceneDBNameB = ".db";
        Reader mReader;
        BinaryReader mBinary;
        string mSceneName = "";
        LoaderCreat mCreat;
        ImPortBase mLoadObject;
        System.Action<string> mLoadFinishCall = null;
        LoadSceneMode mMode;
        public ImPortScene(string _appname)
        {
            mAppname = _appname;
            mLManager = AppCore.App[mAppname].LManager;
        }
        public LoaderCreat SetLoadLevelConfig(string _scenenme, LoadSceneMode _mode, System.Action<string> _finishcall, System.Action<string, float> _progress)
        {
            mMode = _mode;
            mLoadFinishCall = null;
            mLoadFinishCall += _finishcall;
            mSceneName = _scenenme;
            mCreat = new LoaderCreat(mLManager,_scenenme, LoadAllFinished, _progress);
            return mCreat;
        }
        public void StartLoadLevelAsync()
        {
            string tscname = mSceneName + sSceneNameB;
            mCreat.Add("scene", tscname, tscname, null, null);
            string tfullname = LoaderManager.GetDataPath() + mSceneName + sSceneDBNameB;
            if (!File.Exists(tfullname))
                tfullname = LoaderManager.GetStreamingDataPath() + mSceneName + sSceneDBNameB;
            mLManager.WWWLoad(mSceneName, tfullname, LoadScenedbCall, null, null);

        }
        #region 回调
        void LoadScenedbCall(string ObjectKey, object _Res, object _Tar)
        {
            Reader tReader = new Reader(((WWW)_Res).bytes, true);
            mLoadObject = Load(tReader);
            if (mLoadObject != null)
                mLoadObject.AddAssetsToCreat(mCreat);
            mCreat.StartLoad();
        }
        void LoadAllFinished(string _key)
        {
            ScriptTool.LoadSceneAsync(mAppname,mSceneName, CreatSceneCall, mMode);
        }
        void CreatSceneCall()
        {

            if (mMode == LoadSceneMode.Additive)
            {
                Scene tscene = SceneManager.GetSceneByName(mSceneName);
                SceneManager.SetActiveScene(tscene);
            }
            if (mLoadObject != null)
            {
                mLoadObject.CreatObject();
                mLoadObject.ChoseResouse();
            }
            mCreat.RemoveAssets(false);
            mCreat = null;
            ScriptTool.RestShaderOnEditer(RenderSettings.skybox);
            ScriptTool.WaitSecondCall(mAppname,LoacEndCall, 0.5f);
        }

        void LoacEndCall()
        {
            if(mLoadFinishCall!=null)
                mLoadFinishCall(mSceneName);  
        }

        #endregion
        #region 加载场景数据
        public ImPortBase LoadSceneData(string _filename)
        {
            Reader tReader = new Reader(_filename, true);
            return Load(tReader);
        }
        public ImPortBase LoadSceneDataByByteArry(byte[] _byte)
        {
            Reader tReader = new Reader(_byte, true);
            return Load(tReader);
        }
        public ImPortBase LoadSceneDataByStream(Stream _stream)
        {
            Reader tReader = new Reader(_stream, true);
            return Load(tReader);
        }

        ImPortBase Load(Reader _reader)
        {
            if (_reader == null || _reader.ReaderSteam == null) return null;
            mReader = _reader;
            mBinary = mReader.ReaderSteam;
            ImPortGameObject tLoadObject = new ImPortGameObject();
            ImportObjectList(tLoadObject);
            mReader.End();
            return tLoadObject;
        }


        void ImportObjectList(ImPortGameObject _object)
        {
            ImportObjectAssets(_object);
            _object.mIsStatic = mBinary.ReadBoolean();
            _object.mIndex = mBinary.ReadInt32();
            ImportPfbProperty(_object);
            ImporTransform(_object);

            bool isrender = mBinary.ReadBoolean();
            if (isrender)
            {
                bool ismat = mBinary.ReadBoolean();
                if (ismat)
                    ImportGameObjectMats(_object);

                _object.mLightmapIndex = mBinary.ReadInt32();
                _object.mLightmapScaleOffset.x = mBinary.ReadSingle();
                _object.mLightmapScaleOffset.y = mBinary.ReadSingle();
                _object.mLightmapScaleOffset.z = mBinary.ReadSingle();
                _object.mLightmapScaleOffset.w = mBinary.ReadSingle();

                _object.mRealtimeLightmapScaleOffset.x = mBinary.ReadSingle();
                _object.mRealtimeLightmapScaleOffset.y = mBinary.ReadSingle();
                _object.mRealtimeLightmapScaleOffset.z = mBinary.ReadSingle();
                _object.mRealtimeLightmapScaleOffset.w = mBinary.ReadSingle();
            }

            int tchildcount = mBinary.ReadInt32();

            for (int i = 0; i < tchildcount; i++)
            {
                ImPortGameObject tobject = new ImPortGameObject();
                ImportObjectList(tobject);
                _object.AddChild(tobject);
            }

        }
        void ImportObjectAssets(ImPortBase _object)
        {
            _object.mKey = mBinary.ReadString();
            _object.mName = mBinary.ReadString();
            _object.mClassType = mBinary.ReadString();
        }

        void ImportPfbProperty(ImPortGameObject _Object)
        {
            _Object.mAssetsLoadType = mBinary.ReadInt32();
            switch ((ObjectAssetsLoadType)_Object.mAssetsLoadType)
            {
                case ObjectAssetsLoadType.ObjectAssetsType_Have:
                    {
                        _Object.mAssetsName = mBinary.ReadString();
                    }
                    break;
                case ObjectAssetsLoadType.ObjectAssetsType_HaveModel:
                    {

                    }
                    break;
                case ObjectAssetsLoadType.ObjectAssetsType_ParentHaveModel:
                    {

                    }
                    break;
            }
        }

        void ImporTransform(ImPortGameObject _Object)
        {
            _Object.mLocalPostion.x = mBinary.ReadSingle();
            _Object.mLocalPostion.y = mBinary.ReadSingle();
            _Object.mLocalPostion.z = mBinary.ReadSingle();

            _Object.mLocalRotion.x = mBinary.ReadSingle();
            _Object.mLocalRotion.y = mBinary.ReadSingle();
            _Object.mLocalRotion.z = mBinary.ReadSingle();
            _Object.mLocalRotion.w = mBinary.ReadSingle();

            _Object.mLocalScal.x = mBinary.ReadSingle();
            _Object.mLocalScal.y = mBinary.ReadSingle();
            _Object.mLocalScal.z = mBinary.ReadSingle();
        }
        void ImportGameObjectMats(ImPortGameObject _parentObj)
        {
            _parentObj.mMatCount = mBinary.ReadInt32();
            for (int i = 0; i < _parentObj.mMatCount; i++)
            {
                ImPortMaterial tmatobj = new ImPortMaterial
                {
                    mIndex = mBinary.ReadInt32()
                };
                ImportMat(tmatobj);
                _parentObj.AddChild(tmatobj);
            }
        }
        void ImportMat(ImPortMaterial _matobj)
        {
            ImportObjectAssets(_matobj);
            _matobj.mAssetsName = mBinary.ReadString();
            ImportTextureDataFormMat(_matobj);
        }
        void ImportTextureDataFormMat(ImPortMaterial _matobj)
        {
            int texcount = mBinary.ReadInt32();
            for (int i = 0; i < texcount; i++)
            {
                ImPortTexture ttexobj = new ImPortTexture();
                ImportTexture(ttexobj);
                _matobj.AddChild(ttexobj);
            }
        }
        void ImportTexture(ImPortTexture _texobj)
        {
            ImportObjectAssets(_texobj);
            _texobj.mAssetsName = mBinary.ReadString();
            _texobj.mShaderKey = mBinary.ReadString();
        }
        #endregion
    }
}

