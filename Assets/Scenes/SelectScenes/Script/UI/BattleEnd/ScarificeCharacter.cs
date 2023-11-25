using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    public class SacrificeCharacter : MonoBehaviour
    {
        BattleEndManager battleEndManager;

        bool isSelected = false;
        // 위는 drag 처리를 위한 코드 
        public CharacterIcon characterIcon;
        public PixelCharacter character;
        public GameObject mark;


        public void Initialize(PixelCharacter character, BattleEndManager battleEndManager)
        {
            this.character = character;
            characterIcon.Initialize(character);
            this.battleEndManager = battleEndManager;
        }

        public void onClickScarifice()
        {
            if(!isSelected) {
                isSelected = battleEndManager.sacrificeCharacter(character);
                mark.SetActive(isSelected);
            }
            else
            {
                // unSelect
                battleEndManager.unSacrificeCharacter(character);
//                isSelected = false; // 이거는 battleManager가 해줌 
//                mark.SetActive(isSelected);
            }
        }

        public void unSelect()
        {
            isSelected = false;
            mark.SetActive(isSelected);
        }
    }

}
