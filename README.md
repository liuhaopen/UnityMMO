# UnityMMO
做游戏几年了,很多东西不好在工作项目上尝试(比如ECS),所以就有了本项目,我打算利用业余时间从头制作一个3D-MMO游戏,很多功能虽然都接触过,但我想换个做法(不然就不好玩了),反正没人逼着上线.前端上使用xlua,玩法系统用c#(主要是想用unity2018推的ECS,虽然目前还未完善各大系统都未对接不能用pure ecs),界面就用lua开发就行了.后端用Skynet(对比了其它几个开源项目后还是觉得skynet最简洁)  

# 使用方法
克隆本项目:git clone https://github.com/liuhaopen/UnityMMO.git --recurse  
要求:Unity2018以上并从菜单Window/Package Manager里下载Entities  
前端:  
下载下来后整个目录就是Unity的项目目录,用Unity打开,运行main.unity场景即可进入游戏的登录界面  
注:由于游戏资源过大且经常变更(每个版本的资源都会保存在.git文件夹里,clone就要好久了),所以放到另外的项目管理,可在[UnityMMO-Resource](https://github.com/liuhaopen/UnityMMO-Resource/tree/master/Assets/AssetBundleRes "UnityMMO-Resource")下载里面的文件并复制到本项目的Assets/AssetBundleRes里(注:有些插件因为版权问题就没上传了,从其中的download-page见购买链接)  
后端:  
)安装虚拟机,我使用的是CentOS7,然后设置整个项目目录为虚拟机的共享目录,cd到Server目录,先编译skynet:[skynet主页](https://github.com/cloudwu/skynet "skynet主页")  
)在虚拟机安装mysql并导入Server/data/里的两个数据库文件  
)运行:./run.sh跑起服务端  

# 各模块的技术选型
)玩法逻辑:使用Unity2018自带的ECS系统(要用Unity的ECS只能用C#)，服务端也用lua实现一套类似的ECS系统  
)界面逻辑:使用自制的基于组件的UI系统,全lua开发,动画也由lua实现了一份cocos的action  
)网络协议:使用sproto,玩法用c#版本,界面用lua版本  
)场景管理:用T2M切割地形为NxN小块,使用四叉树管理场景模型的动态加载  
)资源管理:使用Unity新版的AssetBundleBrowser打包资源  
)数据管理:使用redis,后面再看看要不要加入mysql  
)同步模式:基于请求回应的状态+差异同步  

# 已完成
)前端用Luaframework的网络接口,后端用skynet的loginserver通过登录验证  
)使用sproto协议,按模块分文件和id组,开发时拼接所有协议文件,发布版则预导出为二进制(支持lua和c#)  
)搭建apache服务器提供资源和lua代码的热更新  
)增加后端通用的mysql数据库服务  
)在PC和安卓手机平台测试通过了,可以连虚拟机上的服务端并登录  
)给xlua集成了lpeg,sproto,lua-crypt第三方库  
)创建玩家帐号数据库和相关操作服务  
)登录流程相关界面  
)导出场景信息（前端json后端lua格式）  
)九宫格加载场景块  
)玩家进入退出场景及坐标信息的同步  

# Todo
前端:   
)人物场景漫游-ECS做法(70%)  
)场景切割及动态加载(85%)  
)基于组件的UI框架(85%)  
)人物动作方面等Unity的新版Animation系统(IAnimationJob)完善后再介入吧  
)战斗系统  
)场景模型LOD,试试UnityGithub上的AutoLOD  

后端:  
)lua版本的ECS(60%)  
)人物的移动同步(80%)  
)NPC与怪物AI  
)使用Redis  
)AOI  

# 进况
由于unity的entities版本更新修改较多导致之前的部分逻辑要重写了，感觉还是暂时先不弄前端的game play逻辑较好，先弄寻路和后端逻辑吧  
初步完成的大世界场景分块加载（还在考虑无限场景的NavMesh资源管理，先尝试用跳跃点连接n个地图块的navmesh）:  
![image](https://github.com/liuhaopen/ReadmeResources/blob/master/UnityMMO/run_in_terrain.gif)  
