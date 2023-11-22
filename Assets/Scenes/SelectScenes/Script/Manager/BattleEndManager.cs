using battle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace deck
{
    /// <summary>
    /// 전투 종료를 처리하는 객체
    /// </summary>
    public class BattleEndManager : MonoBehaviour
    {
        public GameObject battleEndPannel;
        public GameObject winPannel;
        public GameObject lossPannel;
        public TextMeshProUGUI buttonText;
        public Transform battleEndCanvas;

        [Header("LossPannel")]
        public Transform altarContent;
        public GameObject altarPrefab;
        public Transform sacrificeContent;
        public GameObject sacrificePrefab;


        bool isWin = false;
        int sacrificeCount = 3;
        int nowsacrificeCount = 0;

        List<AltarSlot> altarSlots = new List<AltarSlot>();


        public void battleEndEventListener()
        {
            Debug.Log("what?");
            battleEndPannel.SetActive(true);
            winPannel.SetActive(false);
            lossPannel.SetActive(false);

            isWin = SceneParamter.Instance().isWin;
            if (isWin)
            {
                openWinPannel();
            }
            else
            {
                openLossPannel();
            }
        }

        public void openWinPannel()
        {
            winPannel.SetActive(true);
        }

        public void openLossPannel()
        {
            lossPannel.SetActive(true);
            buttonText.text = "제물을 바치고\n맵으로 돌아가기";
            // 여기서 sacrificeCount 설정

            if(sacrificeCount > PlayerManager.Instance().playerCharacters.Count)
            {
                sacrificeCount = PlayerManager.Instance().playerCharacters.Count;
            }

            // 패배 하면 캐릭터 바치기
            for (int i = 1; i <= sacrificeCount; i++) // TODO 맵 진행사항에 맞게 캐릭터 바치게하기
            {
                GameObject go = Instantiate(altarPrefab, altarContent);
                go = go.transform.GetChild(0).gameObject;
                AltarSlot altar = go.GetComponent<AltarSlot>();
                altarSlots.Add(altar);
            }
            List<PixelCharacter> characters =  PlayerManager.Instance().playerCharacters;
            foreach (PixelCharacter character in characters)
            {
                GameObject go = Instantiate(sacrificePrefab, sacrificeContent);
                go = go.transform.GetChild(0).gameObject;
                SacrificeCharacter sacrifice = go.GetComponent<SacrificeCharacter>();
                sacrifice.Initialize(character, this);
            }
        }

        public int sacrificeCharacter(PixelCharacter character)
        {
            if(nowsacrificeCount + 1 > sacrificeCount)
            {
                return -1;
            }
            else
            {
                altarSlots[nowsacrificeCount].select(character);
                nowsacrificeCount += 1;
                return nowsacrificeCount-1;
            }
        }

        public void unSacrificeCharacter(int selectNum)
        {
            altarSlots[selectNum].unSelect();
            nowsacrificeCount -= 1;
        }

        public void returnMap()
        {
            if (!isWin)
            {
                if(nowsacrificeCount < sacrificeCount)
                {
                    MyDeckFactory.Instance().displayInfoMessage("정해진 수만큼 제물을 바쳐야합니다.");
                    return;
                }
                else
                {
                    foreach (AltarSlot altarSlot in altarSlots)
                    {
                        PlayerManager.Instance().removeCharacter(altarSlot.character);
                    }
                    PlayerManager.save();
                }
            }
            if(PlayerManager.Instance().playerCharacters.Count == 0)
            {
                gameOver();
            }
            else
            {
                SceneManager.LoadScene("Scenes/MapScenes/MapScene1");
            }
        }

        public void gameOver()
        {
            SceneManager.LoadScene("Scenes/GameOverScenes/GameOverScene");
        }
    }
}
