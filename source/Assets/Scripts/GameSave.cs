using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;

//游戏保存读取
public class GameSave : MonoBehaviour
{
    //是否需要读取存档
    public static bool LoadGame = false;
    //存档数据
    public static Save file;
    //存档信息
    public static SaveList fileInfo;
    //当前游戏存档
    public static int curSave = -1;

    void Start()
    {   
        //需要读取存档
        if(LoadGame){
            Chunk.loadFinish = false;
            LoadByJson(StartGame.SaveNum);
            Chunk.loadFinish = true;
        }
        getSaveInfo();
    }
    //创建存档文件
    public static void CreateStoreFile(){
        string dirPath = Application.dataPath + "/Save";
        //判断要读取的游戏文档信息是否存在
        if (!Directory.Exists(dirPath)) {
            Debug.Log("dirNotExist!");
            Directory.CreateDirectory(dirPath);
        }
        string filePath = Application.dataPath + "/Save/saveInfo.json";
        if (!File.Exists(filePath)){
            Debug.Log("saveInfoNotExist");
            File.Create(filePath).Dispose();
            SaveList slInfo = new SaveList();
            for(int i =0;i<3;i++){
                slInfo.slist.Add(new SaveInfo());
                slInfo.slist[i].day = -1;
            }
            Debug.Log(slInfo.slist);
            Debug.Log(slInfo);

            string saveJsonStr = JsonMapper.ToJson(slInfo);
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(saveJsonStr);
            sw.Close();
        }

        for(int i=1;i<4;i++){
            string SavePath =Application.dataPath + "/Save/"+i.ToString()+".json";
            if (!File.Exists(SavePath)){
                File.Create(SavePath);
            }
        }
    }

    //将普通的Vector3类型转化为自定义的Vector3D类型
    public static myVector3 convertVector(Vector3 tmp){
        myVector3 mtmp= new myVector3();
        mtmp.x = (double)tmp.x;
        mtmp.y = (double)tmp.y;
        mtmp.z = (double)tmp.z;
        return mtmp;
    }

    //创建游戏存档
    public static Save CreateSave(){
        int[] map;
        int p;
        Save save = new Save();

        GameObject[]  Player = GameObject.FindGameObjectsWithTag ("Player");
        
        myVector3 PlayerPos = convertVector(Player[0].transform.position);
        myVector3 PlayerRotation = convertVector(Player[0].transform.eulerAngles);

        GameObject[]  Enemy = GameObject.FindGameObjectsWithTag ("enemy");
        if(Enemy.Length>0){
            Debug.Log(Enemy.Length);
            myVector3 EnemyPos = convertVector(Enemy[0].transform.position);
            Debug.Log("ENEMYPOS: "+Enemy[0].transform.position);
            save.EnemyPos = EnemyPos;
            save.EnemyStatus = true;
        }else{
            save.EnemyStatus = false;
        }
        
        save.day = Timer.day;
        save.hour = Timer.hour;
        save.weather = Timer.weather;
        save.PlayerPos = PlayerPos;
        save.PlayerRotation = PlayerRotation;

        for(int i=0;i<Chunk.chunks.Count;i++){
            Vector3 tempPos = Chunk.chunks[i].transform.position;
            save.chunksPos.Add(convertVector(tempPos));
            map = new int[Chunk.width* Chunk.height* Chunk.width];
            p = 0;
            for (int x = 0; x < Chunk.width; x++)
            {
                for (int y = 0; y < Chunk.height; y++)
                {
                    for (int z = 0; z < Chunk.width; z++)
                    {
                        map[p] = (int)Chunk.chunks[i].map[x,y,z];
                        p++;
                    }
                }
            }
            save.BlockInfo.Add(map);
        }

        return save;
    }

    //保存游戏存档
    public static void SaveJson(int fname){
        Save save = CreateSave();
		//定义字符串filePath保存文件路径信息
		string filePath = Application.dataPath + "/Save/" + fname+ ".json";
        Debug.Log(filePath);
		//利用JsonMapper将save对象转换为Json格式的字符串
		string saveJsonStr = JsonMapper.ToJson(save);
		//将这个字符串写入到文件中
		//创建一个StreamWriter,并将字符串写入到文件中
		StreamWriter sw = new StreamWriter(filePath);
		sw.Write(saveJsonStr);
		//关闭StreamWriter
		sw.Close();
    }
    
    //读取游戏存档
    public static void LoadByJson(int fname)
	{
		//定义字符串filePath读取文件路径信息
		string filePath = Application.dataPath + "/Save/" + fname + ".json";
		//判断要读取的游戏文档信息是否存在
		if (File.Exists(filePath))  
		{
			//创建一个StreamReader,用来读取流
			StreamReader sr = new StreamReader(filePath);
			//将读取到的流赋值给jsonStr
			string jsonStr = sr.ReadToEnd();
			//关闭
			sr.Close();

			//将字符串jsonStr转换为Save对象
			file = JsonMapper.ToObject<Save>(jsonStr);
            Debug.Log("chunk num in file:"+file.chunksPos.Count);

            Chunk.SetChunks();
            SetPlayer();
		}
		else
		{
			Debug.Log("存档文件不存在");
		}
	}

    //保存存档信息
    public static void storeSaveInfo(){
		string filePath = Application.dataPath + "/Save/saveInfo.json";
		string saveJsonStr = JsonMapper.ToJson(fileInfo);
		StreamWriter sw = new StreamWriter(filePath);
		sw.Write(saveJsonStr);
		sw.Close();
    }

    //获取存档信息
    public static void getSaveInfo(){
        string filePath = Application.dataPath + "/Save/saveInfo.json";
		//判断要读取的游戏文档信息是否存在
		if (File.Exists(filePath))  
		{
			//创建一个StreamReader,用来读取流
			StreamReader sr = new StreamReader(filePath);
			//将读取到的流赋值给jsonStr
			string jsonStr = sr.ReadToEnd();
			//关闭
			sr.Close();
			//将字符串jsonStr转换为Save对象
			fileInfo = JsonMapper.ToObject<SaveList>(jsonStr);
            Debug.Log("getSaveInfo Finish");
		}
		else
		{
			Debug.Log("存档信息文件不存在");
		}
    }

    
    //根据存档设置玩家和敌人的位置
    public static void SetPlayer(){
        GameObject[]  Player = GameObject.FindGameObjectsWithTag ("Player");
        Vector3 PlayerPos = new Vector3((float)file.PlayerPos.x,(float)file.PlayerPos.y,(float)file.PlayerPos.z);
        Player[0].transform.position = PlayerPos;
        Vector3 PlayerRotation = new Vector3((float)file.PlayerRotation.x,(float)file.PlayerRotation.y,(float)file.PlayerRotation.z);
        Player[0].transform.Rotate(PlayerRotation);
        // Debug.Log(Player[0].transform.eulerAngles);
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag ("enemy");
        if(file.EnemyStatus){
            Debug.Log(Enemy.Length);
            Vector3 EnemyPos = new Vector3((float)file.EnemyPos.x,(float)file.EnemyPos.y,(float)file.EnemyPos.z);
            Debug.Log("set enemy pOs:"+EnemyPos);
            Enemy[0].transform.position = EnemyPos;
        }else{
            Enemy[0].GetComponent<Enemy>().explodeFinish = true;
            Enemy[0].SetActive(false);
            Enemy[1].SetActive(false);
        }
    }

}
