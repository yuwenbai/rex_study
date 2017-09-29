# !/bin/bash


# 使用方法:
# step1 : 将AutoPackageScript整个文件夹拖入到项目主目录,项目主目录,项目主目录~~~(重要的事情说3遍!😊😊😊)
# step2 : 打开AutoPackageScript.sh文件,修改 "项目自定义部分" 配置好项目参数
# step3 : 打开终端, cd到AutoPackageScript文件夹 (ps:在终端中先输入cd ,直接拖入AutoPackageScript文件夹,回车)
# step4 : 输入 sh AutoPackageScript.sh 命令,回车,开始执行此打包脚本

# ===============================脚本文件名配置 ============================= #
#项目主路径
#main_path=$1
#unity项目文件名字
#unity_folder_name="Mahjong170329"
#xcode项目路径
xcode_path=$1
#ipa保存路径
ipa_path=$2

#IPA文件夹名字
#ipa_folder_name="IPA"
# 工程中Target对应的配置plist文件名称, Xcode默认的配置文件为Info.plist
info_plist_name="Info.plist"
# 指定项目的scheme名称
#scheme_name="369mahjong"
scheme_name="Unity-iPhone"
#ipa包最终名字前缀
pre_ipa_name="369mj_alpha_1.0.0."
# 指定输出归档文件地址
archive_name="$scheme_name.xcarchive"
# ===============================脚本文件路径配置 ============================= #
#unity项目路径
#unity_path="${main_path}/${unity_folder_name}"
#xcode项目路径
#xcode_path="${main_path}/${xcode_folder_name}"
#IPA包存放路径
#export_ipa_path="${xcode_path}/${ipa_folder_name}"
export_ipa_path="${ipa_path}"
#打包脚本路径
script_path="$xcode_path/AutoPackageScript/AutoPackageScript.sh"
#导出ipa所需要的plist文件路径，默认是development
ExportOptionsPlistPath="$xcode_path/AutoPackageScript/DevelopmentExportOptionsPlist.plist"
# info plist 文件路径
info_plist_path="${xcode_path}/Info.plist"

# =============================== 打印项目打包配置 ============================= #
echo "\033[32m参数配置: \033[0m"
echo "\033[0mxcode_path:${xcode_path}\033[0m"
echo
echo "\033[0mscheme_name:${scheme_name}\033[0m"
echo
echo "\033[0marchive_name:${archive_name}\033[0m"
echo
echo "\033[0mexport_ipa_path:${export_ipa_path}\033[0m"
echo
echo "\033[0mscript_path:${script_path}\033[0m"
echo
echo "\033[0mExportOptionsPlistPath:${ExportOptionsPlistPath}\033[0m"
echo
echo "\033[0minfo_plist_path:${info_plist_path}\033[0m"
echo

# ===============================项目自定义部分============================= #
# 计时
SECONDS=0
# 是否编译工作空间 (例:若是用Cocopods管理的.xcworkspace项目,赋值true;用Xcode默认创建的.xcodeproj,赋值false)
is_workspace="false"
# 指定要打包编译的方式 : Release,Debug...
build_configuration="Release"

# ===============================自动打包部分(无特殊情况不用修改)============================= #
echo "\033[32m开始<${scheme_name}>项目自动打包\033[0m"
# 进到Xcode项目文件下
cd $xcode_path
# 获取项目名称
project_name=`find . -name *.xcodeproj | awk -F "[/.]" '{print $(NF-1)}'`
# 获取版本号,内部版本号,bundleID
#info_plist_path="$info_plist_name.plist"
bundle_version=`/usr/libexec/PlistBuddy -c "Print CFBundleShortVersionString" $info_plist_path`
bundle_build_version=`/usr/libexec/PlistBuddy -c "Print CFBundleVersion" $info_plist_path`
bundle_identifier=`/usr/libexec/PlistBuddy -c "Print CFBundleIdentifier" $info_plist_path`

# 指定输出ipa名称 : scheme_name + bundle_version
export_ipa_name="$pre_ipa_name$bundle_version.ipa"

# 删除旧.xcarchive文件
#rm -rf export_archive_path

# =============================== 打印项目打包配置 ============================= #
echo "\033[0m项目名称:${project_name}\033[0m"
echo
echo "\033[0mPlist路径:${info_plist_path}\033[0m"
echo
echo "\033[0m版本号:${bundle_version}\033[0m"
echo
echo "\033[0m内部版本号:${bundle_build_version}\033[0m"
echo
echo "\033[0mbundle Id:${bundle_identifier}\033[0m"
echo
echo "\033[0mipa包路径:${export_ipa_path}\033[0m"

# AdHoc,AppStore,Enterprise三种打包方式的区别: http://blog.csdn.net/lwjok2007/article/details/46379945
#echo "\033[36;1m请选择打包方式(输入序号,按回车即可) \033[0m"
#echo "\033[33;1m1. AdHoc       \033[0m"
#echo "\033[33;1m2. AppStore    \033[0m"
#echo "\033[33;1m3. Enterprise  \033[0m"
#echo "\033[33;1m4. Development \033[0m"
## 读取用户输入并存到变量里
#read parameter
#sleep 0.5
#method="$parameter"

method="4"
# 判读用户是否有输入
if [ -n "$method" ]
then
    if [ "$method" = "1" ] ; then
    ExportOptionsPlistPath="$xcode_path/AutoPackageScript/AdHocExportOptionsPlist.plist"
    echo "\033[32m**打AdHoc包   \033[0m"
    elif [ "$method" = "2" ] ; then
    ExportOptionsPlistPath="$xcode_path/AutoPackageScript/AppStoreExportOptionsPlist.plist"
    echo "\033[32m**打AppStore包   \033[0m"
    elif [ "$method" = "3" ] ; then
    ExportOptionsPlistPath="$xcode_path/AutoPackageScript/EnterpriseExportOptionsPlist.plist"
    echo "\033[32m**打Enterprise包   \033[0m"
    elif [ "$method" = "4" ] ; then
    ExportOptionsPlistPath="$xcode_path/AutoPackageScript/DevelopmentExportOptionsPlist.plist"
    echo "\033[32m**打Development包   \033[0m"
    else
    echo "输入的参数无效!!!"
    exit 1
    fi
fi

echo "\033[32m*************************  创建文件夹  *************************  \033[0m"

# 指定输出文件目录不存在则创建
if [ -d "$export_ipa_path" ] ; then
echo $export_ipa_path
echo "\033[32m**打包路径:${export_ipa_path}\033[0m"
else
mkdir -pv "$export_ipa_path"
echo "\033[32m**创建打包路径:${export_ipa_path}\033[0m"
fi


# xcarchive包路径
export_archive_path="$export_ipa_path/$archive_name"

# 获取上一次打包时间，如果之前没有打包，则取当前时间
if [ -f "${export_archive_path}/Info.plist" ] ; then
#上一次打包时间
last_date=`/usr/libexec/PlistBuddy -c "Print CreationDate" ${export_archive_path}/Info.plist`
#删除上次打包xcarchive包
rm -r ${export_archive_path}
echo "读取上一次打包时间：${last_date}"
else
#获取当前日期：年-月-日
ls_date=`date +%F`
#获取当前时间：hh:mm:ss
#ls_time=`date +%T`
ls_time=`date +%H-%M-%S`
last_date="$ls_date $ls_time"
echo $date_time
echo "读取当前时间"
fi

#暂时用测试包名命名ipa
export_ipa_name="369mj_alpha_1.0.0.ipa"
last_ipa_path=~/Documents/IPA/"$last_date"

#将旧包保存到制定路径中
if [ -f "$export_ipa_path/$export_ipa_name" ] ; then
    if [ -d "${last_ipa_path}" ]; then
    mv "${export_ipa_path}/${export_ipa_name}"  "${last_ipa_path}"
    else
    mkdir "$last_ipa_path"
    mv "${export_ipa_path}/${export_ipa_name}"  "${last_ipa_path}"
    echo $last_ipa_path
    fi
fi


echo "\033[32m*************************  编译工程  *************************  \033[0m"
# 判断编译的项目类型是workspace还是project
if $is_workspace ; then
echo "\033[32m**编译workspace工程\033[0m"
# 编译前清理工程
xcodebuild clean -workspace ${project_name}.xcworkspace \
                 -scheme ${scheme_name} \
                 -configuration ${build_configuration}

xcodebuild archive -workspace ${project_name}.xcworkspace \
                   -scheme ${scheme_name} \
                   -configuration ${build_configuration} \
                   -archivePath "${export_archive_path}"
else
echo "\033[32m**编译project工程\033[0m"
# 编译前清理工程
xcodebuild clean -project "${xcode_path}/${project_name}.xcodeproj" \
                 -scheme ${scheme_name} \
                 -configuration ${build_configuration}

xcodebuild archive -project "${xcode_path}/${project_name}.xcodeproj"  \
                   -scheme ${scheme_name} \
                   -configuration ${build_configuration} \
                   -archivePath "${export_archive_path}"
fi

#  检查是否构建成功
#  xcarchive 实际是一个文件夹不是一个文件所以使用 -d 判断
if [ -d "$export_archive_path" ] ; then
echo "\033[32;1m项目构建成功 🚀 🚀 🚀  \033[0m"
else
echo "\033[31;1m项目构建失败 😢 😢 😢  \033[0m"
exit 1
fi

echo "\033[32m*************************  开始导出ipa文件  *************************  \033[0m"

xcodebuild  -exportArchive \
            -archivePath "${export_archive_path}" \
            -exportPath "${export_ipa_path}" \
            -exportOptionsPlist ${ExportOptionsPlistPath}

# 修改ipa文件名称
mv "$export_ipa_path/$scheme_name.ipa" "$export_ipa_path/$export_ipa_name"

# 检查文件是否存在
if [ -f "$export_ipa_path/$export_ipa_name" ] ; then
echo "\033[32;1m导出 ${export_ipa_name} 包成功 🎉  🎉  🎉   \033[0m"
open "$export_ipa_path"
else
echo "\033[31;1m导出 ${export_ipa_name} 包失败 😢 😢 😢     \033[0m"
# 相关的解决方法
echo "\033[34mps:以下类型的错误可以参考对应的链接\033[0m"
echo "\033[34m  1.\"error: exportArchive: No applicable devices found.\" --> 可能是ruby版本过低导致,升级最新版ruby再试,升级方法自行百度/谷歌,GitHub issue: https://github.com/jkpang/AutoPackageScript/issues/1#issuecomment-297589697"
echo "\033[34m  2.\"No valid iOS Distribution signing identities belonging to team 6F4Q87T7VD were found.\" --> http://fight4j.github.io/2016/11/21/xcodebuild/ \033[0m"
exit 1
fi
# 输出打包总用时
echo "\033[36;1m使用AutoPackageScript打包总用时: ${SECONDS}s \033[0m"

#关闭Unity
#killall Unity
#echo "finished"
exit










