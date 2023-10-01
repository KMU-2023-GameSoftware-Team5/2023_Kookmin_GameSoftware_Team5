using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;
using System;

namespace deck
{
    /// <summary>
    /// PixelCharacter의 구현체. PixelHumanoid와 호환
    /// </summary>
    public class PixelHumanoid : PixelCharacter
    {
        public PixelHumanoid(string nickname)
        {
            // 더미데이터 생성
            string[] characterNames = { "Demon", "Skeleton", "Goblin Archor" };
            System.Random random = new System.Random();
            
            characterName = characterNames[random.Next(0, characterNames.Length)];
            characterNickName = nickname;

            worldPosition = new Vector3(UnityEngine.Random.Range(-8, -2), 0.0f, UnityEngine.Random.Range(-8, 6));

            // 캐릭터 스텟정보 추출
            characterStat = new CommonStats();
            characterStat.CopyFrom(MyDeckFactory.Instance().getPixelHumanoidData(characterName));
            
            // 아이템 인벤토리
            Inventory = new EquipItem[EquipItemManager.MAX_INVENTORY_SIZE];
        }

    }
}
