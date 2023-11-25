using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameCloseParent : MonoBehaviour
{
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MenuUI(){
        GameObject obj = GameObject.Find("LoadCanvas").transform.Find("MenuUI").gameObject;
        if(obj.activeSelf == true) obj.SetActive(false);
        else obj.SetActive(true);
    }

    public void SettingUI(){
        GameObject obj = GameObject.Find("LoadCanvas").transform.Find("SettingUI").gameObject;
        if(obj.activeSelf == true) obj.SetActive(false);
        else obj.SetActive(true);
    }

    public void WikiUI(){
        GameObject obj = GameObject.Find("LoadCanvas").transform.Find("WikiUI").gameObject;
        if(obj.activeSelf == true) obj.SetActive(false);
        else obj.SetActive(true);
    }
}
