using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ServerRequest : MonoBehaviour
{
    public static ServerRequest Instance;
    public string id;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(UnityWebRequestPOSTTEST());

        //샘플
        /*StartCoroutine(GetRank((jarray)=>{
            Debug.Log(jarray.ToString());
        }));*/
        SetId();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score){
        StartCoroutine(SetScore(this.id,score));
    }

    public void SetId(){
        StartCoroutine(NewUser((id)=>{
            this.id = id;
            Debug.Log(this.id);
        }));
    }

    IEnumerator UnityWebRequestPOSTTEST()
    {
        string url = "http://13.125.165.205/";
        WWWForm form = new WWWForm();
        string data = "test";
        //string pw = "비밀번호";
        form.AddField("data", data);
        //form.AddField("Password", pw);
        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
        }
        else
        {
            Debug.Log("error");
        }
    }

    //사용자 관리번호 발급(id값)
    public IEnumerator NewUser(System.Action<string> callback)
    {
        string url = "http://13.125.165.205/process/userdata/checkuser";
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
            JObject obj = JObject.Parse(www.downloadHandler.text);
            Debug.Log(obj["data"]["id"]);
            callback(obj["data"]["id"].ToString());
        }
        else
        {
            Debug.Log("error");
        }
    }

    //점수 등록
    public IEnumerator SetScore(string id,int score)
    {
        string url = "http://13.125.165.205/process/userdata/setscore";
        WWWForm form = new WWWForm();

        form.AddField("id", id);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
        }
        else
        {
            Debug.Log("error");
        }
    }

    //점수 확인
    public IEnumerator GetScore(string id,System.Action<string> callback)
    {
        string url = "http://13.125.165.205/process/userdata/getscore";
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
            JObject obj = JObject.Parse(www.downloadHandler.text);
            Debug.Log(obj["data"]["score"]);
            callback(obj["data"]["score"].ToString());
        }
        else
        {
            Debug.Log("error");
        }
    }

    //점수 확인
    public IEnumerator GetRank(System.Action<JArray> callback)
    {
        string url = "http://13.125.165.205/process/userdata/getrank";
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
            JObject obj = JObject.Parse(www.downloadHandler.text);
            JArray data = JArray.Parse(obj["data"].ToString());
            callback(data);
        }
        else
        {
            Debug.Log("error");
        }
    }

    //서버 저장
    public IEnumerator SaveData(string id,string save)
    {
        string url = "http://13.125.165.205/process/userdata/savedata";
        WWWForm form = new WWWForm();

        form.AddField("id", id);
        form.AddField("data", save);

        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
        }
        else
        {
            Debug.Log("error");
        }
    }

    //서버 불러오기
    public IEnumerator LoadData(string id,System.Action<string> callback)
    {
        string url = "http://13.125.165.205/process/userdata/getdata";
        WWWForm form = new WWWForm();

        form.AddField("id", id);

        UnityWebRequest www = UnityWebRequest.Post(url, form);  // 보낼 주소와 데이터 입력

        www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);

        yield return null;  // 응답 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
            JObject obj = JObject.Parse(www.downloadHandler.text);
            callback(obj["data"]["data"].ToString());
        }
        else
        {
            Debug.Log("error");
        }
    }
}
