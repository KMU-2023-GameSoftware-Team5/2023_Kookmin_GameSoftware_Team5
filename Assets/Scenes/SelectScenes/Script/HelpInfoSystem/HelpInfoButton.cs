using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace deck
{
    public class HelpInfoButton : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public int idx;
        public HelpInfoManager helpInfoManager;

        public void Initialize(int idx, string title ,HelpInfoManager helpInfoManager)
        {
            this.idx = idx;
            this.title.text = title;
            this.helpInfoManager = helpInfoManager;
        }

        public void onClickHelpInfo()
        {
            helpInfoManager.openHelpContent(idx);
        }

    }

}
