using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace deck
{
    public class NewGameSceneManager : MonoBehaviour
    {

        PlayerManager playerManager;

        [Header("intial setting")]
        [SerializeField] int playerGold;
        [SerializeField] int playerLife;

        [Header("Canvas")]
        public Canvas newGameCanvas;
        public Canvas loadGameCanvas;

        [Header("current Player Status")]
        [SerializeField] Transform characterGrid;
        [SerializeField] Transform itemGrid;

        [Header("New Game")]
        [SerializeField] NewGameSelectorList newGameSelector;


        private void Start()
        {
            playerManager = PlayerManager.Instance();

            List<PixelCharacter> characters = playerManager.playerCharacters;
            List<EquipItem> items = playerManager.playerEquipItems;

            if (characters.Count == 0)
            {
                newGameCanvas.gameObject.SetActive(true);
                loadGameCanvas.gameObject.SetActive(false);
            }
            else
            {
                newGameCanvas.gameObject.SetActive(false);
                loadGameCanvas.gameObject.SetActive(true);
                // 현재 보유중인 캐릭터 출력
                for (int i = 0; i < characters.Count; i++)
                {
                    // MyDeckFactory.Instance().createCharacterInventoryPrefab(characters[i], characterGrid);
                    // MyDeckFactory.Instance().createCharacterCardPrefab(characters[i], characterGrid);
                    MyDeckFactory.Instance().createLightCharacterInfo(characters[i], characterGrid);
                }
            }
                /*
                }
                // 플레이어 보유 아이템에 대한 UI 생성
                foreach (EquipItem item in items)
                {
                    MyDeckFactory.Instance().createLightEquipItemUI(item, itemGrid);
                }
                */
                newGameSelector.Initialize(this);
        } 

        /// <summary>
        /// 새 게임 시작하기 
        /// </summary>
        /// <param name="characters">새로 시작할 캐릭터 세트</param>
        public void newGameStart(List<PixelCharacter> characters)
        {
            foreach(PixelCharacter character in characters)
            {
                character.playerOwned = true;
            }
            // 플레이어매니저 대체하기
            PlayerManager playerManager = new PlayerManager(characters, playerGold, playerLife);

            // 아이템 임시 데이터 생성
            playerManager.addEquipItemByName("sheild");
            playerManager.addEquipItemByName("sword");
            playerManager.addEquipItemByName("scroll");
            playerManager.addEquipItemByName("ring");
            playerManager.addEquipItemByName("wand");
            playerManager.addEquipItemByName("saber");

            PlayerManager.replace(playerManager);

            // 다음씬으로 넘어가 게임 시작하기
            onClickGameStart();
        }

        /// <summary>
        /// 게임 시작 즉 다음 씬으로 넘어가기
        /// </summary>
        public void onClickGameStart()
        {
            // 다음씬으로 점프 
            // SceneManager.LoadScene("Scenes/SelectScenes/SaveLoadTestScene/SaveLoadTestScene");
            // SceneManager.LoadScene("Scenes/MapScenes/MapScene1");
            SceneManager.LoadScene("Scenes/SelectScenes/ShopTestScenes/ShopTestScene");

        }

        /// <summary>
        /// 뉴게임 버튼 클릭시의 작동 
        /// </summary>
        public void onClickNewGame()
        {
            newGameSelector.openNGSelector();
        }
    }
}
