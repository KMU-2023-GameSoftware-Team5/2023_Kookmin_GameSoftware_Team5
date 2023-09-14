using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneEvent : MonoBehaviour
{

    private static MainSceneEvent Instance;
    public static MainSceneEvent instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<MainSceneEvent>();
            }
            return Instance;
        }
    }

    // 메인씬의 간단한 버튼 이벤트 조작
    public GameObject itemPannel;
    public GameObject CharacterPannel;

    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        itemPannel.SetActive(false);
        CharacterPannel.SetActive(false);
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

    public void OnClickCharacter()
    {
        CharacterPannel.SetActive(true);
    }

    public void OnClickCharacterClose()
    {
        CharacterPannel.SetActive(false);
    }

}
