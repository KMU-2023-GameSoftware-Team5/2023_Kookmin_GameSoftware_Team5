using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.SceneManagement;
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
            playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
            initialize();
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
        /// TODO - 맵의 진행사항과 연계하여 판매할 캐릭터 및 아이템 수준 결정하기
        /// </summary>
        void initialize()
        {
            shopParameterSetting();
            makeShopGoods();
            loadPlayerCharacters();
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


        public bool buyCharacter(PixelCharacter character, int price)
        {
            bool ret = PlayerManager.Instance().buyCharacter(character, price);
            if (ret)
            {
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
                loadPlayerCharacters();
                return ret;
            }
            else
            {
                return ret;
            }
        }

        public bool sellCharacter(PixelCharacter character, int price)
        {
            bool ret = PlayerManager.Instance().sellCharacter(character, price);
            if (ret)
            {
                playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
                loadPlayerCharacters();
                return ret;
            }
            else
            {
                return ret;
            }

        }

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
            if(nowPlayerCharacterUI != null)
            {
                for(int i = nowPlayerCharacterUI.Count-1; i >= 0; i--)
                {
                    nowPlayerCharacterUI[i].destroy();
                    nowPlayerCharacterUI.RemoveAt(i);
                }
            }
            nowPlayerCharacterUI = new List<PlayerCharacterGoods>();
            List<PixelCharacter> characters = PlayerManager.Instance().playerCharacters;
            foreach(PixelCharacter character in characters)
            {
                
                GameObject go = Instantiate(playerCharacterPreset, playerCharacterGrid);
                PlayerCharacterGoods tmp = go.transform.GetChild(0).gameObject.GetComponent<PlayerCharacterGoods>();
                tmp.Initialize(character, 100, dragCanvas);
                nowPlayerCharacterUI.Add(tmp);
            }
        }

        public void onClickGameStart()
        {
            // SceneManager.LoadScene("Scenes/CombineScenes/CombineScene");
            // SceneManager.LoadScene("Scenes/MapScenes/MapScene1");
            SceneManager.LoadScene("Scenes/Parameter Test Scene/Parameter Test");
        }

    }
}
