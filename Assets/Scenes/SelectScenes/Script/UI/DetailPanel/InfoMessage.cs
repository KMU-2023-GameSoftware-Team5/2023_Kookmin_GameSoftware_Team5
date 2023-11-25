using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace deck
{
    public class InfoMessage : MonoBehaviour
    {
        public GameObject self;
        public TextMeshProUGUI infoText;
        bool isInfoOn = false;
        public float seconds = 0.3f;
        float _seconds = 0f;

        public void displayInfoMessage(string text)
        {
            isInfoOn = true;
            _seconds = 0f;
            infoText.text = text;
            self.SetActive(true);
        }

        private void Update()
        {
            if (isInfoOn)
            {
                _seconds += Time.deltaTime;
                if (seconds < _seconds)
                {
                    self.SetActive(false);
                }
            }
        }
    }

}
