using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class DifficultyOptionItem : MonoBehaviour
    {
        [SerializeField] List<DifficultyButton> dButtons;
        [SerializeField] NewGameSceneManager sceneManager;
        public int idx=3;

        void Start()
        {
            dButtons[2].pressed();
        }

        public void pressed(int idx)
        {
            foreach(DifficultyButton button in dButtons)
            {
                button.upPressed();
            }
            this.idx  = idx;
            sceneManager.pressedOption();
        }
    }

}
