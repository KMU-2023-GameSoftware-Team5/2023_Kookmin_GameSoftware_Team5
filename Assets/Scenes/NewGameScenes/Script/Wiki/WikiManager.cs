using data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 씬 시작시 도감을 생성하는 역할 
    /// </summary>
    public class WikiManager : MonoBehaviour
    {
        public Dictionary<string, PixelHumanoidData> characterWiki;
        public Dictionary<string, ItemData> itemWiki;

        List<string> characterKeys;
        List<string> itemKeys;

        [Header("prefab")]
        public GameObject wikiItemPrefab;
        public GameObject wikiCharacterPrefab;

        [Header("SetActive Area")]
        public GameObject wikiUI;
        public GameObject characterWikiUI;
        public GameObject itemWikiUI;

        [Header("WikiInfoList")]
        public Transform characterWikiList;
        public Transform itemWikiList;

        // Start is called before the first frame update
        void Start()
        {
            characterWiki = MyDeckFactory.Instance().m_humanoidDataMap;
            itemWiki = MyDeckFactory.Instance().itemDataMap;
            
            characterKeys = new List<string>();
            foreach (string key in characterWiki.Keys)
            {
                characterKeys.Add(key);
                GameObject go = Instantiate(wikiCharacterPrefab, characterWikiList);
                go.GetComponent<WikiCharacter>().Initialize(characterKeys.Count - 1, characterWiki[key], this);
            }


            // 아이템 초기화 
            itemKeys = new List<string>();
            foreach (string  key in itemWiki.Keys)
            {
                itemKeys.Add(key);
                GameObject go = Instantiate(wikiItemPrefab, itemWikiList);
                go.GetComponent<WikiItem>().Initialize(itemKeys.Count-1, itemWiki[key].iconImage, key, itemWiki[key].description, this);
            }
        }

        public void openCharacterWiki()
        {
            wikiUI.SetActive(true);
            characterWikiUI.SetActive(true);
            itemWikiUI.SetActive(false);
        }

        public void openItemWiki()
        {
            wikiUI.SetActive(true);
            characterWikiUI.SetActive(false);
            itemWikiUI.SetActive(true);
        }
    }

}
