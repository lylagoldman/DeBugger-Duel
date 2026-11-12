using UnityEngine;

public static class AttackColor
{
    // Purple and green are equally likely; red overrides both with a small chance.
    public static FxColor Roll(float redChance = 0.1f)
    {
        if (Random.value < redChance) return FxColor.Red;
        return Random.value < 0.5f ? FxColor.Purple : FxColor.Green;
    }
}