using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrin : MonoBehaviour
{
    /// <summary>
    /// 用以柏林噪声采样的X和Z值（柏林噪声返回的是Y值）
    /// </summary>
    private float seedX, seedZ;

    /// <summary>
    /// 地图的宽度（X轴方向）
    /// </summary>
    [SerializeField] private int width = 50;

    /// <summary>
    /// 地图的深度（Z轴方向）
    /// </summary>
    [SerializeField] private int depth = 50;

    /// <summary>
    /// 地图的最大高度
    /// </summary>
    [SerializeField] private int maxHeight = 10;

    /// <summary>
    /// 决定了采样间隔 值越大 采样间隔越小
    /// </summary>
    [SerializeField] private float relief = 15.0f;

    private void Awake()
    {
        seedX = Random.value * 100f;
        seedZ = Random.value * 100f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localPosition = new Vector3(x, 0, z);
                cube.transform.SetParent(transform);

                SetY(cube);
            }
        }
    }
    private void SetY(GameObject cube)
    {
        float y = 0;

        float xSample = (cube.transform.localPosition.x + seedX) / relief;
        float zSample = (cube.transform.localPosition.z + seedZ) / relief;
        float noise = Mathf.PerlinNoise(xSample, zSample);
        y = maxHeight * noise;

        y = Mathf.Round(y);

        cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, 
                                                    y, 
                                                    cube.transform.localPosition.z);

        Color color = Color.black;
        if (y > maxHeight * 0.3f)
        {
            ColorUtility.TryParseHtmlString("#019540FF", out color);
        }
        else if (y > maxHeight * 0.2f)
        {
            ColorUtility.TryParseHtmlString("#2432ADFF", out color);
        }
        else if (y > maxHeight * 0.1f)
        {
            ColorUtility.TryParseHtmlString("#D4500EFF", out color);
        }
        cube.GetComponent<MeshRenderer>().material.color = color;
    }
}
