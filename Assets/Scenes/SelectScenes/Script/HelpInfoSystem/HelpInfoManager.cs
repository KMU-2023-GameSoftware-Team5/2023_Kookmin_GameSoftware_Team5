using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class HelpInfoManager : MonoBehaviour
    {
        public int idx = -1;

        [Header("help Info content")]
        [SerializeField] List<HelpInfoScriptAble> helpInfoList;

        [Header("HelpPannel")]
        [SerializeField] GameObject helpPannel;

        [Header("content")]
        [SerializeField] GameObject helpListContent;
        [SerializeField] GameObject helpContent;

        [Header("helpListContent")]
        [SerializeField] GameObject helpInfoButtonPrefab;
        [SerializeField] Transform helpInfoListTransform;

        [Header("helpContent")]
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI description;

        private void Start()
        {
            RectTransform imageRect = image.GetComponent<RectTransform>();
            float imageWidth = imageRect.rect.width;
            float height = imageWidth / 16;
            height *= 9;
            imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            for (int i = 0; i < helpInfoList.Count; i++)
            {
                GameObject go = Instantiate(helpInfoButtonPrefab, helpInfoListTransform);
                go.GetComponent<HelpInfoButton>().Initialize(i, helpInfoList[i].title, this);
            }
        }

        public void closeAll()
        {
            helpListContent.SetActive(false);
            helpContent.SetActive(false);
        }

        /// <summary>
        /// 도움말 리스트를 엽니다.
        /// </summary>
        public void openHelpPannel()
        {
            helpPannel.SetActive(true);
            openHelpInfoList();
        }

        /// <summary>
        /// 도움말의 리스트를 보여줍니다. 
        /// </summary>
        public void openHelpInfoList()
        {
            closeAll();
            helpListContent.SetActive(true);
        }

        public void nextButton(int flag)
        {
            if(flag == 0)
            {
                openHelpInfoList();
            }
            else
            {
                openHelpContent(idx + flag);
            }
        }

        public void openHelpContent(int idx)
        {
            if (idx < 0)
            {
                this.idx = helpInfoList.Count - 1;
            }
            else if (idx >= helpInfoList.Count)
            {
                this.idx = 0;
            }
            else{
                this.idx = idx;
            }
            closeAll();
            helpContent.SetActive(true);

            if (helpInfoList[this.idx].helpImg == null)
            {
                image.gameObject.SetActive(false);
            }
            else
            {
                image.gameObject.SetActive(true);
                image.sprite = helpInfoList[this.idx].helpImg;
            }
            
            title.text = helpInfoList[this.idx].title;
            description.text = helpInfoList[this.idx].description;
        }
    }

}
