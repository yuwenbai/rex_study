#UNITY工程目录
UNITY_PATH=$1

#unity目录
#例子 /Applications/Unity/Unity.app/Contents/MacOS/Unity
UNITY_EXCUTE_PATH=$2

#日志路径-#PREBUILD_PATH=${PROJECT_PATH}/PreBuild.log
BUILD_PATH=${UNITY_PATH}/TeamCiytUnity_Android.log


echo $UNITY_EXCUTE_PATH -batchmode -projectPath "$UNITY_PATH" -executeMethod AutoBuidle.BuidleAndroidAll -logFile "$BUILD_PATH" -quit

$UNITY_EXCUTE_PATH -batchmode -projectPath "$UNITY_PATH" -executeMethod AutoBuidle.BuidleAndroidAll -logFile "$BUILD_PATH" -quit


echo "\033[32;1m打包完毕 🚀 🚀 🚀  \033[0m"

exit 0
