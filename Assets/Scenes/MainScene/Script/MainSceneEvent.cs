using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneEvent : MonoBehaviour
{
    // ���ξ��� ������ ��ư �̺�Ʈ ����
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
