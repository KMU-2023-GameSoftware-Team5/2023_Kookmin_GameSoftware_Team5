using Castle.Core;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{

    /// <summary>
    /// 캐릭터 세부정보(공격타입, 스킬)에 대한 설명
    /// </summary>
    [CreateAssetMenu(fileName = "PixelCharacterDescription", menuName = "shop/PixelCharacterDescription", order = 2)]
    public class PixelCharacterDescription : ScriptableObject
    {
        public List<AttackTypeDescription> attackTypeDescriptions;
        public List<SkillDescription> skillDescriptions;
    }




}
