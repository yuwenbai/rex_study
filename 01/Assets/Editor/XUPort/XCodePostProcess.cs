using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;


public static class XCodePostProcess
{

#if UNITY_EDITOR
	[PostProcessBuild(999)]
	public static void OnPostProcessBuild( BuildTarget target, string pathToBuiltProject )
	{
		if (target != BuildTarget.iOS) {
			Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		XcodeAdapter adapter = new XcodeAdapter (pathToBuiltProject);

		//设置证书管理方式（fasle：手动管理，true：自动管理）
		adapter.SettingProvisionStyle (false);

		//设置TEAM
		adapter.SettingTeamID ("5BB83946D7");

		//设置签名的证书
//		adapter.SettingCodeSign("iPhone Distribution: jian kang (5BB83946D7)");
//		adapter.SettingCodeSign("iPhone Developer: jian kang (H3FV29MKRE)","Debug");
		adapter.SettingCodeSign("iPhone Developer: jian kang (H3FV29MKRE)");

//		adapter.SettingProvisioningDeprecated ("200dfc50-ac55-49ed-acd2-4848a3b12f21");
//		adapter.SettingProvisioningDeprecated ("1aabb7a1-7d33-486d-a666-f4e17e76c4b1","Debug");

		//设置Provisioning配置文件
//		adapter.SettingProvisioning ("sljMahJong_Dis");
//		adapter.SettingProvisioning ("sljMahJong_dev","Debug");
		adapter.SettingProvisioning ("sljMahJong_dev");

		//设置BitCode
		adapter.SettingBitCode (false);

		//编辑capabilities
		//adapter.AddCapability(Capability.AssociatedDomains);
		//adapter.SettingCapabilityFile ();

		//设置Launch Screen
		//adapter.SettingLaunchScreen ("LaunchScreen");

		//得到xcode工程的路径
		string path = Path.GetFullPath (pathToBuiltProject);
		//复制代码文件
		AutoBuild.IOSTools.CopyOCFileToXcode (path);
		//复制脚本文件
		AutoBuild.IOSTools.CopyScriptToXcode (path);

		// 保存编辑工程
		adapter.SaveProject();

//		BuildIPA (path);

//		// 复制魔窗脚本
//		AutoBuild.IOSTools.removeMagicWindow();
		AutoBuild.IOSTools.removeSettingProjectMod();

	}


	static void BuildIPA (string xcodePath) {

		UnityEngine.Debug.Log ("开始打包");
		string ipaScritpPath = xcodePath+"/AutoPackageScript/AutoPackageScript.sh";

		DirectoryInfo xcodeDirect = new DirectoryInfo (xcodePath);
		string ipaPath = xcodeDirect.Parent.FullName +"/IPA";

		// 导出IPA包，不带进度条
		AutoBuild.IOSTools.XcodebuildIPA (ipaScritpPath,xcodePath,ipaPath);

		// 导出IPA包，带进度条
		//		AutoBuild.IOSTools.XcodebuildIPAWithProcessBar (ipaScritpPath,xcodePath, ipaPath);
		UnityEngine.Debug.Log ("打包结束");
	}
		




#endif

	public static void Log(string message)
	{
		UnityEngine.Debug.Log("PostProcess: "+message);
	}
		
}
