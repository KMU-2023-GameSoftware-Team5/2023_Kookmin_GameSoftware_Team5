using System.Collections;
using System.Collections.Generic;
using testSL;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace deck
{
    public class NewGameSceneManager : MonoBehaviour
    {

        [Header("intial setting")]
        [SerializeField] int playerGold;
        [SerializeField] int playerLife;

        [Header("saveLoad System")]
        [SerializeField] SaveLoadManager saveLoadManager;
        PlayerManager playerManager;

        [Header("current Player Status")]
        [SerializeField] Transform characterGrid;
        [SerializeField] Transform itemGrid;

        [Header("New Game")]
        [SerializeField] NewGameSelectorList newGameSelector;


        private void Start()
        {
            saveLoadManager.GetComponent<SaveLoadManager>();
            saveLoadManager.load();
            playerManager = PlayerManager.Instance();

            List<PixelCharacter> characters = playerManager.playerCharacters;
            List<EquipItem> items = playerManager.playerEquipItems;
            // 현재 보유중인 캐릭터 출력
            for (int i = 0; i < characters.Count; i++)
            {
                MyDeckFactory.Instance().createCharacterInventoryPrefeb(characters[i], characterGrid);
            }
            // 플레이어 보유 아이템에 대한 UI 생성
            foreach (EquipItem item in items)
            {
                MyDeckFactory.Instance().createLightEquipItemUI(item, itemGrid);
            }

            newGameSelector.Initialize(this);
        } 

        public void newGameStart(List<PixelCharacter> characters)
        {
            saveLoadManager.delete();
            PlayerManager pm = new PlayerManager();
            pm.Initialize(playerGold, playerLife, characters, null);
            PlayerManager.Initialize(pm);
            saveLoadManager.save();
            onClickGameStart();
        }

        public void onClickGameStart()
        {
            // 다음씬으로 점프 
            SceneManager.LoadScene("Scenes/SelectScenes/SaveLoadTestScene/SaveLoadTestScene");
        }

        public void onClickNewGame()
        {
            newGameSelector.openNGSelector();
        }
    }
}
