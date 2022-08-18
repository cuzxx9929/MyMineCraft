using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//玩家鼠标点击行为控制
public class MouseClick : MonoBehaviour
{

    public GameObject enemy;
    public RawImage BlockPic;

    public Texture Dirt;
    public Texture Gravel;
    public Texture Leaf;
    public Texture Grass;
    public Texture Wood;
    
    public Camera camera;

    public static Block curBlock= Block.Wood;

    // Update is called once per frame
    void Update()
    {
        //菜单打开，玩家行为失效
        if(PanelController.PanelShow){
            return;
        }

        //按下鼠标左键（破坏方块或攻击敌人）
        if (Input.GetMouseButtonDown(0)){
                DestroyBlock();
        }

        //按下鼠标右键（放置方块）
        if (Input.GetMouseButtonDown(1)){
                AddBlock();
        }
        //改变手中的方块
        if (Input.GetKeyDown("e")){
                ChangeBlockType();
        }
    }

    //改变玩家手中的方块
    void ChangeBlockType(){
        switch (curBlock)
        {
            case Block.Dirt:
                curBlock = Block.Grass;
                BlockPic.texture = Grass;
                return;
            case Block.Grass:
                curBlock = Block.Gravel;
                BlockPic.texture = Gravel;
                return;
            case Block.Gravel:
                curBlock = Block.Leaf;
                BlockPic.texture = Leaf;
                return;
            case Block.Leaf:
                curBlock = Block.Wood;
                BlockPic.texture = Wood;
                return;                        
            case Block.Wood:
                curBlock = Block.Dirt;
                BlockPic.texture = Dirt;
                return;          
            default:
                return;
        }
    }

    //判断Vector3中哪一维是整数
    char CheckInt(Vector3 wPos){
        if ((int)(wPos.x)==wPos.x){
            return 'x';
        }
        else if ((int)(wPos.y)==wPos.y){
            return 'y';
        }
        else if ((int)(wPos.z)==wPos.z)
            return 'z';
        return ' ';
    }

    //摧毁方块（敌人）
    void DestroyBlock()
    {
        //鼠标射线
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //射线
            Debug.DrawLine(ray.origin, hitInfo.point);
            GameObject gameObj = hitInfo.collider.gameObject;

            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider)
            {
                Debug.Log(hitInfo.collider.gameObject.tag);
                if(hitInfo.collider.gameObject.tag=="enemy"){
                    // enemy.SetActive(false);
                    enemy.GetComponent<Enemy>().attacked();
                    Debug.Log("K.O");
                    return;
                }
                Debug.Log("射线检测到的点是" + hitInfo.point);
                Vector3 ChunkPos = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                Chunk chunk = Chunk.GetChunk(true,ChunkPos);     
                Debug.Log("整数的点是："+ CheckInt(hitInfo.point));
                Vector3 BlockPos = new Vector3();
                if(CheckInt(hitInfo.point)=='x'){
                    if(transform.position.x>hitInfo.point.x){
                        BlockPos = new Vector3(hitInfo.point.x-1,Mathf.Floor(hitInfo.point.y),Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }else{
                        BlockPos = new Vector3(hitInfo.point.x,Mathf.Floor(hitInfo.point.y),Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }
                }
                if(CheckInt(hitInfo.point)=='y'){
                    if(transform.position.y>hitInfo.point.y){
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),hitInfo.point.y-1,Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }else{
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),hitInfo.point.y,Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }
                }
                if(CheckInt(hitInfo.point)=='z'){
                    if(transform.position.z>hitInfo.point.z){
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),Mathf.Floor(hitInfo.point.y),hitInfo.point.z-1) - chunk.transform.position; 
                    }else{
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),Mathf.Floor(hitInfo.point.y),hitInfo.point.z) - chunk.transform.position; 
                    }
                }
                if(CheckInt(hitInfo.point)!=' '){
                    Debug.Log(BlockPos);
                    if(BlockPos.x<0 || BlockPos.x>=30 || BlockPos.z<0 || BlockPos.z>=30){
                        Debug.Log("Block error: "+BlockPos);
                        BlockPos += chunk.transform.position;
                        chunk = Chunk.GetChunk(true,BlockPos);
                        BlockPos = BlockPos - chunk.transform.position;
                    }
                    Debug.Log("blockPos+ "+(int)BlockPos.x+ ' '+ (int)BlockPos.y+ ' '+ (int)BlockPos.z);
                    chunk.map[(int)BlockPos.x,(int)BlockPos.y,(int)BlockPos.z] = Block.None;
                    chunk.BuildChunk();
                }
            }            
        }
    }

    //放置方块
    void AddBlock(){
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            //射线
            Debug.DrawLine(ray.origin, hitInfo.point);
            GameObject gameObj = hitInfo.collider.gameObject;

            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider)
            {
                Debug.Log("射线检测到的点是" + hitInfo.point);
                Vector3 ChunkPos = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                Chunk chunk = Chunk.GetChunk(true,ChunkPos);     
                Debug.Log("整数的点是："+ CheckInt(hitInfo.point));
                Vector3 BlockPos = new Vector3();

                if(CheckInt(hitInfo.point)=='x'){
                    if(transform.position.x>hitInfo.point.x){
                        BlockPos = new Vector3(hitInfo.point.x,Mathf.Floor(hitInfo.point.y),Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }else{
                        BlockPos = new Vector3(hitInfo.point.x-1,Mathf.Floor(hitInfo.point.y),Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }
                }
                if(CheckInt(hitInfo.point)=='y'){
                    if(transform.position.y>hitInfo.point.y){
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),hitInfo.point.y,Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }else{
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),hitInfo.point.y-1,Mathf.Floor(hitInfo.point.z)) - chunk.transform.position; 
                    }
                }
                if(CheckInt(hitInfo.point)=='z'){
                    if(transform.position.z>hitInfo.point.z){
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),Mathf.Floor(hitInfo.point.y),hitInfo.point.z) - chunk.transform.position; 
                    }else{
                        BlockPos = new Vector3(Mathf.Floor(hitInfo.point.x),Mathf.Floor(hitInfo.point.y),hitInfo.point.z-1) - chunk.transform.position; 
                    }
                }

                if(CheckInt(hitInfo.point)!=' '){
                    Debug.Log(BlockPos);
                    if(BlockPos.y>=49){
                        Debug.Log("超出高度上限");
                        return;
                    }
                    if(BlockPos.x<0 || BlockPos.x>=30 || BlockPos.z<0 || BlockPos.z>=30){
                        Debug.Log("Block error: "+BlockPos);
                        BlockPos += chunk.transform.position;
                        chunk = Chunk.GetChunk(true,BlockPos);
                        BlockPos = BlockPos - chunk.transform.position;
                    }
                    Debug.Log(chunk);

                    chunk.map[(int)BlockPos.x,(int)BlockPos.y,(int)BlockPos.z] = curBlock;
                    chunk.BuildChunk();
                }
            }
        }
    }
}
