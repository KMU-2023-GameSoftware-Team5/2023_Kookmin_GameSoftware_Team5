using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewGameCloseParent : MonoBehaviour
{
    public GameObject button;
    public GameObject ServerRequest;
    public GameObject RankObj;
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

    public void RankUI(){
        GameObject obj = GameObject.Find("LoadCanvas").transform.Find("RankUI").gameObject;
        if(obj.activeSelf == true) obj.SetActive(false);
        else {
            obj.SetActive(true);
            StartCoroutine(ServerRequest.GetComponent<ServerRequest>().GetRank((jarray)=>{
                for(int i=0;i<jarray.Count;i++){
                    GameObject RankRow = Instantiate(RankObj);
                    RankRow.transform.Find("Num").GetComponent<TMPro.TMP_Text>().text = jarray[i]["score"].ToString();
                    RankRow.transform.Find("Text").GetComponent<TMPro.TMP_Text>().text = jarray[i]["name"].ToString();
                    RankRow.transform.SetParent(obj.transform.Find("ModalView").transform.Find("Content").transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform);
                    RankRow.transform.localScale = new Vector3(1f,1f,1f);
                }
            }));
        }
    }
}
