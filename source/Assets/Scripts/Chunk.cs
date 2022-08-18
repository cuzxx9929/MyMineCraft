using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(MeshFilter))]

//Chunk地形
public class Chunk : MonoBehaviour
{
    public static Chunk chunkPrefab;
    public static int ChunkNum = 0;
    static Chunk chunk;
    public static bool loadFinish = true;

    //当前生成的所有Chunk的列表
    public static List<Chunk> chunks = new List<Chunk>();
    //Chunk的高度
    public static int width = 30;
    //Chunk的宽度
    public static int height = 50;
    //随机数种子
    public int seed;
    public float baseHeight = 10;
    public float frequency = 0.025f;
    public float amplitude = 1;

    //当前Chunk下的所有Block 的类型
    public Block[,,] map;
    Mesh chunkMesh;
	MeshRenderer meshRenderer;
	MeshCollider meshCollider;
	MeshFilter meshFilter;

    Vector3 offset0;
    Vector3 offset1;
    Vector3 offset2;
    System.Random myRandom = new System.Random();

    //用于读取存档时把存档中的Chunk先进行读取和添加
    public static void SetChunks(){
        chunks= new List<Chunk>();
        chunkPrefab = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerMove>().chunkPrefab;
        Vector3 chunkPos;
        int basis;
        int p;
        for(ChunkNum=0;ChunkNum<GameSave.file.chunksPos.Count;ChunkNum++){
            chunkPos = new Vector3((int)GameSave.file.chunksPos[ChunkNum].x,(int)GameSave.file.chunksPos[ChunkNum].y,(int)GameSave.file.chunksPos[ChunkNum].z);
            chunk = (Chunk)Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
            chunk.map = new Block[width, height, width];
            p = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < width; z++)
                    {
                        chunk.map[x, y, z] = LoadBlock(GameSave.file.BlockInfo[ChunkNum][p]);
                        p++;
                    }
                }
            }
        }
    }

    //用于读取存档时根据保存的数字返回Block类型
    public static Block LoadBlock(int blockType){
        switch (blockType)
        {
            case 0:
                return Block.None;
            case 1:
                return Block.Dirt;
            case 2:
                return Block.DirtAndGrass;
            case 3:
                return Block.Grass;
            case 4:
                return Block.Gravel;
            case 5:  
                return Block.Leaf;
            case 6:
                return Block.Wood;
            default:
                return Block.None;
        }
    }

    //根据传入的位置坐标返回位置坐标对应的Chunk
    public static Chunk GetChunk(bool flag,Vector3 PlayerPos)
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            Vector3 tempPos = chunks[i].transform.position;
            if ((PlayerPos.x < tempPos.x) || (PlayerPos.z < tempPos.z) || (PlayerPos.x >= tempPos.x + 30) || (PlayerPos.z >= tempPos.z + 30))
                continue;
            if(flag)
                Debug.Log("Got chunk: "+i+"  Position: "+chunks[i].transform.position);
            return chunks[i];
        }
        return null;
    }


    void Start ()
    {
        Debug.Log(this.transform.position);
        //初始化时将自己加入chunks列表
        chunks.Add(this);
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        //初始化地图
        InitMap();
    }


    void OnDestroy(){
        chunks.Remove(this);
    }

    //初始化地图
    void InitMap()
    {
        if(map==null){
            //初始化随机种子
            map = new Block[width, height, width];
            Random.InitState(seed);
            offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            
            //初始化Map
            int treeFlag;
            //遍历map，生成其中每个Block的信息
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < width; z++)
                    {
                        if(GameSave.LoadGame && chunks.Count==1){
                            map[x,y,z] =Block.None;
                        }
                        else{
                            //不覆盖掉已经生成的木头或树叶
                            if(map[x,y,z]==Block.Wood || map[x,y,z]==Block.Leaf)
                                continue;
                            
                            //根据噪声生成block类型
                            map[x, y, z] = GenerateBlock(new Vector3(x, y, z) + transform.position);

                            //随机生成树木
                            if(map[x,y,z]==Block.Grass&& x>=1 && x<29 && z>=1 && z<29){
                                treeFlag = myRandom.Next(1,100);
                                if(treeFlag==1){
                                    treeFlag = myRandom.Next(5,10);
                                    int h;
                                    for(h =y+1;h<y+treeFlag;h++)
                                        map[x,h,z] = Block.Wood;
                                    for(int x1=-1;x1<=1;x1++)
                                        for(int y1=-1;y1<2;y1++)
                                            for(int z1=-1;z1<=1;z1++){
                                                if(y1<0 && x1==0 && z1==0){
                                                    continue;
                                                }
                                                map[x+x1,h+y1,z+z1] = Block.Leaf;
                                            }                                    
                                }

                            }
                        }
                    }
                }
            }
        }
        BuildChunk();
    }

    //根据噪声产生高度
    int GenerateHeight(Vector3 wPos)
    {

        //让随机种子，振幅，频率，应用于我们的噪音采样结果
        float x0 = (wPos.x + offset0.x) * frequency;
        float y0 = (wPos.y + offset0.y) * frequency;
        float z0 = (wPos.z + offset0.z) * frequency;

        float x1 = (wPos.x + offset1.x) * frequency * 2;
        float y1 = (wPos.y + offset1.y) * frequency * 2;
        float z1 = (wPos.z + offset1.z) * frequency * 2;

        float x2 = (wPos.x + offset2.x) * frequency / 4;
        float y2 = (wPos.y + offset2.y) * frequency / 4;
        float z2 = (wPos.z + offset2.z) * frequency / 4;

        float noise0 = Noise.Generate(x0, y0, z0) * amplitude;
        float noise1 = Noise.Generate(x1, y1, z1) * amplitude / 2;
        float noise2 = Noise.Generate(x2, y2, z2) * amplitude / 4;

        //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + baseHeight);
    }

    //根据随机生成的高度确定Block的类型
    Block GenerateBlock(Vector3 wPos)
    {
        //y坐标是否在Chunk内
        if (wPos.y >= height)
        {
            return Block.None;
        }

        //获取当前位置方块随机生成的高度值
        float genHeight = GenerateHeight(wPos);

        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (wPos.y > genHeight)
        {
            return Block.None;
        }
        //当前方块位置等于随机生成的高度值时，当前方块类型为草地
        else if (wPos.y == genHeight)
        {
            return Block.Grass;
        }
        //当前方块位置小于随机生成的高度值 且 大于 genHeight - 5时，当前方块类型为泥土
        else if (wPos.y < genHeight && wPos.y > genHeight - 5)
        {
            return Block.Dirt;
        }
        //其他情况，当前方块类型为碎石
        return Block.Gravel;
    }

    //构建Chunk
	public void BuildChunk()
    {
		chunkMesh = new Mesh();
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		
        //遍历chunk, 生成其中的每一个Block
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
                    BuildBlock(x, y, z, verts, uvs, tris);
                }
			}
		}
					
		chunkMesh.vertices = verts.ToArray();
		chunkMesh.uv = uvs.ToArray();
		chunkMesh.triangles = tris.ToArray();
		chunkMesh.RecalculateBounds();
		chunkMesh.RecalculateNormals();
		
		meshFilter.mesh = chunkMesh;
		meshCollider.sharedMesh = chunkMesh;
	}

    //面渲染绘制Block可见的面
    void BuildBlock(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        if (map[x, y, z] == 0) return;

        Block typeid = map[x, y, z];

        //Left
        if (CheckNeedBuildFace(x - 1, y, z))
            BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
        //Right
        if (CheckNeedBuildFace(x + 1, y, z))
            BuildFace(typeid, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);

        //Bottom
        if (CheckNeedBuildFace(x, y - 1, z))
            BuildFace(typeid, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
        //Top
        if (CheckNeedBuildFace(x, y + 1, z))
            BuildFace(typeid, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);

        //Back
        if (CheckNeedBuildFace(x, y, z - 1))
            BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
        //Front
        if (CheckNeedBuildFace(x, y, z + 1))
            BuildFace(typeid, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
    }

    //判断面可不可见
    bool CheckNeedBuildFace(int x, int y, int z)
    {
        // return true;
        if (y < 0) return false;
        if (x<0||z<0||x>=width||z>=width)
            return true;
        var type = GetBlock(x, y, z);
        if(type == Block.None)
            return true;
        else 
            return false;
    }

    //获取Block
    public Block GetBlock(int x, int y, int z)
    {
        if (y < 0 || y > height - 1)
        {
            return 0;
        }

        //当前位置是否在Chunk内
        if ((x < 0) || (z < 0) || (x >= width) || (z >= width))
        {
            var id = GenerateBlock(new Vector3(x, y, z) + transform.position);
            return id;
        }
        return map[x, y, z];
    }

    //渲染Block的面
    void BuildFace(Block typeid, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
	{
        int index = verts.Count;
		
		verts.Add (corner);
		verts.Add (corner + up);
		verts.Add (corner + up + right);
		verts.Add (corner + right);

		Vector2 uvWidth = new Vector2((float)(1)/6-(float)0.01, .99f);
		Vector2 uvCorner = new Vector2(0.00f, 0.00f);

        uvCorner.x += (float)(typeid - 1) / 6 + (float)0.01;
        uvs.Add(uvCorner);
		uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
		uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
		uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));
		
		if (reversed)
		{
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}
		else
		{
            tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 3);
			tris.Add(index + 2);
		}
	}


}


