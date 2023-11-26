using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Buffs.Potions
{
    public class QuarionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += 0.5f;
            player.GetDamage(DamageClass.Magic) += 0.5f;
            player.GetDamage(DamageClass.Ranged) += 0.5f;
            player.GetDamage(DamageClass.Throwing) += 0.5f;
            player.statLifeMax2 /= 2;
            player.moveSpeed *= 1.12f;
        }
    }
}