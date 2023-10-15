using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParameterTest : MonoBehaviour
{
    public string sceneName;
    public MobSetData testMobSet;

    public void GoToCombineScene()
    {
        SceneParamter.Instance().MobSet = testMobSet;

        SceneManager.LoadScene(sceneName);
    }
}
