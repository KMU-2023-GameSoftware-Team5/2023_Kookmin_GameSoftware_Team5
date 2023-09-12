using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneEvent : MonoBehaviour
{
    // 메인씬의 간단한 버튼 이벤트 조작
    public GameObject itemPannel;
    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        itemPannel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickItem()
    {
        itemPannel.SetActive(true);
    }

    public void OnClickItemClose() { 
        itemPannel.SetActive(false); 
    }
}
