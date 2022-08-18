using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏存档内容
[System.Serializable]
public class Save  {

    //天数
	public int day;
    //小时
    public int hour;
    //天气
    public int weather;
    //玩家位置
    public myVector3 PlayerPos;
    //敌人位置
    public myVector3 EnemyPos;
    //敌人死亡状态
    public bool EnemyStatus;
    //玩家朝向
    public myVector3 PlayerRotation;
    //每个chunk的位置
    public List<myVector3> chunksPos = new List<myVector3>();
    //各个chunk中对应的block信息
    public List<int[]> BlockInfo = new List<int[]>();
}
