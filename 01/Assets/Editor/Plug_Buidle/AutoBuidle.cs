using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System;
using System.IO;
using System.Diagnostics;
using AutoBuild;

public class AutoBuidle : Editor
{
    public enum EnumBuidleType
    {
        NeiWang,
        NeiWangTest,
        WaiWang,
        YuFaBu,
        YuFaBuDebug,
    }
    /************************************************************************/
    /* 
     * __DEBUG                  : 全功能，包括微信登录，选择服务器，自动帧率
     * __DEBUG_FIXWX            : 只有微信，写死服务器
     * __DEBUG_NOT_SELECT_SERVER: 有账号登录 但无选择服务器 与__DEBUG_FIXWX 互斥
     * __DEBUG_WEIXIN_CHECK     : 检测微信客户端 如果没有微信客户端直接退出 不打此标签微信登录按钮不显示
     * __DEBUG_SERVER_LIST      : 随机服务器
     * __DEBUG_AUTO_FRAMERATE   : 自动帧率
     * __BUNDLE_OUTER_SERVER    : 链接外网资源服务器
     * __BUNDLE_TEST_SERVER     : 连接内网测试服务器(不佳这个是内网正式服务器)
     * __BUNDLE_DOWN            : 无视配置文件(BundleConfig.xml)强制更新下载
     * __DEBUG_NOT_AUTO_LOGIN   : 不自动登录
     * __DEBUG_LOG              : 打印日志
     * __OPEN_USER_ACTION       : 日志管理器加入 
	* __DISABLE_LOG
    * _PRE_DISTRUBUTE_                                                                     */
    /************************************************************************/
	public static string DEFINE = "__DEBUG_SERVER_LIST;__DEBUG_FIXWX";
    //随机服务器  自动帧率  xml下载
    //public const string DEFINE_Android = "__DEBUG;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG";
    //正式            __DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN;
    //正式打印日志    __DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN
    //正式给压测      __DEBUG_NOT_SELECT_SERVER;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN

    //内网,关闭资源加载          __DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG
    //内网,开启资源加载          __DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN;

    //外网,开启资源加载          __DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN
    //外网仅微信登录,开启资源加载    __DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN

    public const string DEFINE_NeiWang = " __DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN;";
    public const string DEFINE_NeiWangTest = "__DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN;__BUNDLE_TEST_SERVER;";

    public const string DEFINE_WaiWang = "__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN;__DISABLE_LOG";
    public const string DEFINE_WaiWang_IOS_PingShen = "__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_DOWN;__DISABLE_LOG;__BUNDLE_IOS_SERVER;__DEBUG_NOT_AUTO_LOGIN";
    public const string DEFINE_YuFaBu = "__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_PRE_OUTER_SERVER;__BUNDLE_DOWN;__OPEN_USER_ACTION;_PRE_DISTRUBUTE_";
    public const string DEFINE_YuFaBuDebug = "__DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__BUNDLE_PRE_OUTER_SERVER;__BUNDLE_DOWN;__OPEN_USER_ACTION;_PRE_DISTRUBUTE_";

    //public const string DEFINE_Android = "__DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN;";
    //发布版
    //public const string DEFINE_iOS = "__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN;__DISABLE_LOG";
    //预发布版
    //public const string DEFINE_iOS_Pre = "__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_PRE_OUTER_SERVER;__BUNDLE_DOWN;__OPEN_USER_ACTION;_PRE_DISTRUBUTE_";

    //debug模式 不自动登录 
    //public const string DEFINE_NeiWang = "__DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN;";

    //发布版本设置
    //__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN;__DISABLE_LOG
    //预发布版本设置 
    //__DEBUG_FIXWX;__DEBUG_SERVER_LIST;__DEBUG_AUTO_FRAMERATE;__BUNDLE_PRE_OUTER_SERVER;__BUNDLE_DOWN;__DISABLE_LOG;_PRE_DISTRUBUTE_

    public const string CURRENT_VERSION = "1.2.2.1280";
    public const string CURRENT_VERSION_YUFABU = "1.2.2.1280";


    public const bool IS_DEVELOPER = false; 
    public const int ANTI_ALIASING = 2;
    public const bool ALLOW_DEBUG = false;

    // 项目名称
    private static string BuildName = "";

    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }


    public const string TARGET_DIR = "Target";

    #region PC
    public static void BuildWin32All()
    {
        BuidlePCByType(EnumBuidleType.NeiWang, true);
        BuidlePCByType(EnumBuidleType.NeiWangTest);
    }

    //[MenuItem("Tools/Build/Win32")]
    public static void BuildWin32()
    {
        BuidlePCByType(EnumBuidleType.NeiWang, true);
    }

    public static void BuildWin32_NeiWang()
    {
        BuidlePCByType(EnumBuidleType.NeiWang, true);
    }

    public static void BuildWin32_YuFaBu()
    {
        BuidlePCByType(EnumBuidleType.YuFaBuDebug, true);
    }
    #endregion

    #region MacOS
    //[MenuItem("Tools/Build/MacOS")]
    public static void BuildMacOS()
    {

        PerSetting();
        DEFINE = DEFINE_NeiWang;


        PlayerSettings.bundleVersion = GetVersion(EnumBuidleType.NeiWang);



        string path = GetDirPath() + "GamePackage/";

        ClearPath(path);
        BuildName = Application.productName + "." + Application.version + ".app";
        string savePath = path + BuildName;
        MakePath(savePath);

        BuildForPlatform(BuildTarget.StandaloneOSXUniversal, savePath);
    }
    #endregion

    #region Android
    public static void BuidleAndroidAll()
    {
        BuidleAndroidByType(EnumBuidleType.NeiWang,  true);
        BuidleAndroidByType(EnumBuidleType.WaiWang);
        BuidleAndroidByType(EnumBuidleType.YuFaBu);
    }

    [MenuItem("Tools/Build/安卓发布")]
    public static void BuildAndroidDist() {
        BuildAndroid();
    }

    [MenuItem("Tools/Build/安卓")]
    public static void BuildAndroid()
    {
        BuidleAndroidByType(EnumBuidleType.NeiWang, true);
    }
    public static void BuildAndroidNeiWang()
    {
        BuidleAndroidByType(EnumBuidleType.NeiWang, true);
    }
    #endregion

    #region IOS

	public enum BuildType
	{
		BuildReleaseIJM,   	//爱加密正式发布版
		BuildPreIJM,	    //爱加密预发布版
		BuildRelease,   	//正式发布版
		BuildPre,	    	//预发布版
		BuildIntranet,		//内网版
		BuildExtranet,		//外网版
		BuildDebug,         //debug版
        BuildIOSReview,     //评审包
    }


	[MenuItem("Tools/Build/IOS_IJM")]
	public static void BuildIOSIJM()
	{
		AutoBuild.IOSTools.copySettingProjectMod (true);
		//爱加密发布版
		Build (BuildType.BuildReleaseIJM);
	}

	[MenuItem("Tools/Build/IOS_Pre_IJM")]
	public static void BuildIOSPreIJM()
	{
		AutoBuild.IOSTools.copySettingProjectMod (true);
		//爱加密预发布版
		Build (BuildType.BuildPreIJM);
	}

    [MenuItem("Tools/Build/IOS")]
	public static void BuildIOS()
	{
		AutoBuild.IOSTools.copySettingProjectMod (false);
        //发布版
        Build (BuildType.BuildRelease);
	}

	[MenuItem("Tools/Build/IOS_Pre")]
	public static void BuildIOSPre()
	{
		AutoBuild.IOSTools.copySettingProjectMod (false);
		//预发布版
		Build (BuildType.BuildPre);
	}
		
	[MenuItem("Tools/Build/IOS_Intranet")]
	public static void BuildIOSIntra()
	{
		AutoBuild.IOSTools.copySettingProjectMod (false);
		//内网版
		Build (BuildType.BuildIntranet);
	}

	[MenuItem("Tools/Build/IOS_Extranet")]
	public static void BuildIOSExtra()
	{
		AutoBuild.IOSTools.copySettingProjectMod (false);
		//外网版
		Build (BuildType.BuildExtranet);
	}

	[MenuItem("Tools/Build/IOS_Debug")]
	public static void BuildIOSDebug()
	{
		AutoBuild.IOSTools.copySettingProjectMod (false);
		Build ();
	}

	[MenuItem("Tools/Build/IOS_ReView")]
    public static void BuildIOSReView()
    {
		AutoBuild.IOSTools.copySettingProjectMod(true);
        //审核版
		Build(BuildType.BuildIOSReview);
    }


	/// <summary>
	/// Build ios by the specified buildType.
	/// </summary>
	/// <param name="buildType">Build type.</param>
	static void Build (BuildType buildType = BuildType.BuildDebug) {
        ClearPath(GetDirPath());

        string xcodePath;
		switch (buildType) {
		case BuildType.BuildReleaseIJM: {
				SetIOSEditor (DEFINE_WaiWang);
				xcodePath = MakeXcodeProjectPath (buildType);
				break;
			}
		case BuildType.BuildPreIJM: {
				SetIOSEditor (DEFINE_YuFaBu,EnumBuidleType.YuFaBu);
				xcodePath = MakeXcodeProjectPath (buildType);
				break;
			}
		case BuildType.BuildRelease: {
				SetIOSEditor (DEFINE_WaiWang);
				xcodePath = MakeXcodeProjectPath (buildType);
				break;
			}
		case BuildType.BuildPre: {
				SetIOSEditor (DEFINE_YuFaBu, EnumBuidleType.YuFaBu);
				xcodePath = MakeXcodeProjectPath (buildType);
				break;
			}
		case BuildType.BuildIOSReview: {
				SetIOSEditor(DEFINE_WaiWang_IOS_PingShen, EnumBuidleType.WaiWang);
                xcodePath = MakeXcodeProjectPath(buildType);
				break;
			}
		case BuildType.BuildIntranet: {
				SetIOSEditor ("__DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_DOWN");
				xcodePath = MakeXcodeProjectPath (buildType);
				break;
			}
		case BuildType.BuildExtranet: {
				SetIOSEditor ("__DEBUG;__DEBUG_NOT_AUTO_LOGIN;__DEBUG_AUTO_FRAMERATE;__DEBUG_LOG;__OPEN_USER_ACTION;__BUNDLE_OUTER_SERVER;__BUNDLE_DOWN");
				xcodePath = MakeXcodeProjectPath (buildType);
				break;
			}
		default : {
				SetIOSEditor ("__DEBUG");
				xcodePath = MakeXcodeProjectPath ();
				break;
			}
		}

		BuildPlayerOptions? option = GetIOSBuildPlayerOptions (xcodePath);
		if (option != null) {
			// 生成Xcode工程
			GenericXcodeProgram ((BuildPlayerOptions)option);
		}

	}
		

    /// <summary>
    /// Sets the IOS editor.
    /// </summary>
    static void SetIOSEditor(string tag, EnumBuidleType type = EnumBuidleType.WaiWang) {

		PlayerSettings.bundleVersion = GetVersion(type);

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
		EditorUserBuildSettings.development = IS_DEVELOPER;
		EditorUserBuildSettings.allowDebugging = ALLOW_DEBUG;

		QualitySettings.antiAliasing = ANTI_ALIASING;

        VersionClient.Versionclient_Update(GetVersion(type));

        AssetDatabase.Refresh();
		SetBuildingDefine(BuildTargetGroup.iOS, tag);
		//Build option：append or replace
		//		UnityEditorInternal.CanAppendBuild = Yes;
	}

	/// <summary>
	/// 根据不同的build版本生成不同的Xcode项目路径
	/// </summary>
	/// <returns>The xcode project path.</returns>
	/// <param name="buildType">Build type.</param>
	static string MakeXcodeProjectPath(BuildType buildType = BuildType.BuildDebug){
		//Unity Assets路径
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		string targetDirect = directory.Parent.FullName +"/Target";
		UnityEngine.Debug.Log ("targetDirect:" + targetDirect);

		string targetPath = "";
		if (buildType == BuildType.BuildReleaseIJM) {
			//爱加密正式发布版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios_ijiami";
		} else if (buildType == BuildType.BuildPreIJM) {
			//爱加密预发布版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios_pre_ijiami";
		}else if (buildType == BuildType.BuildRelease) {
			//正式发布版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios";
		} else if (buildType == BuildType.BuildPre) {
			//预发布版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios_pre";
		} else if (buildType == BuildType.BuildDebug) {
			//debug版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios_debug";
		} else if (buildType == BuildType.BuildIntranet) {
			//内网版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios_Intra";
		} else if (buildType == BuildType.BuildExtranet) {
			//外网版本
			// The path where the application will be built
			targetPath = targetDirect + "/369mahjong_ios_Extra";
		} else if (buildType == BuildType.BuildIOSReview) {
			//评审版
            targetPath = targetDirect + "/369mahjong_ios_Review";
        }

        UnityEngine.Debug.Log ("targetPath:" + targetPath);
		MakePath(targetPath);

		return targetPath;
	}


	/// <summary>
	/// Gets the IOS build player options.
	/// </summary>
	/// <returns>The IOS build player options.</returns>
	static BuildPlayerOptions? GetIOSBuildPlayerOptions(string xcodePath){

		if (xcodePath == null || xcodePath.Length == 0) {
			UnityEngine.Debug.Log ("xcode path is null");
			return  null;
		}

		// The scenes to be included in the build. 
		string[] scenes = new string[] {
			"Assets/Scene/Main.unity",
			"Assets/Scene/Empty.unity",
			"Assets/Scene/MahJong.unity"
		};
			
		//Build target 
		BuildTarget target = BuildTarget.iOS;
		//Build option：append
		BuildOptions option = BuildOptions.AcceptExternalModificationsToPlayer;

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = scenes;
		buildPlayerOptions.locationPathName = xcodePath;
		buildPlayerOptions.target = target;
		buildPlayerOptions.options = option;
		return buildPlayerOptions;
	}

	//<summary>
	//生成Xcode工程
	//</summary>
	static void GenericXcodeProgram(BuildPlayerOptions buildPlayerOptions) {

		// begin build
		string res = BuildPipeline.BuildPlayer(buildPlayerOptions);

		//		string res = BuildPipeline.BuildPlayer(scenes,targetPath,target,option);

		if (res.Length > 0) {
			throw new Exception("Xcode 工程生成失败: " + res);
		}
	}

    #endregion




    #region 辅助方法
    static void PerSetting(EnumBuidleType type = EnumBuidleType.WaiWang)
    {
        PlayerSettings.bundleVersion = GetVersion(type);
        EditorUserBuildSettings.development = IS_DEVELOPER;

        EditorUserBuildSettings.allowDebugging = ALLOW_DEBUG;

        QualitySettings.antiAliasing = ANTI_ALIASING;

        QualitySettings.vSyncCount = 0;

        VersionClient.Versionclient_Update(GetVersion(type));

        AssetDatabase.Refresh();
    }

    static void SetBuildingDefine(BuildTargetGroup gp, string symble)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(gp, symble);
    }

    static void BuildForPlatform(BuildTarget tag, string savePath)
    {

        if (DEFINE != null && DEFINE.Length > 0)
        {
            switch (tag)
            {
                default:
                    break;
                case BuildTarget.Android:
                    SetBuildingDefine(BuildTargetGroup.Android, DEFINE);
                    break;
                case BuildTarget.iOS:
                    SetBuildingDefine(BuildTargetGroup.iOS, DEFINE);
                    break;
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    SetBuildingDefine(BuildTargetGroup.Standalone, DEFINE);
                    break;

            }
        }

        string buildFinish = "";
        buildFinish = BuildPipeline.BuildPlayer(GetBuildScenes(), savePath, tag, BuildOptions.None);
        if (buildFinish == "")
        {
            BuildUpdateEditor.OneKeyBuildView.GetInstance.BuildFinish(savePath, BuildName);
        }
    }

    private static void BuidlePCByType(EnumBuidleType buidleType, bool isClearPath = false)
    {
        PlayerSettings.productName = "369Manjong" + buidleType.ToString();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(buidleType.ToString());
        sb.Append("/");
        sb.Append(Application.productName);
        sb.Append(".");
        sb.Append(GetVersion(buidleType));
        sb.Append(".");
        sb.Append(buidleType.ToString());
        sb.Append(".exe");

        BuidleByType(buidleType, BuildTarget.StandaloneWindows, sb.ToString(), isClearPath);
    }


    private static void BuidleAndroidByType(EnumBuidleType buidleType, bool isClearPath = false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(Application.productName);
        sb.Append(".");
        sb.Append(GetVersion(buidleType));
        sb.Append("_");
        sb.Append(buidleType.ToString());
        sb.Append("_");
        sb.Append(DateTime.Now.ToString("HHMMss"));
        sb.Append(".apk");

        BuidleByType(buidleType, BuildTarget.Android,sb.ToString(), isClearPath);
    }


    private static void BuidleByType(EnumBuidleType buidleType, BuildTarget type,string BuildName, bool isClearPath = false)
    {
        PerSetting(buidleType);

        string savePath = GetDirPath();

        switch (buidleType)
        {
            case EnumBuidleType.NeiWang:
                DEFINE = DEFINE_NeiWang;
                break;
            case EnumBuidleType.NeiWangTest:
                DEFINE = DEFINE_NeiWangTest;
                break;
            case EnumBuidleType.WaiWang:
                DEFINE = DEFINE_WaiWang;
                break;
            case EnumBuidleType.YuFaBu:
                DEFINE = DEFINE_YuFaBu;
                break;
            case EnumBuidleType.YuFaBuDebug:
                DEFINE = DEFINE_YuFaBuDebug;
                break;
        }

        savePath = savePath + BuildName;
        if (isClearPath)
        {
            ClearPath(savePath);
        }

        //删除已经存在的游戏包
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }
        MakePath(savePath);
        BuildForPlatform(type, savePath);
    }


    private static string GetVersion(EnumBuidleType type)
    {
        string version = CURRENT_VERSION;
        if (type == EnumBuidleType.YuFaBu || type == EnumBuidleType.YuFaBuDebug)
        {
            version = CURRENT_VERSION_YUFABU;
        }
        return version;
    }
    #endregion

    #region 路径方法
    static string GetDirPath()
    {
        return Application.dataPath
              + System.IO.Path.DirectorySeparatorChar + ".."
              + System.IO.Path.DirectorySeparatorChar
              + TARGET_DIR
              + System.IO.Path.DirectorySeparatorChar;
    }

    /// <summary>
    /// 清除目录
    /// </summary>
    /// <param name="path"></param>
    static void ClearPath(string path)
    {
        string apath = path;
        string sp = System.IO.Path.DirectorySeparatorChar.ToString();
        if (!path.EndsWith(sp))
        {
            apath = path.Substring(0, path.LastIndexOf(sp));
        }

        if (System.IO.Directory.Exists(apath))
        {
            System.IO.Directory.Delete(apath, true);
        }
    }

    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="path"></param>
    static void MakePath(string path)
    {
        string apath = path;
        string sp = System.IO.Path.DirectorySeparatorChar.ToString();

        if (!path.EndsWith(sp))
        {
            apath = path.Substring(0, path.LastIndexOf(sp));

        }

        if (!System.IO.Directory.Exists(apath))
        {
            System.IO.Directory.CreateDirectory(apath);
        }

    }
    #endregion
}


[InitializeOnLoad]
public class AutoSetAndroid
{
    // Custom compiler defines:
    //
    // CROSS_PLATFORM_INPUT : denotes that cross platform input package exists, so that other packages can use their CrossPlatformInput functions.
    // EDITOR_MOBILE_INPUT : denotes that mobile input should be used in editor, if a mobile build target is selected. (i.e. using Unity Remote app).
    // MOBILE_INPUT : denotes that mobile input should be used right now!

    static AutoSetAndroid()
    {
        PlayerSettings.Android.keyaliasName = "com.slj.gamemj";
        PlayerSettings.Android.keyaliasPass = "sljgamemj";
        PlayerSettings.Android.keystoreName = Application.dataPath + "/../(sljgamemj)com.slj.gamemj.keystore";
        PlayerSettings.Android.keystorePass = "sljgamemj";
        PlayerSettings.bundleIdentifier = "com.slj.gamemj";
        PlayerSettings.bundleVersion = AutoBuidle.CURRENT_VERSION;

        VersionClient.Versionclient_Update(AutoBuidle.CURRENT_VERSION);
    }
}
