using deck;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpriteLoader : MonoBehaviour
{
    /// <summary>
    /// 좌우반전
    /// </summary>
    public bool characterViewLeft = true;
    [SerializeField] Image image;

    public void loadCharacterSprite(string characterName)
    {
        if(characterViewLeft)
        {
            image.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        image.sprite = MyDeckFactory.Instance().getSprite(characterName);
        image.color = Color.white;

    }
}
