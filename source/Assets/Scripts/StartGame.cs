using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using UnityEngine.SceneManagement;

//游戏开始界面按钮控制
public class StartGame : MonoBehaviour {
    public static int SaveNum;

    public GameObject startButton;
    public GameObject loadButton;
    public GameObject storeMenu;

    public Image img1;
    public Image img2;
    public Image img3;
    
    public Color colorChosed;
    public Color colorUnchosed;

    void Start(){
        //新建存档文件
        GameSave.CreateStoreFile();
        SaveNum = 0;
        ColorUtility.TryParseHtmlString("#494949", out colorChosed);
        ColorUtility.TryParseHtmlString("#6E6E6E", out colorUnchosed);
        GameSave.LoadGame = false;
    }

    //加载存档
    public void LoadOldGame() {
        GameSave.getSaveInfo();
        startButton.SetActive(false);
        loadButton.SetActive(false);
        storeMenu.SetActive(true);
    }

    //开始新游戏
    public void StartNewGame() {
        GameSave.curSave = -1;
        GameSave.LoadGame = false;
        Chunk.ChunkNum = 0;
        Timer.day = 0;
        Timer.hour = 8;
        Timer.weather = 0;
        SceneManager.LoadScene("MainScene");
    }

    //返回游戏主界面
    public void BackToHome(){
        GameSave.LoadGame = false;
        startButton.SetActive(true);
        loadButton.SetActive(true);
        storeMenu.SetActive(false);
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

    //确定加载游戏
    public void confirmLoadGame(){
        if(GameSave.fileInfo.slist[SaveNum-1].day==-1){
            Debug.Log("can't");
            return;
        }
        GameSave.curSave = SaveNum;
        GameSave.LoadGame = true;
        SceneManager.LoadScene("MainScene");
    }
}