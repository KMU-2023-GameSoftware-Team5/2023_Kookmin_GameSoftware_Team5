using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class SpriteResizerForUI : MonoBehaviour
    {
        [Header("size of Sprite")]
        public int width;
        public int height;

        [SerializeField] SpriteRenderer spriteRenderer;

        public void resizeSprite()
        {
            spriteRenderer.size = new Vector2(width, height);
        }
    }

}
