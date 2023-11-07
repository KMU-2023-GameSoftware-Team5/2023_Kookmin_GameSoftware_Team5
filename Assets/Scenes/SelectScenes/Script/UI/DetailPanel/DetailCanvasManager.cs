using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deck;

namespace deck
{
    public class DetailCanvasManager : MonoBehaviour
    {
        /// <summary>
        /// CharacterDetailPanel
        /// </summary>
        [SerializeField] CharacterDetails characterDetailPanel;
        [SerializeField] EquipItemDetails equipItemPanel;
        public void openCharacterDetail(PixelCharacter character)
        {
            characterDetailPanel.gameObject.SetActive(true);
            characterDetailPanel.openCharacterDetails(character);
        }

        public void openItemDetail(EquipItem item) {
            equipItemPanel.gameObject.SetActive(true);
            equipItemPanel.openItemDetail(item);
        }
    }
}
