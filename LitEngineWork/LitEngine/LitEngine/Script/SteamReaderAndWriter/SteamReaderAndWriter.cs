using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace LitEngine
{
    namespace ReaderAndWriterTool
    {
        #region 回调
        public delegate void WriterCallBack(ref BinaryWriter _steam);
        public delegate void ReaderCallBack(ref BinaryReader _steam);
        #endregion
        #region 基类
        public class ReaderAndWriterBase
        {
            public static byte[] mKeySc = Encoding.UTF8.GetBytes("Dcpyxczm");
            public static byte[] mIVSc = Encoding.UTF8.GetBytes("Dcpyxczm");
            protected string mFileName;
            protected bool mIsCrypt;
            protected FileStream mFile = null;
            protected CryptoStream mCst;
            public ReaderAndWriterBase(string _filename, bool _IsCrypt)
            {
                mFileName = _filename;
                mIsCrypt = _IsCrypt;
            }

            public virtual void End()
            {
            }

            static public void WriteString(string _filenameFullPath, string _data, bool _isCrypt)
            {

                FileStream flstr = File.Open(_filenameFullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

                StreamWriter sw = null;
                CryptoStream cst = null;
                if (_isCrypt)
                {

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(mKeySc, mIVSc), CryptoStreamMode.Write);
                    sw = new StreamWriter(cst);
                }
                else
                {
                    sw = new StreamWriter(flstr);
                }


                sw.Write(_data);
                sw.Flush();
                if (_isCrypt)
                    cst.FlushFinalBlock();
                sw.Close();
                flstr.Close();

            }

            static public string GetString(string _filenameFullPath, bool _isCrypt)
            {
                if (!File.Exists(_filenameFullPath))
                    return "";

                FileStream aFile = File.Open(_filenameFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = null;
                if (_isCrypt)
                {

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    CryptoStream cst = new CryptoStream(aFile, cryptoProvider.CreateDecryptor(mKeySc, mIVSc), CryptoStreamMode.Read);
                    sr = new StreamReader(cst);
                }
                else
                {
                    sr = new StreamReader(aFile);

                }

                string ret = sr.ReadToEnd();
                sr.Close();
                aFile.Close();
                return ret;

            }

            static public string GetUnCodeString(byte[] _EncodeString, bool isCypt)
            {
                string ret = "";
                StreamReader sr = null;
                if (isCypt)
                {
                    MemoryStream stream = new MemoryStream(_EncodeString);
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    CryptoStream cst = new CryptoStream(stream, cryptoProvider.CreateDecryptor(mKeySc, mIVSc), CryptoStreamMode.Read);
                    sr = new StreamReader(cst);
                }
                else
                {
                    MemoryStream stream = new MemoryStream(_EncodeString);
                    sr = new StreamReader(stream);
                }


                ret = sr.ReadToEnd();
                sr.Close();
                return ret;
            }
        }
        #endregion

        #region 读取
        public class Reader : ReaderAndWriterBase
        {
            private BinaryReader mReader;
            public Reader(string _filename, bool _IsCrypt) : base(_filename, _IsCrypt)
            {
                if (File.Exists(mFileName))
                {
                    mFile = new FileStream(mFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    InitStream(mFile);
                }
            }

            public Reader(Stream _stream, bool _IsCrypt) : base("", _IsCrypt)
            {
                InitStream(_stream);
            }

            public Reader(byte[] _bytes, bool _IsCrypt) : base("", _IsCrypt)
            {
                MemoryStream tstream = new MemoryStream(_bytes);
                InitStream(tstream);
            }

            public BinaryReader ReaderSteam
            {
                get
                {
                    return mReader;
                }
            }

            void InitStream(Stream _stream)
            {
                if (_stream == null)
                    return;
                if (mIsCrypt)
                {

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    cryptoProvider.Key = mKeySc;
                    cryptoProvider.IV = cryptoProvider.Key;
                    cryptoProvider.Mode = CipherMode.CBC;
                    cryptoProvider.Padding = PaddingMode.ISO10126;
                    mCst = new CryptoStream(_stream, cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read);
                    mReader = new BinaryReader(mCst);
                }
                else
                {
                    mReader = new BinaryReader(_stream);
                }
            }
            void Init()
            {

            }

            public override void End()
            {
                if (mIsCrypt)
                    mCst.Close();
                mReader.Close();
                if (mFile != null)
                    mFile.Close();
            }
        }
        #endregion

        #region 写入
        public class Writer : ReaderAndWriterBase
        {

            private BinaryWriter mWriter;
            public Writer(string _filename, bool _IsCrypt) : base(_filename, _IsCrypt)
            {
                Init();
            }

            public BinaryWriter WriterSteam
            {
                get
                {
                    return mWriter;
                }
            }

            public void Init()
            {
                if (File.Exists(mFileName))
                    File.Delete(mFileName);
                mFile = File.Open(mFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                if (mIsCrypt)
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    cryptoProvider.Key = mKeySc;
                    cryptoProvider.IV = cryptoProvider.Key;
                    cryptoProvider.Mode = CipherMode.CBC;
                    cryptoProvider.Padding = PaddingMode.ISO10126;
                    mCst = new CryptoStream(mFile, cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write);
                    mWriter = new BinaryWriter(mCst);
                }
                else
                {
                    mWriter = new BinaryWriter(mFile);
                }
            }

            public override void End()
            {
                mWriter.Write(new byte[100], 0, 100);
                mWriter.Flush();
                if (mIsCrypt)
                    mCst.FlushFinalBlock();
                mFile.Close();
                mWriter.Close();

            }
        }
        #endregion
    }
}


