using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//时间天气系统
public class Timer : MonoBehaviour
{
    public GameObject enemyCube;
    public GameObject enemy;
    public GameObject Player;
    //天数
    public static int day =0;
    //小时
    public static int hour = 8;
    //每小时在游戏中有多少秒
    public double intervaletime=5;
    //计时器
    public double timer; 

    public static GameObject sun;
    public static Text hourText;
    public static Text dayText;
    public static Text weatherText;

    double sunRotateSpeed;

    static GameObject Snow;
    static GameObject Rain;
    
    System.Random myRandom = new System.Random();
    public static int weather;

    //根据存档更新时间、天气
    public static void init(){
        if(GameSave.LoadGame){
            day = GameSave.file.day;
            hour = GameSave.file.hour;
            weather = GameSave.file.weather;
        }
        sun = GameObject.FindGameObjectsWithTag("sun")[0];
        sun.transform.Rotate(-180+hour*15,0,0);
        Snow = GameObject.FindGameObjectsWithTag("snow")[0];
        Rain = GameObject.FindGameObjectsWithTag("rain")[0];
        hourText = GameObject.FindGameObjectsWithTag("hourtext")[0].GetComponent<Text>();
        weatherText = GameObject.FindGameObjectsWithTag("weathertext")[0].GetComponent<Text>();
        dayText = GameObject.FindGameObjectsWithTag("daytext")[0].GetComponent<Text>();

        dayText.text = "Day "+day.ToString();;
        hourText.text =  hour>9?hour.ToString()+":00":"0"+hour.ToString()+":00";
        if(weather<=6){
                weatherText.text = "晴天";
                Snow.GetComponent<ParticleSystem >().Stop();
                Rain.GetComponent<ParticleSystem >().Stop();
            }else if(weather<=8){
                weatherText.text = "下雪";
                Snow.GetComponent<ParticleSystem >().Play();
                Rain.GetComponent<ParticleSystem >().Stop();
            }else {
                weatherText.text = "下雨";
                Snow.GetComponent<ParticleSystem >().Stop();
                Rain.GetComponent<ParticleSystem >().Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
        sunRotateSpeed = (double)360/24/intervaletime;
        timer = intervaletime;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTime();
        RotateSun();
    
    }

    //太阳旋转
    void RotateSun(){
        sun.transform.Rotate((float)sunRotateSpeed*Time.deltaTime,0,0);
    }

    //时间改变
    void ChangeTime(){
        if (hour < 24)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {

                timer += intervaletime;
                hour+=1;
                hourText.text =  hour>9?hour.ToString()+":00":"0"+hour.ToString()+":00";
            }
        }
        //每过一天如果更新天气以及敌人死亡刷新
        if (hour >= 24)
        {
            if(enemy.GetComponent<Enemy>().explodeFinish || enemy.GetComponent<Enemy>().hp<0){
                enemy.GetComponent<Enemy>().hp = 2;
                Vector3 tmp = new Vector3(Player.transform.position.x+myRandom.Next(15,30), Player.transform.position.y+5, Player.transform.position.z+myRandom.Next(15,30));
                enemy.transform.position = tmp;
                enemy.GetComponent<Enemy>().invincible = false;
                enemy.GetComponent<Enemy>().explode = false;
                enemy.GetComponent<Enemy>().explodeFinish = false;
                enemy.GetComponent<Enemy>().invincibleTime = 0.2f;
                enemy.GetComponent<Enemy>().deadTime = 1f;
                enemy.GetComponent<Enemy>().explodeTime = 2f;
                enemy.GetComponent<MeshRenderer>().material.color = Color.white;
                enemyCube.GetComponent<MeshRenderer>().material.color = Color.white;
                enemy.SetActive(true);
                enemyCube.SetActive(true);
            }

            weather = myRandom.Next(1, 11);
            if(weather<=6){
                weatherText.text = "晴天";
                Snow.GetComponent<ParticleSystem >().Stop();
                Rain.GetComponent<ParticleSystem >().Stop();
            }else if(weather<=8){
                weatherText.text = "下雪";
                Snow.GetComponent<ParticleSystem >().Play();
                Rain.GetComponent<ParticleSystem >().Stop();
            }else {
                weatherText.text = "下雨";
                Snow.GetComponent<ParticleSystem >().Stop();
                Rain.GetComponent<ParticleSystem >().Play();
            }
            day+=1;
            dayText.text = "Day "+day.ToString();
            hour = 0;
        }
    }
}
