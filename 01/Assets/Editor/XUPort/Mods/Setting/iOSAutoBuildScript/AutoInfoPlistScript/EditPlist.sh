# !/bin/bash

# =============================== 修改项目的info.plist文件 ============================= #


# ************************* 第一种方案：直接添加

#xcode 工程目录
main_path=$1

xcode_path=${main_path}"/mahjongios_xcode"
echo "xcode_path:"${xcode_path}
echo
#plist文件名
info_plist_name=${xcode_path}"/Info.plist"

echo ${info_plist_name}
echo
#微信secret值
wetchat_secret="wx8820c0bb95c6fc96"

# 先添加CFBundleURLTypes key 值
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes array' ${info_plist_name}

# 添加 Wechat scheme value值
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes: dict' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:0:CFBundleTypeRole string Viewer' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:0:CFBundleURLIconFile string LaunchScreen-iPad' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:0:CFBundleURLName string weixin' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:0:CFBundleURLSchemes array' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:0:CFBundleURLSchemes: string wx8820c0bb95c6fc96' ${info_plist_name}

# 添加 app scheme value值
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes: dict' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:1:CFBundleTypeRole string Viewer' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:1:CFBundleURLIconFile string LaunchScreen-iPad' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:1:CFBundleURLName string com.slj.gamemj' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:1:CFBundleURLSchemes array' ${info_plist_name}
/usr/libexec/PlistBuddy -c 'Add :CFBundleURLTypes:1:CFBundleURLSchemes: string app' ${info_plist_name}

echo "\033[32;1m编辑 Info.plist成功 🚀 🚀 🚀  \033[0m"

# ************************* 第二种方案：合并plist文件
#/usr/libexec/PlistBuddy -c 'Merge ExtentionInfo.plist'  Info.plist

