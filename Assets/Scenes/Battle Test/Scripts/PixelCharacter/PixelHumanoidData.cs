using Assets.PixelHeroes.Scripts.CharacterScrips;
using UnityEngine;

namespace lee
{
    public enum EDefualtAttackType
    {
        Melee,
        RangedShot,    // 활
        RangedFire1,    // 한손 총
        RangedFire2,    // 양손 총
    }

    [CreateAssetMenu(fileName = "PixelHumanoidData", menuName = "lee/PixelHumanoidData", order = 1)]
    public class PixelHumanoidData: ScriptableObject
    {
        // WARNUNG: MUST BE UNIQUE!
        public string characterName = "unique_name";

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

        [Header("Status")]
        public int hp;
        public int damage;
        public float walkSpeed;
        public float attackRange;
        public float attackDelay;
        public float criticalRate;

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
