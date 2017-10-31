# LitEngine
自己用的一个简单集合. 集合了ILRuntime,sharpzip等库,实现了ilruntime下的proto解析,还有自己用的资源加载管理. 留个爪印把.
内容缓慢补充中,之后的大概会缓慢修改资源加载管理模块.资源拆分加载部分.以适配新版本的unity.看时间把.


 **命名空间 LitEngine** 

 **ScriptInterface** 命名空间下定义了与unity对象的接口

 **Loader** 命名空间下为资源加载模块

 **ProtoCSLS** 命名空间下为protobuf解析模块

 **NetTool** 命名空间下为网络模块,包含TCP,UDP,WWW的一些封装 UDP没有进行网络适配

 **ReaderAndWriterTool** 命名空间下为文本加密模块

 **XmlLoad** XML加载模块

其他命名空间功能单一,有空再补

 **管理器类** 

**AppCore**  App核心

**GameCore** App下的游戏核心

**ScriptManager** 脚本管理

**LoaderManager** 资源管理

**GameUpdateManager** 更新逻辑处理

 **使用方法:** 
更改中,稍后上传测试工程