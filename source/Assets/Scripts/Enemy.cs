using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//敌人逻辑控制
public class Enemy : MonoBehaviour
{
    System.Random myRandom = new System.Random();

    //是否开始爆炸
    public bool explode;
    //爆炸是否完成
    public bool explodeFinish;
    //霸体
    public bool invincible;
    //霸体时间
    public float invincibleTime;
    //死亡延迟
    public float deadTime=1f;
    //爆炸延迟
    public float explodeTime = 2f;
    //血量
    public int hp;

    public GameObject enemyCube;

    Rigidbody rig;
    //玩家
    public Transform m_player;
    //速度
    public int speed = 3;
    //跳跃速度
    public static float jumpSpeed = 9.8f;
    //敌人朝向
    Vector3 FaceDirection;
    //行动方向
    Vector3 MoveDirection;
    public static float gravity =-9.8f;

    //初始化
    void Start(){
        hp = 2;
        rig = GetComponent<Rigidbody>();
        invincible = false;
        explode = false;
        explodeFinish = false;
        invincibleTime = 0.2f;
    }


    void Update(){
        //hp<0，死亡
        if(hp<0){
            if(deadTime>0){
                transform.Rotate(0,0,90f/0.8f*Time.deltaTime);
            }else{
                gameObject.SetActive(false);
                enemyCube.SetActive(false);
            }
            deadTime-=Time.deltaTime;
            return;
        }
        //霸体状态
        if(invincible){
            invincibleTime-=Time.deltaTime;
            if(invincibleTime<0){
                GetComponent<MeshRenderer>().material.color = Color.white;
                enemyCube.GetComponent<MeshRenderer>().material.color = Color.white;
                invincible = false;
                invincibleTime = 0.2f;
            }
        }

        //朝向玩家
        FaceDirection = m_player.position;
        FaceDirection.y = transform.position.y;
        transform.LookAt(FaceDirection);
        
        //爆炸
        if(explode){
            GetComponent<MeshRenderer>().material.color = Color.black;
            enemyCube.GetComponent<MeshRenderer>().material.color = Color.black;
            rig.velocity =new Vector3(0,0,0);
            if(explodeTime<0){
                if(!explodeFinish)
                    EnemyExplode();
                gameObject.SetActive(false);
                enemyCube.SetActive(false);
            }
            explodeTime-=Time.deltaTime;
            return;
        }

        //判断和玩家的距离，一定距离内爆炸
        if((m_player.position-transform.position).magnitude<5){
            rig.velocity =new Vector3(0,0,0);
            explode = true;
            return;
        }

        //朝向玩家移动
        MoveDirection=m_player.position - transform.position;
        MoveDirection.y = gravity;
        rig.velocity = MoveDirection.normalized ;
        rig.velocity = MoveDirection.normalized * speed;

        if(transform.position.y <0){
            Vector3 tmp = new Vector3(m_player.transform.position.x+myRandom.Next(15,30), m_player.transform.position.y+5, m_player.transform.position.z+myRandom.Next(15,30));
            transform.position = tmp;
        }
        
        if(transform.position.y - m_player.position.y>3)
            Enemy.gravity = -9.8f;
    }

    //被攻击
    public void attacked(){
        if(explode)
            return;
        if(!invincible){
            hp--;
            GetComponent<MeshRenderer>().material.color = Color.red;
            enemyCube.GetComponent<MeshRenderer>().material.color = Color.red;
            invincible = true;
        }
    }
    
    //爆炸摧毁地形
    public void EnemyExplode(){
        explodeFinish = true;

        Vector3 EnemyPos = new Vector3(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.y),Mathf.Floor(transform.position.z));
        Debug.Log("enemyPos: "+EnemyPos);
        Chunk chunk = Chunk.GetChunk(true,EnemyPos);
        Vector3 tmp = new Vector3();
        for(int x=-2;x<3;x++)
            for(int y=-4;y<2;y++)
                for(int z=-2;z<3;z++){
                    tmp.x = x;
                    tmp.y = y;
                    tmp.z = z;
                    tmp = tmp + EnemyPos - chunk.transform.position;
                    Debug.Log(tmp);
                    if(tmp.y<0)
                        continue;
                    if(tmp.x<0 || tmp.x>=30 || tmp.z<0 || tmp.z>=30){
                        Debug.Log("tmp error: "+tmp);
                        tmp += chunk.transform.position;
                        Chunk tmpChunk = Chunk.GetChunk(true,tmp);
                        tmp = tmp - tmpChunk.transform.position;
                        tmpChunk.map[(int)tmp.x,(int)tmp.y,(int)tmp.z] = Block.None;
                        tmpChunk.BuildChunk();
                    }else{
                        Debug.Log("tmp: "+(int)tmp.x+ ' '+ (int)tmp.y+ ' '+ (int)tmp.z);
                        chunk.map[(int)tmp.x,(int)tmp.y,(int)tmp.z] = Block.None;
                    }
                }
        chunk.BuildChunk();
        
    }

}
