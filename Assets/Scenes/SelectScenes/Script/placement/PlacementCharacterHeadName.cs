using deck;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace placement
{
    /// <summary>
    /// 캐릭터 배치시 캐릭터 위에 위치할 이름
    /// </summary>
    public class PlacementCharacterHeadName : MonoBehaviour
    {
        public GameObject headName;
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI characterUpgrade;
        public GameObject hp;
        public GameObject mp;
        public PixelCharacter character;
        public Image bg; 

        public void Initialize(deck.PixelCharacter character)
        {
            this.character = character;
            characterName.text  = character.characterNickName;
            characterUpgrade.text = $"★{character.tier}";
            hp.SetActive(false);
            mp.SetActive(false);
            MyDeckFactory.Instance().nickNameChangeEvent.AddListener(onNickNameChange);
            if(character.tier > 1)
            {
                Color bgColor = MyDeckFactory.Instance().tierColors[character.tier - 1];
                bgColor.a = 0.2f;
                bg.color = bgColor;
            }
        }

        internal void unSelect()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// HeadBar의 hp, mp 추가, HeadName삭제
        /// </summary>
        public void battleStart()
        {
            hp.SetActive(true);
            mp.SetActive(true);
            Destroy(headName);
            Destroy(this);
        }

        public void onNickNameChange(string id)
        {
            if (character.ID == id)
            {
               characterName.text = character.characterNickName;
            }
        }

    }
}
