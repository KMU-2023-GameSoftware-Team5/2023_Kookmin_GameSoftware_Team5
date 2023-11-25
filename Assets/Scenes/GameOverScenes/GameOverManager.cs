using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace deck
{
    public class GameOverManager : MonoBehaviour
    {

        [Header("Event")]
        public UnityEvent<int> playerScoreEvent;

        [Header("ScoreBoard")]
        public TextMeshProUGUI winRateTxt;
        /// <summary>
        /// 최다 연승 보여주기
        /// </summary>
        public TextMeshProUGUI playerScoreTxt;

        private void Awake()
        {
            playerScoreEvent.AddListener(test);
        }

        private void Start()
        {
            // regame하기 직전에 이벤트발생
            playerScoreEvent.Invoke(PlayerManager.Instance().playerScore);
            PlayerManager pm = PlayerManager.Instance();
            float winRate = ((float)(pm.playerBattleCount-pm.playerLoseCount) / pm.playerBattleCount);
            winRate *= 100;
            winRateTxt.text = $"{pm.playerBattleCount}전 {pm.playerBattleCount - pm.playerLoseCount}승 {pm.playerLoseCount}패 ({winRate.ToString("F1")}%)";
            playerScoreTxt.text = $"{pm.playerScore}점";
        }

        public void regame()
        {
            resetPlayerManager();
            SceneManager.LoadScene("Scenes/NewGameScenes/NewGameScene");
        }

        public void resetPlayerManager()
        {
            PlayerManager.delete();
        }

        public void test(int a)
        {
            Debug.Log(a);
        }
    }
}
