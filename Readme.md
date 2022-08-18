### · 项目名称：

​    我的世界

---

### · 简介：

  《我的世界》是一款出色的开放世界游戏。游戏世界由许多不同类型方块组成。玩家可以通过拆除和放置方块构建自己所想要的世界，也可以不断探索全新的世界。本项目为模仿《我的世界》游戏的核心玩法实现的简易版《我的世界》游戏。

---

### · 项目工程使用的技术、工具和软件版本：

​	项目使用了Unity引擎（2022.1.7f1c1版本）

---

### · 使用方法：

​	工程项目： 使用Unity打开unity文件运行即可

​	打包项目： 下载 **myMC.rar** 解压后运行 **myMineCraft.exe** 即可

​	

​	**W ，A，S，D：**控制玩家移动

​	**Q：**切换人称

​	**E：**更改手中方块

​	**`(反引号键， ESC下面的按键)：**打开游戏操作菜单

​	**鼠标左键：**放置方块/攻击敌人

​	**鼠标右键：** 放置方块

---

### · 技术方案简介：

1. **地图生成以及动态添加：**

   地图由多个Chunk组成，每个Chunk维护一个Block数组记录这个Chunk下不同坐标的方块类型。采用柏林噪声模拟使得地形平滑过渡。不同高度对应不同的方块类型。玩家有一定的视野范围，当玩家移动时，如果视野为内查找不到Chunk说明已经到了已生成的地图的边界，需要添加新的Chunk，使得地图动态生成。

   为减少渲染开销，实际上地图上并不存在一个又一个的方块，而是通过网格渲染绘制出方块的可被玩家看见的面从而模拟方块，即如果某面贴着的方块类型并不是空气（可被玩家看见）则渲染，反之则不渲染，减少渲染压力。

2. **放置/破坏方块：** 

   首先通过鼠标射线获得某个Block某面上某点的坐标，因为Block并不时一个直接的方块，所以不能够直接删除或隐藏。但可以通过如下方式将Block上点的物理坐标到此Block在的Chunk中数组位置，修改其类型为空气后重新渲染以达到删除方块的效果：首先要通过这个点坐标获得Block的物理坐标。所有Block的坐标为整数且边长为一，所以该点坐标一定有一个维度的值为整数，结合玩家与该点在此维度下的相对位置，可以确定此点在该Block的哪个面上。进而可以坐标换算得到Block的物理坐标，减去其所在Chunk的坐标后即可得到其在数组中的位置。放置方块方式类似，通过相同的方法明确鼠标射线点在Block的哪个面上，在修改对应方向上相邻Block的类型为手中持有的方块类型，渲染更新。需要处理边界条件，即被修改的Block属于另一个Chunk的情况。

3. **时间天气系统：**

   通过定时器实现时间系统，太阳旋转角度根据当前时间来确定，实现昼夜更替，且保证同步。每天24点时，随机更新天气，有晴天、下雨以及下雪。下雨和下雪通过粒子系统实现。粒子绑定在人物上模拟全场景天气，并不需要绑定在整个地图上。

4. **怪物逻辑：**

   随机生成一个怪物，怪物不断向玩家移动，当和玩家距离在一定范围内时自动爆炸，破坏周围一定范围内所有方块。怪物受击会减少生命值，生命值为零的时候会消失。怪物受击会有短暂的霸体，无法在此期间造成伤害。每天24点时，如果怪物已爆炸或是死亡，会在玩家一定范围内重新生成一个新怪物。

5. **存档读档：**

   维护三个存档文件以及一个存档信息文件，采用Json序列化。保存了地图、玩家以及怪物的相关信息。如果玩家选择读取存档，先将存档中的内容添加渲染。

​		

---

### · 游戏内容展示：

​	游戏主界面，玩家可以选择开始新的游戏或者是读取存档

![image-20220817104519762](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817104519762.png)



​	读取存档界面，会读取已保存的存档信息，包括日期，时间，天气以及存档时间

![image-20220817105059110](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105059110.png)



​	游戏基本界面

![image-20220817105301093](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105301093.png)



​	放置破坏方块

![image-20220817105719970](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105719970.png)

![image-20220817105810973](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105810973.png)



​	怪物受击

![image-20220817105545956](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105545956.png)



​	怪物爆炸

![image-20220817105425888](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105425888.png)

![image-20220817105447880](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105447880.png)



​	菜单界面

![image-20220817105921284](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105921284.png)



​	存档界面

![image-20220817105951908](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817105951908.png)

---

### · 工程目录结构说明：

![image-20220817152812134](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20220817152812134.png) 

**Material：**方块材质球

**Plugins：**插件

**Save：**游戏存档

**Scene：**游戏场景

**Scripts**：游戏脚本

**Texture：**图片资源
