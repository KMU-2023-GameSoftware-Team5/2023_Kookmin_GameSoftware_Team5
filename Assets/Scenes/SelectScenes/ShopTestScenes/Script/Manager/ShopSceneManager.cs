using System.Collections;
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
            playerGoldText.text = $"{PlayerManager.Instance().playerGold}";
            initialize();
        }

        int skipFrame = 1;
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
            makeShopGoods();
            loadPlayerCharacters();
        }

        public void onClickReroll()
        {
            if (PlayerManager.Instance().useGold(rerollPrice))
            {
                makeShopGoods();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void makeShopGoods()
        {
            // 더미데이터 생성 - TODO : 맵 진행상황을 참고하여 플레이어 상황에 맞게 생성하기
            string[] characterNames = { "Demon", "Skeleton", "Goblin Archor" };
            System.Random random = new System.Random();
            foreach (PixelCharacterGoods characterGood in pixelCharacterGoods)
            {
                PixelCharacter pc = MyDeckFactory.Instance().buildPixelCharacter(characterNames[random.Next(0, characterNames.Length)]);
                characterGood.initialize(pc, random.Next(10, 50));
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
            SceneManager.LoadScene("Scenes/CombineScenes/CombineScene");
            // SceneManager.LoadScene("Scenes/MapScenes/MapScene1");

        }

    }
}
