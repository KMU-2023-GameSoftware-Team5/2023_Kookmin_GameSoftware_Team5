using Castle.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    [CreateAssetMenu(fileName = "skillDescription", menuName = "shop/skillDescription", order = 4)]
    public class SkillDescription : ScriptableObject
    {
        public battle.PixelHumanoid.ESkill type;
        public string desc;
    }
}
