using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace deck
{
    public class GameOverManager : MonoBehaviour
    {
        public void regame()
        {
            resetPlayerManager();
            SceneManager.LoadScene("Scenes/NewGameScenes/NewGameScene");
        }

        public void resetPlayerManager()
        {
            PlayerManager.delete();
        }
    }
}
