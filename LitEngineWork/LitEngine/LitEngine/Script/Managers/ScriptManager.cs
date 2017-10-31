using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;
using ILRuntime.CLR.TypeSystem;
namespace LitEngine
{
    using ReaderAndWriterTool;
    public enum UseScriptType
    {
        UseScriptType_System = 1,
        UseScriptType_LS = 2,
    }
    public class ScriptManager: ManagerInterface
    {
        public ILRuntime.Runtime.Enviorment.AppDomain Env
        {
            get;
            private set;
        }
        private UseScriptType mUseSystemAssm = UseScriptType.UseScriptType_LS;
        private CodeToolBase mCodeTool;
        public CodeToolBase CodeTool
        {
            get
            {
                if (!ProjectLoaded)
                    DLog.LOG(DLogType.Error, "脚本系统还未载入任何脚本.请执行 LoadProject 系列方法.");
                return mCodeTool;
            }
        }
        public ScriptManager(UseScriptType _stype)
        {
            mUseSystemAssm = _stype;
            switch (mUseSystemAssm)
            {
                case UseScriptType.UseScriptType_LS:
                    Env = new ILRuntime.Runtime.Enviorment.AppDomain();
                    mCodeTool = new CodeTool_LS(Env);
                    break;
                case UseScriptType.UseScriptType_System:
                    mCodeTool = new CodeTool_SYS();
                    break;
            }
        }
        #region 释放
        bool mDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool _disposing)
        {
            if (mDisposed)
                return;

            if (_disposing)
                DisposeNoGcCode();

            mDisposed = true;
        }

        virtual protected void DisposeNoGcCode()
        {
            mCodeTool.Dispose(true);
        }

        ~ScriptManager()
        {
            Dispose(false);
        }
        #endregion

        public void DestroyManager()
        {
            Dispose(true);
        }

        public bool ProjectLoaded
        {
            get;
            private set;
        }

        public void LoadProject(string _PathFile)
        {
            try
            {
                var dll = System.IO.File.ReadAllBytes(_PathFile + ".dll");
                var pdb = System.IO.File.ReadAllBytes(_PathFile + ".pdb");
                LoadProjectByBytes(dll, pdb);
            }
            catch (Exception err)
            {
                DLog.LOG(DLogType.Error,"LoadProject" + err);
            }


        }

        public void LoadProjectFromPacket(string _PathFile)
        {
            try
            {
                using (FileStream tfile = File.OpenRead(_PathFile))
                {
                    Reader treader = new Reader(tfile, true);
                    int len = treader.ReaderSteam.ReadInt32();
                    byte[] tbuffer = new byte[len];
                    treader.ReaderSteam.Read(tbuffer, 0, len);
                    treader.End();

                    byte[] tdllbyts = null;
                    byte[] tpdbbyts = null;
                    Stream tstream = new MemoryStream(tbuffer);
                    #region build ProjectList

                    using (ZipInputStream f = new ZipInputStream(tstream))
                    {

                        while (true)
                        {
                            ZipEntry zp = f.GetNextEntry();
                            if (zp == null) break;
                            if (!zp.IsDirectory && zp.Crc != 00000000L)
                            {

                                byte[] b = new byte[f.Length];
                                int treadlen = f.Read(b, 0, b.Length);

                                //取得文件所有数据
                                if (zp.Name.Contains(".dll"))
                                    tdllbyts = b;
                                else if (zp.Name.Contains(".pdb"))
                                    tpdbbyts = b;
                            }
                        }
                        f.Close();
                    }
                    #endregion
                    tstream.Close();
                    tfile.Close();

                    LoadProjectByBytes(tdllbyts, tpdbbyts);
                }
            }
            catch (Exception err)
            {
                DLog.LOG(DLogType.Error,"loadpacket" + err);
            }

        }

        public void LoadProjectByBytes(byte[] _dll, byte[] _pdb)
        {
            try
            {
                switch (mUseSystemAssm)
                {
                    case UseScriptType.UseScriptType_LS:
                        {
                            System.IO.MemoryStream msDll = new System.IO.MemoryStream(_dll);
                            System.IO.MemoryStream msPdb = new System.IO.MemoryStream(_pdb);
                            Env.LoadAssembly(msDll, msPdb, new Mono.Cecil.Pdb.PdbReaderProvider());
                        }
                        break;
                    case UseScriptType.UseScriptType_System:
                        {
                            // byte[] pdbbytes = msPdb.ToArray();
                            Assembly assem = Assembly.Load(_dll);
                            ((CodeTool_SYS)CodeTool).AddAssemblyType(assem);

                        }
                        break;
                }
                ProjectLoaded = true;
            }
            catch (Exception err)
            {
                DLog.LOG(DLogType.Error,"加载脚本出现错误:" + err);
            }


        }

       
    }
}

