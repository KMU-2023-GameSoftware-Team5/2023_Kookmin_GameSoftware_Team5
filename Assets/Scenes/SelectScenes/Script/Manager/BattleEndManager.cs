using battle;
using System;
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
        public TextMeshProUGUI buttonText;
        public Transform battleEndCanvas;

        [Header("WinPannel")]
        public GameObject winPannel;
        public TextMeshProUGUI playerCoinText;
        public TextMeshProUGUI rewardCoinText;
        public TextMeshProUGUI rewardReasonText;
        public TextMeshProUGUI winTitle;
        public EquipItemDetails equipItemInfo;

        [Header("LossPannel")]
        public GameObject lossPannel;
        public Transform altarContent;
        public GameObject altarPrefab;
        public Transform sacrificeContent;
        public GameObject sacrificePrefab;


        bool isWin = false;
        int sacrificeCount = 3;
        int nowsacrificeCount = 0;

        List<AltarSlot> altarSlots;
        List<SacrificeCharacter> sacrificeCharacters;

        private void Start()
        {
            //battleEndEventListener();
        }

        public void battleEndEventListener()
        {
            battleEndPannel.SetActive(true);
            winPannel.SetActive(false);
            lossPannel.SetActive(false);

            isWin = SceneParamter.Instance().isWin;
            PlayerManager.Instance().playerBattleCount += 1;

                //isWin = true;
                if (isWin)
            {
                openWinPannel();
            }
            else
            {
                openLossPannel();
            }
        }

        /// <summary>
        /// 승리시 패널
        /// </summary>
        public void openWinPannel()
        {
            // 연승 계산 
            int winCount = PlayerManager.Instance().playerWinCount;
            if (winCount < 0)
            {
                winCount = 1;
            }
            else
            {
                winCount++;
            }
            PlayerManager.Instance().playerWinCount = winCount;
            if(winCount > 1)
            {
                winTitle.text = $"승리했습니다 ({winCount}연승)";
            }
            int rewardCoin;
            // 기본 승리 보상
            rewardCoin = 3;
            string rewardReason = $"승리 보상 +{rewardCoin}\n";
            // 연승 보상 (3연승 당 2원씩 증가
            if (winCount >= 3)
            {
                int tmp = 2; 
                tmp += (winCount / 3) - 1;
                rewardReason += $"연승 보너스 +{tmp}";
                rewardCoin += tmp;
            }


            // (보스?) 승리시 아이템 제공
            EquipItem rewardItem;
            rewardItem = MyDeckFactory.Instance().buildRandomItem();
            PlayerManager.Instance().addEquipItem(rewardItem);

            // UI 적용
            winPannel.SetActive(true);

            PlayerManager.Instance().playerGold += rewardCoin;
            playerCoinText.text = PlayerManager.Instance().playerGold.ToString();
            rewardCoinText.text = $"(+{rewardCoin})";
            rewardReasonText.text = rewardReason;

            equipItemInfo.openItemDetail(rewardItem);

            // 점수 계산 
            PlayerManager.Instance().playerScore += SceneParamter.Instance().Score;
        }

        /// <summary>
        /// 패배시 패널
        /// </summary>
        public void openLossPannel()
        {
            PlayerManager.Instance().playerLoseCount += 1;
            // 연패 계산 
            int winCount = PlayerManager.Instance().playerWinCount;
            if(winCount > 0)
            {
                winCount = -1;
            }
            else
            {
                winCount--;
            }
            PlayerManager.Instance().playerWinCount = winCount;

            lossPannel.SetActive(true);
            buttonText.text = "제물을 바치고\n맵으로 돌아가기";
            // 여기서 sacrificeCount 설정
            sacrificeCount = 1 + PlayerManager.Instance().StageCount / 3;
            if (sacrificeCount > PlayerManager.Instance().playerCharacters.Count)
            {
                sacrificeCount = PlayerManager.Instance().playerCharacters.Count;
            }

            // 패배 하면 캐릭터 바치기
            altarSlots = new List<AltarSlot>();
            sacrificeCharacters = new List<SacrificeCharacter>();

            for (int i = 1; i <= sacrificeCount; i++) 
            {
                GameObject go = Instantiate(altarPrefab, altarContent);
                go = go.transform.GetChild(0).gameObject;
                AltarSlot altar = go.GetComponent<AltarSlot>();
                altar.Intialize(this);
                altarSlots.Add(altar);
            }
            List<PixelCharacter> characters =  PlayerManager.Instance().sortingCharacter();
            for(int i=characters.Count-1;i>=0;i--)
            {
                PixelCharacter character = characters[i]; 
                GameObject go = Instantiate(sacrificePrefab, sacrificeContent);
                go = go.transform.GetChild(0).gameObject;
                SacrificeCharacter sacrifice = go.GetComponent<SacrificeCharacter>();
                sacrifice.Initialize(character, this);
                sacrificeCharacters.Add(sacrifice);
            }
        }

        public bool sacrificeCharacter(PixelCharacter character)
        {
            if(nowsacrificeCount + 1 > sacrificeCount)
            {
                return false;
            }
            else
            {
                foreach(var altar in altarSlots) { 
                    if(altar.character == null)
                    {
                        altar.select(character);
                        break;
                    }
                }
                nowsacrificeCount += 1;
                return true;
            }
        }

        /// <summary>
        /// 바쳐진 제물을 취소하기
        /// </summary>
        /// <param name="character">제물취소할 캐릭터</param>
        public void unSacrificeCharacter(PixelCharacter character)
        {
            foreach(var altar in altarSlots)
            {
                if (altar.character == null)
                    continue;
                if(altar.character.ID == character.ID) { 
                    altar.unSelect();
                    break;
                }
            }
            foreach(var sacrifice in sacrificeCharacters)
            {
                if(sacrifice.character.ID == character.ID)
                {
                    sacrifice.unSelect();
                    break;
                }
            }
            nowsacrificeCount -= 1;
        }

        /// <summary>
        /// 패배시 캐릭터를 죽이고, 승리시 그냥 넘어가는 코드. 
        /// </summary>
        /// <returns></returns>
        public bool killCharacter()
        {
            if (!isWin)
            {
                if (nowsacrificeCount < sacrificeCount)
                {
                    MyDeckFactory.Instance().displayInfoMessage("정해진 수만큼 제물을 바쳐야합니다.");
                    return false; 
                }
                else
                {
                    foreach (AltarSlot altarSlot in altarSlots)
                    {
                        PlayerManager.Instance().removeCharacter(altarSlot.character);
                    }
                    PlayerManager.save();
                    return true;
                }
            }
            return true;
        }

        public void returnMap()
        {
            if (!killCharacter())
            {
                return;
            }
            if(PlayerManager.Instance().playerCharacters.Count == 0)
            {
                gameOver();
            }
            else
            {
                PlayerManager.Instance().stageCount+=1;
                PlayerManager.save();
                SceneManager.LoadScene("Scenes/MapScenes/MapScene1");
            }
        }

        public void gameOver()
        {
            PlayerManager.save();
            SceneManager.LoadScene("Scenes/GameOverScenes/GameOverScene");
        }
    }
}
