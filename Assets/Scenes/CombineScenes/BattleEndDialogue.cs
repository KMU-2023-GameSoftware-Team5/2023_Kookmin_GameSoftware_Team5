using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEndDialogue : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public int mapSceneIndex;

    public void Initialize()
    {
        if (SceneParamter.Instance().isWin)
        {
            resultText.text = "승리했습니다.";
        }
        else
        {
            resultText.text = "패배했습니다.";
        }
    }

    public void GoToMapScene()
    {
        SceneManager.LoadScene(mapSceneIndex);
    }
}
