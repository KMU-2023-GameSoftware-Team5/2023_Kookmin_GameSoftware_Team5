using CharacterManager;
using DamageManager;

namespace Characters
{
    //캐릭터 샘플
    public class RokGeum : Character
    {
        public RokGeum()
        {
            this.name = "록금이";
            this.maxHp = 4320000;
            this.maxMp = 100;
            this.defaultAttackSpeed = 0.1f;
            this.defaultMoveSpeed = 0.1f;
            this.AddDefaultAttack(new Attack("물리", 432, 0, 0, 0));
            this.AddDefaultDefense(new Defense("물리", 432, 0, 0, 0.5f));
        }
    }
}