using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//玩家移动控制
public class PlayerMove : MonoBehaviour
{
    public Chunk chunkPrefab;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = -9.8f;
    public float jumpHeight = 10f;
    float viewRange = 30;

    private Vector3 velocity;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {  
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //移动处理
        Move();
        //判断是否需要更新地图,生成新的Chunk
        if(Chunk.chunks.Count >= Chunk.ChunkNum-1)
            BuildChunk();
    }

    //玩家移动
    void Move(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        controller.Move(moveDirection * speed * Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -1 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //动态更新地图
    void BuildChunk()
    {

        for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += Chunk.width)
        {
            for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += Chunk.width)
            {
                Vector3 pos = new Vector3(x, 0, z);
                pos.x = Mathf.Floor(pos.x / (float)Chunk.width) * Chunk.width;
                pos.z = Mathf.Floor(pos.z / (float)Chunk.width) * Chunk.width;
                Chunk chunk = Chunk.GetChunk(false,pos);                                                                                                                                                         
                if (chunk != null) continue;
                chunk = (Chunk)Instantiate(chunkPrefab, pos, Quaternion.identity);
            }
        }
    }


}