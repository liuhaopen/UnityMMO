# UnityMMO
做游戏几年了,很多东西不好在工作项目上尝试(比如ECS),所以就有了本项目,我打算利用业余时间从头制作一个3D-MMO游戏,很多功能虽然都接触过,但我想换个做法(不然就不好玩了),反正没人逼着上线.前端上使用xlua,玩法系统用c#(主要是想用unity2018推的ECS,虽然目前还未完善各大系统都未对接不能用pure ecs),界面就用lua开发就行了.后端用Skynet(对比了其它几个开源项目后还是觉得skynet最简洁)  

# 使用方法
克隆本项目:git clone https://github.com/liuhaopen/UnityMMO.git --recurse  
前端:  
下载下来后整个目录就是Unity的项目目录,用Unity打开,运行main.unity场景即可进入游戏的登录界面  
注:由于游戏资源过大且经常变更(每个版本的资源都会保存在.git文件夹里,clone就要好久了),所以放到另外的项目管理,可在[UnityMMO-Resource](https://github.com/liuhaopen/UnityMMO-Resource/tree/master/Assets/AssetBundleRes "UnityMMO-Resource")下载里面的文件并复制到本项目的Assets/AssetBundleRes里(注:有些插件因为版权问题就没上传了,从其中的download-page见购买链接)  
后端:  
)安装虚拟机,我使用的是CentOS7,然后设置整个项目目录为虚拟机的共享目录,cd到Server目录,先编译skynet:[skynet主页](https://github.com/cloudwu/skynet "skynet主页")  
)在虚拟机安装mysql并导入Server/data/里的两个数据库文件  
)运行:./run.sh跑起服务端  

# 开发笔记
见[Wiki](https://github.com/liuhaopen/UnityMMO/wiki/%E5%BC%80%E5%8F%91%E7%AC%94%E8%AE%B0 "Wiki")  

# 近况Gif
完成怪物追捕和攻击的逻辑：    
![image](https://github.com/liuhaopen/ReadmeResources/blob/master/UnityMMO/monster_fighting.gif)     