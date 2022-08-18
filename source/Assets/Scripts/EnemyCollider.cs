using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    //碰撞判断，判断是否需要跳跃跨过地形
    public void OnTriggerEnter(Collider collider){

        Enemy.gravity = 11f;
    }

    public void OnTriggerExit(Collider collider){

        Enemy.gravity = -9.8f;
    }

}