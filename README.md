# UnityMMO
做游戏几年了,很多东西不好在工作项目上尝试(比如ECS),所以就有了本项目,我打算利用业余时间从头制作一个3D-MMO游戏,很多功能虽然都接触过,但我想换个做法(不然就不好玩了),反正没人逼着上线.前端上使用xlua,玩法系统用c#(主要是想用unity2018推的ECS),界面就用lua开发就行了.后端用Skynet(对比了其它几个开源项目后还是觉得skynet最简洁)  

# 使用方法
要求:Unity2018以上并从菜单Window/Package Manager里下载Entities  
前端:下载下来后整个目录就是Unity的项目目录,用Unity打开,运行main.unity场景即可进入游戏的登录界面  
后端:  
)安装虚拟机,我使用的是CentOS7,然后设置整个项目目录为虚拟机的共享目录,cd到Server目录,先编译skynet:[skynet主页](https://github.com/cloudwu/skynet "skynet主页")  
)在虚拟机安装mysql并导入Server/data/里的两个数据库文件  
)运行:./run.sh跑起服务端  

# 已完成
)前端用Luaframework的网络接口,后端用skynet的loginserver通过登录验证  
)使用sproto协议,按模块分文件和id组,开发时拼接所有协议文件,发布版则预导出为二进制  
)搭建apache服务器提供资源和lua代码的热更新(从ulua改成xlua后暂不支持)  
)增加后端通用的数据库服务(有mysql增删改接口就行)  
)在PC和安卓平台测试通过了,可以连虚拟机上的服务端并登录(从ulua改成xlua后暂不支持安卓)  
)给xlua集成了lpeg,sproto,lua-crypt第三方库  
)创建玩家帐号数据库和相关操作服务  
)登录流程相关界面  

# Todo
前端:   
)人物场景漫游-ECS做法(50%)  
)基于组件的UI框架(70%)  
)人物动作方面等Unity的新版Animation系统(IAnimationJob)完善后再介入吧  
)战斗系统  
)报错界面(10%)  
)资源管理:增加本地加载模式(直接读取本地lua和资源,Luaframework每次改界面都要打包资源这太难受了,目前只有一界面暂时不做)(30%)  
)使用Unity的AssetBundleBrowser打包资源
)管理SDK接入  

后端:  
)人物的移动同步(10%)  
)使用Redis  
)AOI  
)NPC与怪物AI  
