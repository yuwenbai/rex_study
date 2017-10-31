
using System.Collections.Generic;
using UnityEditor;
using System.Security;
public class ExportConfig {
    private const string cFileName = "AppConfig.xml";

    public  string sAppName = "/TestApp/"; // App名字
    public  string sDefaultFolder = "Assets/BundlesResources/Data/"; // 默认导出路径,统一不可更改
    public  string sResourcesPath = "Assets/NeedExportResources/"; //需要导出的资源文件夹
    public  string sEditorBundleFolder = "Assets/../Data/"; //编辑器工程外部资源路径
    public  string sStreamingBundleFolder = "Assets/StreamingAssets/Data/";//内部资源路径

    public  BuildAssetBundleOptions sBuildOption = BuildAssetBundleOptions.ChunkBasedCompression; //编译选择


    public  string GetTartFolder(BuildTarget _target)
    {
        return string.Format("/{0}/", _target.ToString()); 
    }

    public ExportConfig()
    {
    }

    public void LoadConfig()
    {
        string tfullpath = System.IO.Directory.GetCurrentDirectory() + "\\Assets\\Editor\\"+ cFileName;
        tfullpath = tfullpath.Replace("\\", "/");
        LitEngine.XmlLoad.SecurityParser txmlfile = new LitEngine.XmlLoad.SecurityParser();
        txmlfile.LoadXml(System.IO.File.ReadAllText(tfullpath));
        SecurityElement txmlroot = txmlfile.ToXml();
        SecurityElement tapp = txmlroot.SearchForChildByTag("App");
        sAppName = tapp.Attribute("AppName");
        sBuildOption = (BuildAssetBundleOptions)int.Parse( tapp.Attribute("BuildAssetBundleOptions"));
        sDefaultFolder = tapp.SearchForChildByTag("DefaultExportPath").Text;
        sResourcesPath = tapp.SearchForChildByTag("NeedExportPath").Text;
        sEditorBundleFolder = tapp.SearchForChildByTag("EditorBundlePath").Text;
        sStreamingBundleFolder = tapp.SearchForChildByTag("StreamingBundlePath").Text;

    }
}
