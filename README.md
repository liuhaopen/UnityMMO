# UGUI开发流程

UI编辑器扩展相关的代码和资源都放在Assets/UIEditor目录里,默认大部分功能都是开启的,如果你觉得哪些用得不顺手,可以在UIEditor/Configure.cs文件里关闭相应功能,设为false保存一下就会立即生效的:
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/img_1.png)


## PrefabWin窗口

一般一些通用常用的资源我们都会做成prefab,比如某些按钮,文本样式等,然后用到时就在Project视图把它拉入场景,但说实话Project视图真的太丑了,你是想这样的?
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/img_2.png)

还是这样的?
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/img_3.png)

图2对于有用过NGUI的人应该不陌生(NGUI可是UGUI之前的霸主啊!)

所以用UIEditor的开发流程一般就是从PrefabWin这个窗口拉控件出去的,拉到场景时会判断控件落在哪个Canvas上,有则挂其上,无则自动生成一个给你,然后右键保存为一个界面prefab,或者随便你怎么处理那坨界面:  
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/prefab%20win.gif)  
PrefabWin窗口可以从菜单Window-&gt;PrefabWin打开  
PrefabWin窗口一开始时肯定是没东西的啦,你可以往里面拉prefab,然后它会自动生成预览图的.2D3D的prefab都可以.底下还有个搜索框可以让你快速过滤.  

## 参考图
一般做界面,我们就按美术出的图,那里拉个按钮,那里弄个文本,其坐标大小比例等肯定要严格按美术出的图来调节的,所以编辑界面时最好就有张参考图,添加方式如下:  
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/consult%20pic.gif)  
参考图的资源可来自项目外的目录,且右键保存界面时会跳过它的.另外就是每当你保存该界面,UIEditor会保存该界面上的所有参考图的图片和大小坐标等信息,下次打开时会自动加载.    
添加参考图后,可以选中它后右键菜单-&gt;锁定,这样就不会碍着你了.    
对了,顺便说下选中节点后可以用方向键调节节点的坐标,每次加减1.  

## 拉图生成Image节点  
在Project视图拉图到场景的Canvas(无则自动生成Canvas)将生成一Image节点并把图赋在其上.还有就是选中Image节点时再点击Project视图里的图片也可以赋上该图.  
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/drag%20pic.gif)

## 优化层次
下图有8个节点,其中4个图片中,有两个图片来自图集1,两个来自图集2,如果它们是按图集连续排的话就可以合为同一批次,但被其它图集打断就合不了了,另外4个text也是一样,同一字体的也是可以合为一批次的,这个功能就是自动排列好顺序优化合批:  
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/optimize_depth_for_batch_draw.gif)  

## 合并组和解体

有时需要把几个节点合成一个组,这时可以这样:
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/make%20group.gif)


## 排列和清理所有界面

![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/sort%20and%20clean.gif)

## 对齐工具

![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/img_4.png)
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/align%20tool.gif)


## 其它功能
)运行结束后重新加载所有正在编辑的界面(因为运行期间的修改在运行结束后会重置的)  
)记录每个界面的参考图信息(现在每次重新打开界面都要添加一次参考图太麻烦了)  
)增加右键菜单:优化层级,文字放一起,同一图集的放一起  
)按ctrl+shift+c可以复制选中的节点名到剪切板上,生成的字符串是带路径的:
![image](https://github.com/BalckCat/UIEditor/blob/master/Picture/img_5.png)


## TODO  
)界面优化大全:选中某界面后遍历其所有子节点并在一个window列出优化建议(比如Text别用bestfix,用到了其它图集的小图等等)  
)支持大部分操作的Undo(在操作前用Undo这个工具类记录)(30%)  
)运行时打开的界面,退出运行时Unity会自动清掉的,所以我们要记录下来,退出时重新打开  
)右键显示颜色框(有时代码要设置颜色值可以用的)  
)Hierarchy界面也要显示我们的右键菜单  


