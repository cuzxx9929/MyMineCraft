using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//游戏存档信息
public class SaveInfo {
    //天数
	public int day;
    //小时
    public int hour;
    //天气
    public int weather;
    //更新时间
    public string time;
}

[System.Serializable]
public class SaveList  {
    public List<SaveInfo> slist = new List<SaveInfo>();
}