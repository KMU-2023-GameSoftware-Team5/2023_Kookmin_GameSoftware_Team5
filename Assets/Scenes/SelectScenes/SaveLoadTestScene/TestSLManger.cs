using deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace testSL
{
    public class TestSLManger : MonoBehaviour
    {

        public GameObject characterNamePrefab;
        public Transform characterList;
        public GameObject itemNamePrefab;
        public Transform itemList;

        List<TestCharacterLI> characters;
        List<TestItemLI> items;


        private void Awake()
        {
            items = new List<TestItemLI>();
            characters = new List<TestCharacterLI>();

            PlayerManager pm = PlayerManager.Instance();
            /*
                PlayerManager playerManager = new PlayerManager();
                PlayerManager.Initialize(playerManager);
            */
        }

        public void onClickDestroy()
        {
            destroyUI();
            PlayerManager.delete();
        }

        public void onClickSave()
        {
            PlayerManager.save();
        }

        public void onClickLoad()
        {
            destroyUI();
            PlayerManager pm = PlayerManager.load();

            List<PixelCharacter> tmpCharacter = pm.playerCharacters;
            foreach (PixelCharacter character in tmpCharacter)
            {
                addCharacterUI(character);
            }
            List<EquipItem> tmpItem = pm.playerEquipItems;
            foreach (EquipItem item in tmpItem)
            {
                addItemUI(item);
            }
        }

        void destroyUI()
        {
            foreach (TestCharacterLI character in characters)
            {
                if(character != null)
                    character.destroy();
            }
            foreach (TestItemLI item in items)
            {
                if(item != null)
                    item.destroy();
            }
            items = new List<TestItemLI>();
            characters = new List<TestCharacterLI>();
        }

        public void onClickAddCharacter()
        {
            // 더미데이터 생성
            string[] characterNames = { "Demon", "Skeleton", "Goblin Archor" };
            System.Random random = new System.Random();
            PixelCharacter character = PlayerManager.Instance().addCharacterByName(characterNames[random.Next(0, characterNames.Length)]);
            addCharacterUI(character);
        }

        public void addCharacterUI(PixelCharacter character)
        {
            GameObject go = Instantiate(characterNamePrefab, characterList);
            TestCharacterLI tmp = go.GetComponent<TestCharacterLI>();
            tmp.Initialize(character);
            characters.Add(tmp);
        }


        public void onClickAddItem()
        {
            // 더미데이터 생성
            string[] itemNames = { "sheild", "sword", "scroll", "ring", "wand", "saber" };
            System.Random random = new System.Random();
            EquipItem item = PlayerManager.Instance().addEquipItemByName(itemNames[random.Next(0, itemNames.Length)]);
            addItemUI(item);
        }

        public void addItemUI(EquipItem item)
        {
            GameObject go = Instantiate(itemNamePrefab, itemList);
            TestItemLI tmp = go.GetComponent<TestItemLI>();
            tmp.Initialize(item);
            items.Add(tmp);
        }

        /// <summary>
        /// 테스트를 위해 새 PlayerManager 생성
        /// </summary>
        public void onClickNewManager()
        {
            destroyUI();
            PlayerManager pm = new PlayerManager();
            PlayerManager.replace(pm);
        }

        public void onClickGameStart()
        {
            SceneManager.LoadScene("Scenes/SelectScenes/SelectScene");
            // SceneManager.LoadScene("Scenes/CombineScenes/CombineScene");
        }
    }
}
