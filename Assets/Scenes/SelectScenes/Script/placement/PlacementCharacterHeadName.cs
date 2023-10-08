using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace placement
{
    /// <summary>
    /// 캐릭터 배치시 캐릭터 위에 위치할 이름
    /// </summary>
    public class PlacementCharacterHeadName : MonoBehaviour
    {
        public TextMeshProUGUI characterName;
        public GameObject hp;
        public GameObject mp;

        public void Initialize(string characterNickname)
        {
            characterName.text  = characterNickname;
            hp.SetActive(false);
            mp.SetActive(false);
        }

        internal void unSelect()
        {
            Destroy(gameObject);
        }

        void battleStart()
        {
            hp.SetActive(true);
            mp.SetActive(true);
            Destroy(this);
        }
    }
}
