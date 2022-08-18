using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewControl : MonoBehaviour
{
    //第一人称摄像机
    public Camera c1;
    //第三人称摄像机
    public Camera c3;
    //当前状态
    private bool firstView = true;

    // Start is called before the first frame update
    void Start()
    {
        c1.gameObject.SetActive(firstView);
        c3.gameObject.SetActive(!firstView);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            firstView = !firstView;
            c1.gameObject.SetActive(firstView);
            c3.gameObject.SetActive(!firstView);
        }
    }
}
