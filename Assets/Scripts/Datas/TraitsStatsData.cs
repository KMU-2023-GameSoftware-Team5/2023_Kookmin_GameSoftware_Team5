using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraitsStats", menuName = "data/TraitsStats", order = 1)]
public class TraitsStats : ScriptableObject
{
    public static CommonStats InvalidStats = new CommonStats(0);

    public CommonStats GetStats(ETraits traits, int count)
    {
        if (traits == ETraits.Goblin)
        {
            if (count == 2)
                return Goblin2;
            else if (count == 3)
                return Goblin3;
            else if (count == 4)
                return Goblin4;
            else if (count == 5)
                return Goblin5;
        }
        else if (traits == ETraits.Skeleton)
        {
            if (count == 2)
                return Skeleton2;
            else if (count == 3)
                return Skeleton3;
            else if (count == 4)
                return Skeleton4;
            else if (count == 5)
                return Skeleton5;
        }
        else if (traits == ETraits.Demon)
        {
            if (count == 2)
                return Demon2;
            else if (count == 3)
                return Demon3;
            else if (count == 4)
                return Demon4;
            else if (count == 5)
                return Demon5;
        }
        else if (traits == ETraits.Human)
        {
            if (count == 2)
                return Human2;
            else if (count == 3)
                return Human3;
            else if (count == 4)
                return Human4;
            else if (count == 5)
                return Human5;
        }
        else if (traits == ETraits.Elf)
        {
            if (count == 2)
                return Elf2;
            else if (count == 3)
                return Elf3;
            else if (count == 4)
                return Elf4;
            else if (count == 5)
                return Elf5;
        }

        return InvalidStats;
    }

    public CommonStats Goblin2;
    public CommonStats Goblin3;
    public CommonStats Goblin4;
    public CommonStats Goblin5;

    public CommonStats Skeleton2;
    public CommonStats Skeleton3;
    public CommonStats Skeleton4;
    public CommonStats Skeleton5;

    public CommonStats Demon2;
    public CommonStats Demon3;
    public CommonStats Demon4;
    public CommonStats Demon5;

    public CommonStats Human2;
    public CommonStats Human3;
    public CommonStats Human4;
    public CommonStats Human5;

    public CommonStats Elf2;
    public CommonStats Elf3;
    public CommonStats Elf4;
    public CommonStats Elf5;
}
