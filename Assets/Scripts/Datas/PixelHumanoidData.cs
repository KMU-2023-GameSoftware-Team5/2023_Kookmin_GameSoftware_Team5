using Assets.PixelHeroes.Scripts.CharacterScrips;
using battle;
using UnityEngine;

namespace data
{
    public enum EDefualtAttackType
    {
        Melee,
        RangedShot,    // 활
        RangedFire1,    // 한손 총
        RangedFire2,    // 양손 총
    }

    public enum ETraits
    {
        Goblin,
        Skeleton,
        Demon, 
        Human,
        Elf
    }

    [CreateAssetMenu(fileName = "PixelHumanoidData", menuName = "data/PixelHumanoid", order = 1)]
    public class PixelHumanoidData: scriptable.CommonStats
    {
        [Header("PixelHumanoidData")]
        // WARNUNG: MUST BE UNIQUE!
        public string characterName = "unique_name";
        public float attackRange;
        public ETraits traits;

        [Header("Appearance")]
        public string Head = "Human";
        public string Ears = "Human";
        public string Eyes = "Human";
        public string Body = "Human";
        public string Hair;
        public string Armor;
        public string Helmet;
        public string Weapon;
        public string Shield;
        public string Cape;
        public string Back;
        public string Mask;

        [Header("Setting")]
        public EDefualtAttackType defualtAttackType;
        public bool useCustomSkil;
        public PixelHumanoid.ESkill skill;
        public string customSkillName;

        public void SetOutToBuilder(CharacterBuilder builder)
        {
            builder.Head = Head;
            builder.Ears = Ears;
            builder.Eyes = Eyes;
            builder.Body = Body;
            builder.Hair = Hair;
            builder.Armor = Armor;
            builder.Helmet = Helmet;
            builder.Weapon = Weapon;
            builder.Shield = Shield;
            builder.Cape = Cape;
            builder.Back = Back;
            builder.Mask = Mask;
        }
    }
}
