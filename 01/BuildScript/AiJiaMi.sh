#!bin/sh
#项目工程
UNITY_PATH=$1
for file in ${UNITY_PATH}/Target/*
   do
        if test -f $file
        then
        echo $file
            java -jar ${UNITY_PATH}/BuildScript/encryptiontool-1.2.jar "http://192.168.221.20:10086/api" "robot" "0" $file ${file%.*}.JM.apk "" "gamemj"
        fi
   done
exit 0