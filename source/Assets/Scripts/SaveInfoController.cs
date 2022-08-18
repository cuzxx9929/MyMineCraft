using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//游戏存档信息显示
public class SaveInfoController : MonoBehaviour
{
    public static GameObject[] playtags;
    // Start is called before the first frame update
    void Start()
    {
        updateSaveInfo();
        playtags = GameObject.FindGameObjectsWithTag ("playtag");
        for(int i =0;i<playtags.Length;i++){
            playtags[i].SetActive(false);
        }
        if(GameSave.curSave!=-1){
            playtags[GameSave.curSave-1].SetActive(true);
        }
    }

    //更新游戏存档信息
    public static void updateSaveInfo(){
        int day;
        string weather,hour;
        GameObject[] saveinfo = GameObject.FindGameObjectsWithTag ("saveinfo");
        GameObject[] saveinfotime = GameObject.FindGameObjectsWithTag ("saveinfotime");


        for(int i=0;i<saveinfo.Length;i++){
            day = GameSave.fileInfo.slist[i].day;
            hour = GameSave.fileInfo.slist[i].hour>9?GameSave.fileInfo.slist[i].hour.ToString()+":00":"0"+GameSave.fileInfo.slist[i].hour.ToString()+":00";
            if(GameSave.fileInfo.slist[i].weather<=6){
                    weather = "晴天";
                }else if(GameSave.fileInfo.slist[i].weather<=8){
                    weather = "下雪";
                }else {
                    weather = "下雨";
            }
            if(day<0){
                saveinfo[i].GetComponent<Text>().text = "暂无存档";
                continue;
            }
            saveinfotime[i].GetComponent<Text>().text = GameSave.fileInfo.slist[i].time;
            saveinfo[i].GetComponent<Text>().text = 
                "DAY "+day.ToString()+"  "+hour+"  "+weather;
        }
        
    }
}
