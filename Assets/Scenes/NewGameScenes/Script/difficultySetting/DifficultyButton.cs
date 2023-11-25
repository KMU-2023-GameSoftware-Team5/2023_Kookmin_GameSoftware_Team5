using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class DifficultyButton : MonoBehaviour
    {
        Color pressedColor = new Color(1f, 0.73f, 0.15f, 1f);
        Color pressedTextColor = new Color(0.812f, 0.556f, 0.365f,1f);
        Color originalTextColor = new Color(0.812f, 0.712f, 0.593f,1f);

        public int idx;
        public DifficultyOptionItem options;

        public Image buttonImage;
        public TextMeshProUGUI buttonText;

        public void pressed()
        {
            options.pressed(idx);
            buttonImage.color = pressedColor;
            buttonText.color = pressedColor;
        }

        public void upPressed()
        {
            buttonImage.color = Color.white;
            buttonText.color = originalTextColor;
        }
    }

}
