using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

namespace Assets.PixelHeroes.Scripts.ExampleScripts
{
    public class CharacterSwitch : MonoBehaviour
    {
        public SpriteLibrary Character;
        public SpriteLibraryAsset[] Characters;

        private int _index;

        public void Update()
        {
            for (var i = 0; i < Characters.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    _index = i;
                    Character.spriteLibraryAsset = Characters[_index];
                }
            }

            if (Input.GetKeyDown(KeyCode.Minus))
            {
                _index--;

                if (_index < 0) _index = Characters.Length - 1;

                Character.GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset = Characters[_index];
            }

            if (Input.GetKeyDown(KeyCode.Equals))
            {
                _index++;

                if (_index >= Characters.Length) _index = 0;

                Character.GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset = Characters[_index];
            }
        }
    }
}