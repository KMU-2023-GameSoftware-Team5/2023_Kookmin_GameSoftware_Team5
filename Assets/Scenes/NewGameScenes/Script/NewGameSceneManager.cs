using System.Collections;
using System.Collections.Generic;
using testSL;
using UnityEngine;


namespace deck
{
    public class NewGameSceneManager : MonoBehaviour
    {
        [SerializeField] SaveLoadManager saveLoadManager;
        PlayerManager playerManager;

        [SerializeField] Transform characterGrid;
        [SerializeField] Transform itemGrid;

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

        }
    }
}
