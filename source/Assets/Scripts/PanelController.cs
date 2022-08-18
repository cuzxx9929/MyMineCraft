using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制菜单弹出（游戏暂停）和隐藏（游戏继续）
public class PanelController : MonoBehaviour
{
    public static bool PanelShow;
    public GameObject Menu;
    public GameObject Mouse;
    public GameObject opinfo;
    public GameObject ToStoreVerify;
    public GameObject Store;

    // Start is called before the first frame update
    void Start()
    {
        PanelShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("`")){
            PanelShowChange();
        }
    }
    
    void PanelShowChange(){
        PanelShow = !PanelShow;
        Menu.SetActive(PanelShow);
        opinfo.SetActive(false);
        ToStoreVerify.SetActive(false);
        Store.SetActive(false);
        Mouse.SetActive(!PanelShow);

        Cursor.visible = PanelShow;
        if(PanelShow){
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
