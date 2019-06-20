# 资源文件来源
先在[UnityMMO-Resource](https://github.com/liuhaopen/UnityMMO-Resource/tree/master/Assets/AssetBundleRes "UnityMMO-Resource")下载里面的文件并把Assets/AssetBundleRes及其meta文件复制到本目录里

# 资源ID命名规则
角色怪物NPC的类型ID都为4位数字：千分位代表场景节点类型，其中1角色，2怪物，3NPC  
角色类型ID的百分位为性别，1男，2女，3人妖（开玩笑的），十和个位用于皮肤，所以1203就代表第3套皮肤的女角色   
怪物和NPC类型ID的其它位用于区分不同怪物或NPC而已，如2001表示某怪物，3001表示某NPC，就是说预留999个不同的怪物和NPC      
技能ID都为6位数字:  
前四位表示附属的场景节点类型ID，后两位递增，比如120001，前面的1200就是代表女角色，后面的01就代表她使用的某技能；又比如200105，前面的2001代表某只怪物，后面的05表示它的某技能。

