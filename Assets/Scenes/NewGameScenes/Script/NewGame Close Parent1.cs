using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewGameCloseParent : MonoBehaviour
{
    public GameObject button;
    public GameObject ServerRequest;
    // Start is called before the first frame update
    void Start()
    {
        ServerRequest = GameObject.Find("ServerRequest");
        Debug.Log("NewGameScene - EventManager");
        Debug.Log(ServerRequest.GetComponent<ServerRequest>().id);
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
        else {
            obj.SetActive(true);
            TMPro.TMP_Text text = obj.transform.Find("ModalView").transform.Find("Content").transform.Find("PlayerData").transform.Find("Id").GetComponent<TMPro.TMP_Text>();
            text.text = ServerRequest.GetComponent<ServerRequest>().id;
        }
    }

    public void WikiUI(){
        GameObject obj = GameObject.Find("LoadCanvas").transform.Find("WikiUI").gameObject;
        if(obj.activeSelf == true) obj.SetActive(false);
        else obj.SetActive(true);
    }
}
