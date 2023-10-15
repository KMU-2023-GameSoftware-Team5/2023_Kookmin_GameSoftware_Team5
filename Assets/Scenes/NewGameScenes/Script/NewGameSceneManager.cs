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

        private void Start()
        {
            MyDeckFactory.Instance().Initialize();
            saveLoadManager.GetComponent<SaveLoadManager>();
            saveLoadManager.load();
            playerManager = PlayerManager.Instance();

            List<PixelCharacter> characters = playerManager.playerCharacters;
            // 현재 보유중인 캐릭터 출력
            for (int i = 0; i < characters.Count; i++)
            {
                MyDeckFactory.Instance().createCharacterInventoryPrefeb(characters[i], characterGrid, false);
            }

        }
    }
}
