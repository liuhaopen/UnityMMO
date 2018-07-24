# UnityMMO
做游戏几年了,很多东西不好在工作项目上尝试(比如ECS),所以就有了本项目,我打算利用业余时间从头制作一个3D-MMO游戏,很多功能虽然都接触过,但我想换个做法(不然就不好玩了),反正没人逼着上线.框架上前端使用unity的Luaframework.后端用Skynet  

# 已完成
)前端用Luaframework的网络接口,后端用skynet的loginserver通过登录验证  
)使用sproto协议,按模块分文件和id组,开发时拼接所有协议文件,发布版则预导出为二进制  
)搭建apache服务器提供资源和lua代码的热更新  
)增加后端通用的数据库服务(有mysql增删改接口就行)  
)在PC和安卓平台测试通过了,可以连虚拟机上的服务端并登录  

# Todo
前端:  
)基于组件的UI框架(70%)  
)飘字提示  
)选择和创建角色界面(70%)  
)主界面：移动摇杆，操作按钮
)战斗系统    
)人物操作  
)本地存档  
)报错界面(10%)  
)资源管理:增加本地加载模式(直接读取本地lua和资源,Luaframework每次改界面都要打包资源这太难受了,目前只有一界面暂时不做)(30%)  
)管理SDK接入  

后端:  
)创建玩家帐号数据库和相关操作服务(5%)  
)人物的移动同步  
)NPC与怪物AI  

# 预览
![image](https://github.com/liuhaopen/UnityMMO/blob/master/Preview/LoginView.jpg)  

![image](https://github.com/liuhaopen/UnityMMO/blob/master/Preview/CreateRole.jpg)