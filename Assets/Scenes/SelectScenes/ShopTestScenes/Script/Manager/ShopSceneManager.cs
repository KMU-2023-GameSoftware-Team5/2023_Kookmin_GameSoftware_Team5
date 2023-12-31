using battle;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace deck
{
    public class ShopSceneManager : MonoBehaviour
    {
        private static ShopSceneManager instance;
        public static ShopSceneManager Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShopSceneManager>();
            }
            return instance;
        }

        public int rerollPrice = 50;

        [SerializeField] TextMeshProUGUI playerGoldText;

        
        private void Start()
        {
            stageNum = PlayerManager.Instance().StageCount;
            stageNum = PlayerManager.Instance().stageCount += 1;
            initialize();
            loadPlayerCharacters();
        }

        private void Update()
        {
            /*
            if (skipFrame >= 0)
            {
                if(skipFrame == 0) initialize();
                skipFrame--;
            }*/
        }

        [SerializeField] List<PixelCharacterGoods> pixelCharacterGoods;

        /// <summary>
        /// 초기화 함수
        /// </summary>
        void initialize()
        {
            playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
            shopParameterSetting();
            makeShopGoods();
            displayCurrentUpgrade();
        }

        /// <summary>
        /// 상점에서 캐릭터의 티어 출현 확률
        /// </summary>
        int[] shopParameter;
        /// <summary>
        /// TODO - 현재 게임 진행을 반영하는 파라미터
        /// </summary>
        public int stageNum;

        [SerializeField] TierProbabilitiesData tiers;

        [Header("upgrade")]
        public TextMeshProUGUI selectableText;
        public TextMeshProUGUI inventoryText;
        public int selectPrice=15;
        public int inventoryPrice=20;
        public TextMeshProUGUI selectablePriceText;
        public TextMeshProUGUI inventoryPriceText;
        public TextMeshProUGUI playerMaxCharacterText;

        public void displayPlayersCharacter()
        {
            playerMaxCharacterText.text = $"{PlayerManager.Instance().playerCharacters.Count} / {PlayerManager.Instance().max_character}";
        }

        public void displayCurrentUpgrade()
        {
            selectPrice = PlayerManager.Instance().max_selectable;
            selectablePriceText.text = selectPrice.ToString();
            inventoryPrice = PlayerManager.Instance().max_character;
            inventoryPriceText.text = inventoryPrice.ToString();
            selectableText.text = PlayerManager.Instance().max_selectable.ToString();
            inventoryText.text = PlayerManager.Instance().max_character.ToString();
            displayPlayersCharacter();
        }

        public void upGradeSelectAble()
        {
            if (PlayerManager.Instance().useGold(selectPrice))
            {
                PlayerManager.Instance().max_selectable++;
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
                displayCurrentUpgrade();
            }
            else
            {
                MyDeckFactory.Instance().displayInfoMessage("잔액이 부족합니다.");
            }
        }

        public void upGradeInventory()
        {
            if (PlayerManager.Instance().useGold(inventoryPrice))
            {
                PlayerManager.Instance().max_character++;
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
                displayCurrentUpgrade();
            }
            else
            {
                MyDeckFactory.Instance().displayInfoMessage("잔액이 부족합니다.");
            }
        }

        public void shopParameterSetting()
        {
            int idx = 0;
            while (stageNum > tiers.tierProbabilities[idx].stage)
            {
                idx++;
                if(tiers.tierProbabilities.Length <= idx)
                {
                    idx--;
                    break;
                }
            }
            shopParameter = new int[6];
            shopParameter[1] = tiers.tierProbabilities[idx].tier1;
            shopParameter[2] = tiers.tierProbabilities[idx].tier2;
            shopParameter[3] = tiers.tierProbabilities[idx].tier3;
            shopParameter[4] = tiers.tierProbabilities[idx].tier4;
            shopParameter[5] = tiers.tierProbabilities[idx].tier5;
            shopParameter[0] = shopParameter[1] + shopParameter[2] + shopParameter[3] + shopParameter[4] + shopParameter[5];

            for(int tier=0;tier<5;tier++)
            {
                probText[tier].color = MyDeckFactory.Instance().tierColors[tier];
                float rate = (float) shopParameter[tier+1] / shopParameter[0] * 100;
                probText[tier].text = string.Format("+{0} : {1:F1}%", tier + 1, rate);
            }
        }

        public void onClickReroll()
        {
            if (PlayerManager.Instance().useGold(rerollPrice))
            {
                makeShopGoods();
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
            }
            else
            {
                MyDeckFactory.Instance().displayInfoMessage($"돈이 없습니다.\n리롤비용은 {rerollPrice}코인입니다.");
            }
        }

        public int makeTier()
        {
            // shop parameter 기반으로 생성할 캐릭터의 강화값을 산출
            int tier = 0;
            int rand = UnityEngine.Random.Range(0, shopParameter[0]);
            for (int i = 1; i <= 5; i++)
            {
                if (rand <= shopParameter[i])
                {
                    tier = i;
                    break;
                }
                else
                {
                    rand -= shopParameter[i];
                }
            }
            return tier;
        }

        /// <summary>
        /// 판매할 캐릭터 생성
        /// </summary>
        public void makeShopGoods()
        {
            
            // price값을 기반으로 판매할 캐릭터 생성
            foreach (PixelCharacterGoods characterGood in pixelCharacterGoods)
            {
                int tier = makeTier();
                PixelCharacter pc = MyDeckFactory.Instance().buildCharcterByPrice(tier);
                characterGood.initialize(pc, tier);
            }

        }


        /// <summary>
        /// 캐릭터 구매
        /// </summary>
        /// <param name="character"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool buyCharacter(PixelCharacter character, int price)
        {
            if (PlayerManager.Instance().max_character <= PlayerManager.Instance().playerCharacters.Count)
            {
                MyDeckFactory.Instance().displayInfoMessage("보유 가능 캐릭터 수보다\n더 많은 캐릭터를 구매할 수 없습니다.");
                return false;
            }

            bool ret = PlayerManager.Instance().buyCharacter(character, price);
            if (ret)
            {
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
                displayPlayersCharacter();
                loadPlayerCharacters();
                return ret;
            }
            else
            {
                MyDeckFactory.Instance().displayInfoMessage("돈이 없습니다");
                return ret;
            }
        }

        /// <summary>
        /// 플레이어 보유 캐릭터 판매 메서드
        /// </summary>
        /// <param name="character"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool sellCharacter(PixelCharacter character, int price)
        {
            bool ret = PlayerManager.Instance().sellCharacter(character, price);
            if (ret)
            {
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
                displayPlayersCharacter();
                loadPlayerCharacters();
                return ret;
            }
            else
            {
                MyDeckFactory.Instance().displayInfoMessage("마지막 캐릭터는 판매할 수 없습니다.");
                return ret;
            }

        }

        [Header("preset")]
        [SerializeField] GameObject playerCharacterPreset;
        [SerializeField] Transform playerCharacterGrid;
        [SerializeField] Transform dragCanvas;
        List<PlayerCharacterGoods> nowPlayerCharacterUI;
        /// <summary>
        /// 
        /// </summary>
        public TextMeshProUGUI[] probText;
        public void loadPlayerCharacters()
        {
            if (nowPlayerCharacterUI != null)
            {
                for(int i = nowPlayerCharacterUI.Count-1; i >= 0; i--)
                {
                    nowPlayerCharacterUI[i].destroy();
                    nowPlayerCharacterUI.RemoveAt(i);
                }
            }
            nowPlayerCharacterUI = new List<PlayerCharacterGoods>();
            List<PixelCharacter> characters = PlayerManager.Instance().sortingCharacter();
            foreach (PixelCharacter character in characters)
            {
                
                GameObject go = Instantiate(playerCharacterPreset, playerCharacterGrid);
                PlayerCharacterGoods tmp = go.transform.GetChild(0).gameObject.GetComponent<PlayerCharacterGoods>();
                tmp.Initialize(character, 100, dragCanvas);
                nowPlayerCharacterUI.Add(tmp);
            }
        }

        public void onClickGameStart()
        {
            PlayerManager.save();
            // SceneManager.LoadScene("Scenes/CombineScenes/CombineScene");
            SceneManager.LoadScene("Scenes/MapScenes/MapScene1");
            // SceneManager.LoadScene("Scenes/Parameter Test Scene/Parameter Test");
        }
    }
}
