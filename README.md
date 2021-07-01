# UnityMMO
很多东西不好在工作项目上尝试(比如 ECS),所以就有了本项目,我打算利用业余时间从头制作一个 3D-MMO 游戏,大部分功能虽然都多少接触过,但我想换个做法,不然就不好玩了.前端主要用 lua 开发.后端用 skynet.  
详细的设计和进度见：[Wiki](https://github.com/liuhaopen/UnityMMO/wiki "Wiki")  

# Usage
克隆本项目: git clone https://github.com/liuhaopen/UnityMMO.git --recurse  
前端:  
下载下来后整个目录就是 Unity 的项目目录,用 Unity 打开,运行 main.unity 场景即可进入游戏的登录界面  
注:由于游戏资源过大且经常变更(每个版本的资源都会保存在 .git 文件夹里, clone 就要好久了),所以放到另外的项目管理,可在 [UnityMMO-Resource](https://github.com/liuhaopen/UnityMMO-Resource/tree/master/Assets/AssetBundleRes "UnityMMO-Resource") 下载里面的文件并把 Assets/AssetBundleRes 及其 meta 文件复制到本项目的 Assets 目录里(注:有些插件因为版权问题就没上传了,从其中的 download-page 见购买链接)  
后端:  
参考项目：[SkynetMMO](https://github.com/liuhaopen/SkynetMMO "SkynetMMO")  

# Status & Prerequisites
```
Unity version: 2019.4.28f1
Platforms    : 
client for Windows Android IOS;  
server only for Linux;
```

# Recent GIF
21.06.28：决定不用 Unity 的 Entities 了，主要是不好热更，而且又不好用，所以还是用 lua 另外实现了一套 [ecs](https://github.com/liuhaopen/ecs "ecs")，后面也会把战斗，场景相关的逻辑也挪到 lua 上，做成可以实际用的项目吧。  
20.03.08：把服务器代码分隔到另外的 git 项目
19.07.03：初步实现了自动寻路去找 npc 对话和打怪两种任务：   
![image](https://github.com/liuhaopen/ReadmeResources/blob/master/UnityMMO/auto_talk_and_fight.gif)     
19.07.10：增加一个副本场景：    
![image](https://github.com/liuhaopen/ReadmeResources/blob/master/UnityMMO/change_scene.gif)     
19.07.31：初步完成背包和 GM 系统      
19.08.11：初步完成基于 action 组件的技能系统,见 Server/lualib/Action及 FightMgr,Hurt 和 PickTarget.lua          
19.08.13：完成复活流程      
19.08.28：最近经常在手机上测试，优化了一波：摄像机操作，资源预加载，对象池，和使用了 AutoLOD 插件为各场景节点生成了两级简模（其实很多模型在最远处时是可以用一个面片替代的，就是做成公告板永远面向摄像机，但没美术资源就算了），树的话删了不少上万三角面的了。灯光烘培改成用 Distance ShadowMask,近处实时阴影远处贴图。暂时可以在我的垃圾手机流畅运行了。    
19.09.07：后端增加 buff:火，毒，冰冻，晕眩，吸血，扣属性(防御、攻击等)，沉默。详见 BuffActions.lua。前端目前只加了吸血和晕眩的效果。  
19.09.18：增加各平台的图片格式管理工具，针对不同用途的图片使用不同深度，比如安卓平台时 ui 文件夹里的图片用 ETC2_RGBA8，模型图片用 ETC_RGB8 等等，在 iOS 就用 ASTC 系列的格式。详细见 unity 编辑器菜单：TextureFormatter   
19.10.21：最近忙工作上的事，而且想要憋个大招，所以未来一个月进度会慢下来，先搞下小 ui 界面。  