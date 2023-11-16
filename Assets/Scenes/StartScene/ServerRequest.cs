using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UnityWebRequestPOSTTEST());
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
