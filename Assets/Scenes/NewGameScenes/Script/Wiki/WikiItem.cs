using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class WikiItem : MonoBehaviour
    {
        public Image wikiImage;
        public TextMeshProUGUI wikiTitle;
        public TextMeshProUGUI wikiText;
        int idx;
        WikiManager wikiManager;

        public void Initialize(int idx,Sprite wikiImage, string wikiTitle, string wikiText, WikiManager wikiManager)
        {
            this.wikiImage.sprite = wikiImage;
            this.wikiTitle.text = wikiTitle; 
            this.wikiText.text = wikiText;
            this.idx = idx;
        }

        public void onClickWikiDetail()
        {
            Debug.Log("openWikiDetail");
        }
    }

}
