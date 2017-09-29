# !/bin/bash

# =============================== 导出Xcode工程脚本 ============================= #
#运行脚本时传入参数：Unity项目路径

#工程主目录
#PROJECT_PATH=${path}

#Unity工程名字
#UNITY_NAME=Mahjong170329

#Xcode工程名字
#XCODE_NAME=mahjongios_xcode

#UNITY程序的启动路径#
UNITY_EXCUTE_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity

#UNITY工程目录
UNITY_PATH=$1

#XCODE程序路径#
#XCODE_PATH=${PROJECT_PATH}/${XCODE_NAME}

#日志路径-#PREBUILD_PATH=${PROJECT_PATH}/PreBuild.log
BUILD_PATH='~/TeamCiytUnity.log'

#IOS打包脚本路径#
#BUILD_IOS_PATH=${PROJECT_PATH}/Assets/buildios.sh

#生成的Xcode工程路径#
#XCODE_PATH=${PROJECT_PATH}/$1


#将unity导出成xcode工程#
$UNITY_EXCUTE_PATH -batchmode -projectPath ${UNITY_PATH} -executeMethod AutoBuidle.BuildIOS  -logFile $BUILD_PATH -quit
#$UNITY_EXCUTE_PATH  quit -batchmode -projectPath ${UNITY_PATH} -executeMethod AutoBuidle.BuildIOS

echo "\033[32;1mXcode包生成完毕 🚀 🚀 🚀  \033[0m"

#echo "\033[32;1m脚本执行完毕 🚀 🚀 🚀  \033[0m"
