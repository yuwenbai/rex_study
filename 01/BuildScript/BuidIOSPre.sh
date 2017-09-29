#UNITY工程目录
UNITY_PATH=$1

#unity目录
#例子 /Applications/Unity/Unity.app/Contents/MacOS/Unity
UNITY_EXCUTE_PATH=$2

#日志路径-#PREBUILD_PATH=${PROJECT_PATH}/PreBuild.log
BUILD_PATH=${UNITY_PATH}/TeamCiytUnity_IOS.log

#打包xcode工程
$UNITY_EXCUTE_PATH -batchmode -projectPath "$UNITY_PATH" -executeMethod AutoBuidle.BuildIOSPre -logFile "$BUILD_PATH" -quit

#Xcode工程路径
XCODE_PROJECT_PATH=${UNITY_PATH}/Target/369mahjong_ios_pre

#IPA包路径
IPA_PATH=${UNITY_PATH}/Target/IPA

#调用IPA打包脚本
sh ${XCODE_PROJECT_PATH}/AutoPackageScript/AutoPackageScript.sh  ${XCODE_PROJECT_PATH}  ${IPA_PATH}
echo "\033[32;1miOS一键打包完毕 🚀 🚀 🚀  \033[0m"

exit 0
