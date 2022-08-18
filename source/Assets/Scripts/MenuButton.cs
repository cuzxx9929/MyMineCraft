using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

//菜单按钮
public class MenuButton : MonoBehaviour
{

    public Image img1;
    public Image img2;
    public Image img3;
    
    public Color colorChosed;
    public Color colorUnchosed;

    public int SaveNum;
    public GameObject Menu;
    public GameObject Mouse;
    public GameObject OpInfo;
    public GameObject ToHomeVerify;
    public GameObject Store;

    // Start is called before the first frame update
    void Start()
    {
        SaveNum = 0;
        ColorUtility.TryParseHtmlString("#494949", out colorChosed);
        ColorUtility.TryParseHtmlString("#6E6E6E", out colorUnchosed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //继续游戏
    public void ContinueGame(){
        PanelController.PanelShow = false;
        Menu.SetActive(false);
        Mouse.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //回到游戏开始界面
    public void BackToHome(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    //显示操作信息
    public void ShowOpInfo(){
        Menu.SetActive(false);
        OpInfo.SetActive(true);
    }

    //返回菜单目录
    public void BackToMenu(){
        Menu.SetActive(true);
        OpInfo.SetActive(false);
        ToHomeVerify.SetActive(false);
        Store.SetActive(false);

    }

    //返回游戏开始界面确认
    public void ToHomeVerifyShow(){
        ToHomeVerify.SetActive(true);
        Menu.SetActive(false);
    }

    //保存游戏界面
    public void ToStore(){
        ToHomeVerify.SetActive(false);
        Menu.SetActive(false);
        Store.SetActive(true);
    }

    //选择存档一
    public void Choose1(){
        SaveNum = 1;
        // Debug.Log(colorChosed,colorUnchosed);
        img1.color = colorChosed;
        img2.color = colorUnchosed;
        img3.color = colorUnchosed;

    }

    //选择存档二
    public void Choose2(){
        SaveNum = 2;
        img1.color = colorUnchosed;
        img2.color = colorChosed;
        img3.color = colorUnchosed;
    }

    //选择存档三
    public void Choose3(){
        SaveNum = 3;
        img1.color = colorUnchosed;
        img2.color = colorUnchosed;
        img3.color = colorChosed;
    }

    //确认保存
    public void confirmSave(){
        GameSave.fileInfo.slist[SaveNum-1].day = Timer.day;
        GameSave.fileInfo.slist[SaveNum-1].hour = Timer.hour;
        GameSave.fileInfo.slist[SaveNum-1].weather = Timer.weather;
        GameSave.fileInfo.slist[SaveNum-1].time = DateTime.Now.ToString();

        for(int i =0;i<SaveInfoController.playtags.Length;i++){
            SaveInfoController.playtags[i].SetActive(false);
        }

        if(SaveNum!=-1){
            SaveInfoController.playtags[SaveNum-1].SetActive(true);
        }

        GameSave.curSave = SaveNum;
        SaveInfoController.updateSaveInfo();
        GameSave.storeSaveInfo();
        GameSave.SaveJson(SaveNum);
    }
}

