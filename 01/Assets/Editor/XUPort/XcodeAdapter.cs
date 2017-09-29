using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.XCodeEditor;
using System.IO;

public class XcodeAdapter {

	public XCProject _project;

	public XcodeAdapter (string xcodePath) {
		this.project = new XCProject( xcodePath );
		// Find and run through all projmods files to patch the project.
		// Please pay attention that ALL projmods files in your project folder will be excuted!
		string[] files = Directory.GetFiles( Application.dataPath, "*.projmods", SearchOption.AllDirectories );
		foreach( string file in files ) {
			UnityEngine.Debug.Log("ProjMod File: "+file);
			this.project.ApplyMod( file );
		}
	}

	public XCProject project {
		get {
			return _project;
		}
		set
		{
			if (_project!= value)
			{
				_project = value;
			}
		}
	}

	//设置证书管理方式 Automatic,Manual
	public void SettingProvisionStyle (bool isAutomatic) {
		
		this.project.SettingProvisionStyle (isAutomatic);
	}

	//设置TEAM
	public void SettingTeamID(string teamID) {
		this.project.overwriteStringSetting ("DEVELOPMENT_TEAM", teamID);
	}
		
	//设置签名的证书
	public void SettingCodeSign (string codeSign,string build = "all") {

		if (build == @"all") {
			this.project.overwriteStringSetting ("CODE_SIGN_IDENTITY", codeSign);
		} else {
			this.project.overwriteStringSetting ("CODE_SIGN_IDENTITY", codeSign, build);
		}
	}

	//设置Provisioning配置文件
	public void SettingProvisioning(string provisioning,string build = "all") {
		if (build == @"all") {
			this.project.overwriteStringSetting ("PROVISIONING_PROFILE_SPECIFIER", provisioning);
		} else {
			this.project.overwriteStringSetting ("PROVISIONING_PROFILE_SPECIFIER", provisioning,build);
		}
	}


	//设置Provisioning配置文件(Xcode中已废弃的配置，但是在Unity中不配置会报错，仅仅为了Unity不报错)
	public void SettingProvisioningDeprecated(string provisioning,string build = "all") {
		if (build == @"all") {
			this.project.overwriteStringSetting ("PROVISIONING_PROFILE_SPECIFIER", provisioning);
		} else {
			this.project.overwriteStringSetting ("PROVISIONING_PROFILE_SPECIFIER", provisioning,build);
		}
	}



	//设置BitCode
	public void SettingBitCode(bool isBitCode = false) {
		if (isBitCode) {
			this.project.overwriteBuildSetting ("ENABLE_BITCODE", "YES");
		} else {
			this.project.overwriteBuildSetting ("ENABLE_BITCODE", "NO");
		}
	}

	//设置Launch Screen
	public void SettingLaunchScreen(string launchScreen) {
		project.overwriteBuildSetting ("UILaunchStoryboardName~ipad", launchScreen);
		project.overwriteBuildSetting ("UILaunchStoryboardName~iphone", launchScreen);
		project.overwriteBuildSetting ("UILaunchStoryboardName~ipod", launchScreen);
	}

	//添加LinkFlag
	public void AddLinkFlag (string linkFlag) {
		this.project.AddOtherLinkerFlags(linkFlag);
	}


	//设置Capability服务
	public void AddCapability(Capability capability){
		project.AddCapability(capability);
		Debug.Log( "AddCapability" );
	}

	//添加Unity保存的*.entitlements到Xcode
	public void SettingCapabilityFile (){

		string[] files = Directory.GetFiles( Application.dataPath, "*.entitlements", SearchOption.AllDirectories );
		foreach( string file in files ) {
			UnityEngine.Debug.Log("Entitlements File: "+file);
			// 第二个参数设置成capabilityFile path
			this.project.overwriteCapabilitySetting ("CODE_SIGN_ENTITLEMENTS", file);
			break;
		}
	}

	//保存编辑工程
	public void SaveProject() {
		this.project.Save ();
	}
}
