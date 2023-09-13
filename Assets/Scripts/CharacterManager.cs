using DamageManager;

namespace CharacterManager
{
    public class Character
    {

        public string name;
        public string description;

        public int maxHp = 100;
        public int maxMp = 100;
        public float defaultAttackSpeed = 1;
        public float defaultMoveSpeed = 1;
        public Attack[] defaultAttack;
        public Defense[] defaultDefense;

        public int hp = 100;
        public int mp = 100;
        public float attackSpeed = 1;
        public float moveSpeed = 1;
        public Attack[] attack;
        public Defense[] defense;
        //Item[] item;

        public Character(){
            defaultAttack = new Attack[Types.TypeIndex.Length];
            attack = new Attack[Types.TypeIndex.Length];
            defaultDefense = new Defense[Types.TypeIndex.Length];
            defense = new Defense[Types.TypeIndex.Length];
        }

        //기본공격 추가
        protected void AddDefaultAttack(Attack newAttack)
        {

        }

        //기본방어 추가
        protected void AddDefaultDefense(Defense newDefense)
        {

        }

        //장비, 스탯 등 적용
        private void Refresh()
        {

        }


        /*
        //아이템 추가
        public void EquipItem(Item equip)
        {

        }

        //아이템 제거
        public void UnequipItem(Item unequip)
        {

        }
        */
    }
}