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

        public SaveLoadManager saveLoadManager;

        private void Awake()
        {
            items = new List<TestItemLI>();
            characters = new List<TestCharacterLI>();
            PlayerManager playerManager = new PlayerManager();
            playerManager.Initialize();
            PlayerManager.Initialize(playerManager);
        }

        public void onClickDestroy()
        {
            destroyUI();
            saveLoadManager.delete();
            saveLoadManager.load();
        }

        public void onClickSave()
        {
            saveLoadManager.save();
        }

        public void onClickLoad()
        {
            saveLoadManager.load();
            destroyUI();

            List<PixelCharacter> tmpCharacter = PlayerManager.Instance().playerCharacters;
            foreach (PixelCharacter character in tmpCharacter)
            {
                addCharacterUI(character);
            }
            List<EquipItem> tmpItem = PlayerManager.Instance().playerEquipItems;
            foreach (EquipItem item in tmpItem)
            {
                addItemUI(item);
            }
        }

        void destroyUI()
        {
            foreach (TestCharacterLI character in characters)
            {
                character.destroy();
            }
            foreach (TestItemLI item in items)
            {
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

        public void onClickNewManager()
        {
            destroyUI();
            PlayerManager pm = new PlayerManager();
            pm.Initialize();
            PlayerManager.Initialize(pm);
        }

        public void onClickGameStart()
        {
            SceneManager.LoadScene("Scenes/SelectScenes/SelectScene");
            // SceneManager.LoadScene("Scenes/CombineScenes/CombineScene");
        }
    }
}
