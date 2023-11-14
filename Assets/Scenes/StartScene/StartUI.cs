using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("click");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        //    onClickStart();
    }
    public void onClickStart()
    {
        SceneManager.LoadScene("Scenes/NewGameScenes/NewGameScene");
    }

    public void onClickStart_map()
    {
        SceneManager.LoadScene("MapScene1");
    }

    public void onClickStart_battle()
    {
        SceneManager.LoadScene("Battle Test");
    }
}
