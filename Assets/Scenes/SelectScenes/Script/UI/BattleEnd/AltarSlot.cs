using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    public class AltarSlot : MonoBehaviour
    {
        public GameObject noScarificeMark;
        public GameObject characterInfo;
        public CharacterIcon characterIcon;
        public PixelCharacter character;
        BattleEndManager battleEndManager;

        public void Intialize(BattleEndManager battleEndManager)
        {
            this.battleEndManager = battleEndManager;
        }

        public void select(PixelCharacter character)
        {
            this.character = character;
            characterIcon.Initialize(character);
            // noScarificeMark.SetActive(false);
            characterInfo.SetActive(true);
        }

        public void unSelect()
        {
            this.character = null;
            // noScarificeMark.SetActive(true);
            characterInfo.SetActive(false );
        }

        public void onClickUnSelect()
        {
            if(character != null)
            {
                battleEndManager.unSacrificeCharacter(character);
            }
            else
            {
                return;
            }
        }
    }

}
