using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class CharacterDetailTabManager : MonoBehaviour
    {
        [SerializeField] GameObject characterDescription;
        [SerializeField] CharacterDetailItemList itemList;
        [SerializeField] GameObject characterSkill;

        void unActiveAll()
        {
            characterDescription.SetActive(false);
            itemList.closeItemList();
            characterSkill.SetActive(false);
        }

        public void openItemList()
        {
            unActiveAll();
            itemList.openItemList();    
        }

        public void openCharacterDescription()
        {
            unActiveAll();
            characterDescription.SetActive(true);
        }

        public void openCharacterSkill()
        {
            unActiveAll();
            characterSkill.SetActive(true);
        }
    }

}
