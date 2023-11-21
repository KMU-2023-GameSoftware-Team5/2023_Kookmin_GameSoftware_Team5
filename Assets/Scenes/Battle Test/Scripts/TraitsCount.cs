using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battle
{
    public class TraitsCount
    {
        public TraitsCount(List<PixelCharacter> list)
        {
            GoblinCount = 0;
            SkeletonCount = 0;
            DemonCount = 0;
            HumanCount = 0;
            ElfCount = 0;

            foreach (PixelCharacter character in list)
            {
                if (character == null) continue;

                switch (character.traits)
                {
                    case ETraits.Goblin:
                        GoblinCount++;
                        break;
                    case ETraits.Skeleton:
                        SkeletonCount++;
                        break;
                    case ETraits.Demon:
                        DemonCount++;
                        break;
                    case ETraits.Human:
                        HumanCount++;
                        break;
                    case ETraits.Elf:
                        ElfCount++;
                        break;
                }
            }
        }

        public CommonStats GetTraitsStats()
        {
            CommonStats ret = new CommonStats();

            TraitsStats traitsStats = StaticLoader.Instance().GetTraitsStats();

            ret.Add(traitsStats.GetStats(ETraits.Goblin, GoblinCount));
            ret.Add(traitsStats.GetStats(ETraits.Skeleton, SkeletonCount));
            ret.Add(traitsStats.GetStats(ETraits.Demon, DemonCount));
            ret.Add(traitsStats.GetStats(ETraits.Human, HumanCount));
            ret.Add(traitsStats.GetStats(ETraits.Elf, ElfCount));

            return ret;
        }

        public int GoblinCount;
        public int SkeletonCount;
        public int DemonCount;
        public int HumanCount;
        public int ElfCount;
    }
}
